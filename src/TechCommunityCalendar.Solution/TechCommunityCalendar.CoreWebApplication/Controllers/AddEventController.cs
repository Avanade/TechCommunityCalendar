using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class AddEventController : Controller
    {
        readonly IWebHostEnvironment currentEnvironment;

        static ITechEventAdminRepository _techEventAdminRepository;

        public AddEventController(IWebHostEnvironment env,
            ITechEventAdminRepository techEventAdminRepository)
        {
            currentEnvironment = env;
            _techEventAdminRepository = techEventAdminRepository;
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

                if(model.Name.Contains("#"))
                {
                    ModelState.AddModelError("Name", "Event Name cannot contain a hash sign (#)");
                }

                if (model.Name.Contains("?"))
                {
                    ModelState.AddModelError("Name", "Event Name cannot contain a question mark (?)");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.City))
            {
                if (model.City.Contains(","))
                {
                    ModelState.AddModelError("City", "City cannot contain a comma (,)");
                }
            }

            // Make sure End Date is after Start Date

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date cannot be before the start date");
            }

            // Calculate Duration
            if (model.StartDate == model.EndDate)
            {
                model.Duration = "1 day";
            }
            else if (model.EndDate.Subtract(model.StartDate).TotalHours <= 7)
            {
                model.Duration = (model.EndDate.Subtract(model.StartDate).TotalHours).ToString("0.0") + " hour";
            }
            else
            {
                model.Duration = (model.EndDate.Subtract(model.StartDate).TotalDays + 1).ToString("0.0") + " day";
            }

            // Twitter Handle?
            if (!string.IsNullOrWhiteSpace(model.TwitterHandle))
            {
                model.TwitterHandle = model.TwitterHandle.Trim();
                if (model.TwitterHandle.StartsWith("@"))
                {
                    ModelState.AddModelError("TwitterHandle", "Twitter Handles cannot start with @");
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            // Add to Database
            await AddToDatabase(model);

            // To add to csv in git repository
            //await CreatePullRequest(model);

            return View("Success", model);
        }

        private static async Task AddToDatabase(AddEventViewModel model)
        {
            var techEvent = new TechEvent();
            techEvent.Name = model.Name;
            techEvent.City = model.City ?? String.Empty;
            techEvent.Country = model.Country;
            techEvent.Duration = model.Duration;
            techEvent.StartDate = model.StartDate;
            techEvent.EndDate = model.EndDate;
            techEvent.EventFormat = (EventFormat)Enum.Parse(typeof(EventFormat), model.EventFormat);
            techEvent.EventType = (EventType)Enum.Parse(typeof(EventType), model.EventType);
            techEvent.TwitterHandle = model.TwitterHandle;
            techEvent.Url = model.Url;
            techEvent.Hidden = true;

            await _techEventAdminRepository.Add(techEvent);
        }

        private static async Task CreatePullRequest(AddEventViewModel model)
        {
            // Create file with details..
            var row = $"{model.Name},{model.EventType},{model.StartDate},{model.EndDate},{model.Duration},{model.Url},{model.EventFormat},{model.City},{model.Country},{model.TwitterHandle}";
            var gitHubClient = new GitHubClient(new ProductHeaderValue("TechCommunityCalendarApp"));
            var personalAccessKey = Environment.GetEnvironmentVariable("GitHubPersonalAccessKey");
            gitHubClient.Credentials = new Credentials(personalAccessKey);

            var repositoryName = "TechCommunityCalendar";
            var ownerName = "Avanade";

            // Create new branch
            var branchName = TechEventCleaner.MakeFriendlyBranchName($"new-event-{model.Name}");
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
        }

    }
}
