using Microsoft.AspNetCore.Mvc;
using Onatrix_Umbraco.Services;
using Onatrix_Umbraco.ViewModels;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;           
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Onatrix_Umbraco.Controllers
{
    public class FormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        FormSubmissionsService formSubmissions,
        EmailService emailService
        ) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        private readonly FormSubmissionsService _formSubmissions = formSubmissions;
        private readonly EmailService _emailService = emailService;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleQuestionForm(QuestionFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["FormError"] = "Please fill in all required fields.";
                return CurrentUmbracoPage();
            }
            var result = _formSubmissions.SaveQuestionRequest(model);
            if (!result)
            {
                TempData["FormError"] = "There was an error while saving your request.";
                return RedirectToCurrentUmbracoPage();
            }
            try
            {
               
                //// Confirmation email to the user
                //var confirmMessage = new EmailMessage(
                //    from: "no-reply@onatrix.com",
                //    to: model.Email,
                //    subject: "We received your question!",
                //    body: $@"
                //        <p>Hi {model.Name},</p>
                //        <p>Thank you for reaching out. We’ve received your question and will get back to you soon.</p>
                //        <p><em>Your question:</em> {model.Question}</p>
                //        <p>Best regards,<br/>The Onatrix Team</p>",
                //   true
                //);

                await _emailService.SendEmailAsync(model.Email);

                TempData["FormSuccess"] = "Thank you! Your question has been submitted successfully.";
            }
            catch 
            {
                TempData["FormError"] = "There was an issue sending your message. Please try again later.";
            }
            

            return RedirectToCurrentUmbracoPage();
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleCallbackForm(CallbackFormViewModel model)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            var saved = _formSubmissions.SaveCallbackRequest(model);
            if (!saved)
            {
                TempData["FormError"] = "There was an error while submitting your request. Please try again later.";
                return RedirectToCurrentUmbracoPage();
            }

            //// Email confirmation message
            //var confirmMessage = new EmailMessage(
            //    "no-reply@onatrix.com",
            //    model.Email,
            //    "Thanks for getting in touch!",
            //    $@"
            //        <p>Hi {model.Name},</p>
            //        <p>We’ve received your callback request about <strong>{model.SelectedOption}</strong>.</p>
            //        <p>We’ll contact you at {model.Phone} soon.</p>
            //        <p>Best regards,<br/>The Onatrix Team</p>",
            //    true
            //);


            await _emailService.SendEmailAsync(model.Email);

            TempData["FormSuccess"] = "Thank you! Your request has been received. We will get back to you soon.";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
