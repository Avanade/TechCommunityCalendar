using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.CoreWebApplication.Models;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class AddEventController : Controller
    {
        IWebHostEnvironment currentEnvironment;

        public AddEventController(IWebHostEnvironment env)
        {
            currentEnvironment = env;
        }

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

            // Append line to existing file
            var targetFilePath = $"src/TechCommunityCalendar.Solution/TechCommunityCalendar.CoreWebApplication/wwwroot/Data/TechEvents.csv";
            var currentFileText = await System.IO.File.ReadAllTextAsync(Path.Combine(currentEnvironment.WebRootPath, "Data", "TechEvents.csv"));
            var contents = await gitHubClient.Repository.Content.GetAllContentsByRef(ownerName, repositoryName, targetFilePath, branchName);
            var targetFile = contents[0];

            if (targetFile.EncodedContent != null)
            {
                currentFileText = Encoding.UTF8.GetString(Convert.FromBase64String(targetFile.EncodedContent));
            }
            else
            {
                currentFileText = targetFile.Content;
            }

            var newFileText = string.Format("{0}\n{1}", currentFileText, row);
            var updateRequest = new UpdateFileRequest("Updating TechEvents.csv", newFileText, targetFile.Sha, branchName);
            var updatefile = await gitHubClient.Repository.Content.UpdateFile(ownerName, repositoryName, targetFilePath, updateRequest);

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
