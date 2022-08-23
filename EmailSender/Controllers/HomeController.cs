using EmailSender.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using System.Net;

namespace EmailSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult EmailAttachment()
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Test Project", "enykoruna1@gmail.com"));
            //кому отправляем
            message.To.Add(new MailboxAddress("Tom", "enykoruna4@rambler.ru"));
            //тема письма
            message.Subject = "Тестовое письмо для приятеля!";
            //тело письма
            message.Body = new TextPart("plain")
            {
                Text = "Доброго времени суток. Если вы получили это письмо, значит у меня все хорошо.",
            };
            //добавление вложения
            MemoryStream memoryStream = new MemoryStream();
            BodyBuilder bb = new BodyBuilder();
            using (var wc = new WebClient())
            {
                bb.Attachments.Add("Email.pdf",
                wc.DownloadData("wwwroot/pdf-test.pdf"));
            }
            message.Body = bb.ToMessageBody();
            //конец добавления вложения
            using (var client = new SmtpClient())
            {
                //Указываем smtp сервер почты и порт
                client.Connect("smtp.gmail.com", 587, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate("enykoruna1@gmail.com", "egweflcsvbxgnffd");

                client.Send(message);
                client.Disconnect(true);
            }
            return View("Email");
        }


        [HttpGet]
        public IActionResult Email()
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Test Project", "mail@gmail.com"));
            //кому отправляем
            message.To.Add(new MailboxAddress("Tom", "enykoruna4@rambler.ru"));
            //тема письма
            message.Subject = "Тестовое письмо для приятеля!";
            //тело письма
            message.Body = new TextPart("plain")
            {
                Text = "Доброго времени суток. Если вы получили это письмо, значит у меня все хорошо.",
            };
            using (var client = new SmtpClient())
            {
                //Указываем smtp сервер почты и порт
                client.Connect("smtp.gmail.com", 587, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate("mail@gmail.com", "egweflcsvbxgnffd");

                client.Send(message);
                client.Disconnect(true);
            }
            return View();
        }
    }
}