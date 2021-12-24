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
        readonly IWebHostEnvironment currentEnvironment;

        public AddEventController(IWebHostEnvironment env)
        {
            currentEnvironment = env;
        }

        public IActionResult Index()
        {
            ViewBag.Canonical = "https://TechCommunityCalendar.com/AddEvent/";
            ViewBag.Title = "Tech Community Calendar : Add Event";
            ViewBag.Description = "A calendar list of upcoming Conferences, Meetups and Hackathons in the Tech Community";

            var model = new AddEventViewModel();
            model.StartDate = DateTime.Now.Date;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(AddEventViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                if (model.Name.Contains(","))
                {
                    ModelState.AddModelError("Name", "Event Name cannot contain a comma (,)");
                }
            }

            //if (!string.IsNullOrWhiteSpace(model.Duration))
            //{
            //    var durationParts = model.Duration.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //    if (durationParts.Length != 2)
            //    {
            //        ModelState.AddModelError("Duration", "Event Duration format invalid");
            //    }
            //    else
            //    {
            //        if (!int.TryParse(durationParts[0], out _))
            //            ModelState.AddModelError("Duration", "Event Duration format invalid");

            //        if (durationParts[1] != "day" && durationParts[1] != "hour")
            //            ModelState.AddModelError("Duration", "Event Duration format invalid");
            //    }
            //}

            // Make sure End Date is after Start Date

            if(model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date cannot be before the start date");
            }

            // Calculate Duration
            if(model.StartDate == model.EndDate)
            {
                model.Duration = "1 day";
            }
            else if(model.EndDate.Subtract(model.StartDate).TotalHours <= 7)
            {
                model.Duration = model.EndDate.Subtract(model.StartDate).TotalHours + " hour";
            }
            else
            {
                model.Duration = model.EndDate.Subtract(model.StartDate).TotalDays + 1 + " day";
            }

            if (!ModelState.IsValid)
                return View(model);

            // Create file with details..
            var row = $"{model.Name},{model.EventType},{model.StartDate},{model.EndDate},{model.Duration},{model.Url},{model.EventFormat},{model.City},{model.Country}";
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
            var contents = await gitHubClient.Repository.Content.GetAllContentsByRef(ownerName, repositoryName, targetFilePath, branchName);
            var targetFile = contents[0];

            string currentFileText;
            if (targetFile.EncodedContent != null)
            {
                currentFileText = Encoding.UTF8.GetString(Convert.FromBase64String(targetFile.EncodedContent));
            }
            else
            {
                currentFileText = targetFile.Content;
            }

            var newFileText = string.Format("{0}\n{1}", currentFileText, row);
            var updateRequest = new UpdateFileRequest($"Adding event {model.Name} to TechEvents.csv", newFileText, targetFile.Sha, branchName);
            await gitHubClient.Repository.Content.UpdateFile(ownerName, repositoryName, targetFilePath, updateRequest);

            // Create pull request
            var headRef = $"{ownerName}:{branchName}";
            var baseRef = "main";
            var pullRequest = new NewPullRequest($"Merging {branchName} into main", headRef, baseRef);
            var pullRequestResult = await gitHubClient.Repository.PullRequest.Create(ownerName, repositoryName, pullRequest);

            model.NewPullRequestUrl = pullRequestResult.HtmlUrl;

            return View("Success", model);
        }
    }
}
