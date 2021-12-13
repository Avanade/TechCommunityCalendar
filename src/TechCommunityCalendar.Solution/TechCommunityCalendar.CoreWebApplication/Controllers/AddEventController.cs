using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;
using System.Threading.Tasks;
using TechCommunityCalendar.CoreWebApplication.Models;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class AddEventController : Controller
    {
        public IActionResult Index()
        {
            var model = new AddEventViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(AddEventViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validate the data sent..
            // Make sure there are no commas!
            // Check date format!
            // Check duration format!

            // Create file with details..
            var row = $"{model.Name},{model.EventType},{model.StartDate:dd/MM/yyyy},{model.Duration},{model.Url},{model.EventFormat},{model.Country},{model.City}";
            var gitHubClient = new GitHubClient(new ProductHeaderValue("TechCommunityCalendarApp"));
            var personalAccessKey = Environment.GetEnvironmentVariable("GitHubPersonalAccessKey");
            gitHubClient.Credentials = new Credentials(personalAccessKey);

            var repositoryName = "TechCommunityCalendar";
            var ownerName = "Avanade";

            // Create new branch
            var branchName = $"NewEvents_{Guid.NewGuid()}";
            var master = await gitHubClient.Git.Reference.Get(ownerName, repositoryName, "heads/main");

            await gitHubClient.Git.Reference.Create(ownerName, repositoryName, new NewReference($"refs/heads/{branchName}", master.Object.Sha));

            // Create commit to new branch
            var fileName = $"{Guid.NewGuid()}.csv";
            var (owner, repoName, filePath, branch) = (ownerName, repositoryName, $"src/TechCommunityCalendar.Solution/TechCommunityCalendar.CoreWebApplication/wwwroot/Data/{fileName}", branchName);

            await gitHubClient.Repository.Content.CreateFile(owner, repoName, filePath,
                 new CreateFileRequest($"Adding event file {filePath}", content: row, branch));

            // Create pull request
            var headRef = $"{ownerName}:{branchName}";
            var baseRef = "main";
            var pullRequest = new NewPullRequest($"Merging {branchName} into main", headRef, baseRef);

            await gitHubClient.Repository.PullRequest.Create(ownerName, repositoryName, pullRequest);

            // todo: Return a successful better view
            return View();
        }
    }
}
