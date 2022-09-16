// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using TechCommunityCalendar.Concretions;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var url = "C:\\Development\\TechCommunityCalendar\\src\\TechCommunityCalendar.Solution\\TechCommunityCalendar.CoreWebApplication\\wwwroot\\Data\\TechEvents.csv";

        // Get list of events
        var events = await new CsvTechEventRepository(url).GetAll();
        

        // To to url, get screenshot, save locally
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("headless");//Comment if we want to see the window. 
        var driver = new ChromeDriver("C:\\Development\\TechCommunityCalendar\\src\\TechCommunityCalendar.Solution\\ScreenshotThubmnailGenerator\\bin\\Debug\\net6.0\\", options);
        //var driver = new ChromeDriver(options);
        //driver.Manage().Window.Size = new Size(1920, 1080);
        driver.Manage().Window.Size = new Size(1366, 768);
        var userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36";
        options.AddArgument("user-agent=" + userAgent);
        

        foreach (var _event in events.OrderByDescending(x=>x.StartDate))
        {
            var filePath = @$"c:\temp\selenium\{TechEventCleaner.MakeFriendlyBranchName(_event.Name)}.png";
            var filePathSmaller = @$"c:\temp\selenium\300\{TechEventCleaner.MakeFriendlyBranchName(_event.Name)}-300.png";


            if (!File.Exists(filePath))
            {
                try
                {
                    Console.WriteLine($"Getting {_event.Url}");
                    driver.Navigate().GoToUrl(_event.Url);
                    Thread.Sleep(3000);
                    var screenshot = (driver as ITakesScreenshot).GetScreenshot();
                    screenshot.SaveAsFile(filePath);

                    Resize(filePath, filePathSmaller, 0.25);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        driver.Close();
        driver.Quit();        
    }

    static void Resize(string imageFile, string outputFile, double scaleFactor)
    {
        

        using (var srcImage = Image.FromFile(imageFile))
        {
            var newWidth = (int)(srcImage.Width * scaleFactor);
            var newHeight = (int)(srcImage.Height * scaleFactor);
          
            using (var newImage = new Bitmap(newWidth, newHeight))
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                newImage.Save(outputFile);
            }
        }
    }
}