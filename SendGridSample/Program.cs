using SendGrid;
using SendGrid.Helpers.Mail;

var apiKey = "apikey...";

var client = new SendGridClient(apiKey);

var to = new EmailAddress("odbiorca@wp.pl","Odbiorca");
var from = new EmailAddress("nadawca@wp.pl", "Nadawca");

var subject = "Test email from sendgrid";

var htmlContent = "<strong>Hello emial form sendgrid!</strong>";

var message = MailHelper.CreateSingleTemplateEmail(from, to, "d-234212b932873r3j379", new
{
    Title = "Test title",
    Description = "Sample description"
});

var response =  await client.SendEmailAsync(message);

var responseBody = await response.Body.ReadAsStringAsync();

Console.WriteLine("Email sent.");
Console.WriteLine(response.IsSuccessStatusCode);
Console.WriteLine(responseBody);