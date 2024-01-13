using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Mail;

string Url = "https://www.basketball-reference.com/boxscores/";
string EmailRecipient = "recipient@yahoo.com";
string EmailSender = "example@gmail.com";
string EmailPassword = "xxx";


// Create a timer to run the service once a day
using (var timer = new Timer(async (state) =>
{
    try
    {
        // Scrape the data from the website
        var data = await ScrapeDataFromWebsiteAsync(Url);
        Console.WriteLine($"Title: {data}");

        // Send an email with the data
        await SendEmailAsync(data, EmailRecipient, EmailSender, EmailPassword);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message} - {ex.StackTrace}");
    }
}, null, (int)TimeSpan.Zero.TotalMilliseconds, (int)TimeSpan.FromSeconds(10).TotalMilliseconds))
{
    // Keep the application running to prevent it from exiting
    Console.ReadLine();
}

static async Task<string> ScrapeDataFromWebsiteAsync(string url)
{
    // Scrape the data from the website using HtmlAgilityPack
    // Replace this with your actual web scraping function
    // await Task.Delay(1000);
    HtmlDocument doc = await LoadHtmlDocument(url);
    string title = ExtractTitle(doc);
    return title;
}

static async Task SendEmailAsync(string data, string recipient, string sender, string password)
{
    // Send an email with the scraped data
    // Replace this with your actual email sending function
    MailMessage message = new(
        from: sender,
        to: recipient,
        subject: "Website Data",
        body: data
    );

    // Create an SmtpClient object
    SmtpClient smtpClient = new(
        host: "smtp.gmail.com",
        port: 587
    )
    {
        Credentials = new NetworkCredential(sender, password),
        EnableSsl = true
    };

    await smtpClient.SendMailAsync(message);
}

static async Task<HtmlDocument> LoadHtmlDocument(string url)
{
    // Create a WebClient instance
    using HttpClient client = new();

    // Download the HTML document
    string html = await client.GetStringAsync(url);

    // Create an HtmlDocument object
    HtmlDocument doc = new();

    // Load the HTML document into the object
    doc.LoadHtml(html);

    // Return the HtmlDocument object
    return doc;
}

static string ExtractTitle(HtmlDocument doc)
{
    // Find the title element
    HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//title");

    // Get the text of the title
    string title = titleNode.InnerText;

    // Return the title
    return title;
}