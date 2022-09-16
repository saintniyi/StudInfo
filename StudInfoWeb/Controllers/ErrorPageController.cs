using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StudInfoWeb.Controllers
{

    [AllowAnonymous]
    public class ErrorPageController : Controller
    {
        private readonly ILogger<ErrorPageController> _logger;

        public ErrorPageController(ILogger<ErrorPageController> logger)
        {
            _logger = logger;
        }


        [AllowAnonymous]
        [Route("Error/{statusCode}")]
        public IActionResult ErrorStatus(int? statusCode = null)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 404:
                        //ViewData["ErrorMessage"] = "Error 404 - Sorry, the resource you requested could not be found";
                        //ViewBag.Path = statusCodeResult.OriginalPath;
                        //ViewBag.QS = statusCodeResult.OriginalQueryString;
                        ViewData["statusCode"] = statusCode;

                        if (statusCodeResult != null)
                        {
                            _logger.LogError($"404 Error Occured. Path = {statusCodeResult.OriginalPath}" +
                                $" and QueryString = {statusCodeResult.OriginalQueryString}");
                        }

                        break;
                    case 500:
                        //ViewData["ErrorMessage"] = "Error 500 - An error has occured. Please contact the vendor at: help@fixit.com";
                        //ViewBag.Path = statusCodeResult.OriginalPath;
                        //ViewBag.QS = statusCodeResult.OriginalQueryString;
                        ViewData["statusCode"] = statusCode;

                        if (statusCodeResult != null)
                        {
                            _logger.LogError($"500 Error Occured. Path = {statusCodeResult.OriginalPath}" +
                                $" and QueryString = {statusCodeResult.OriginalQueryString}");
                        }

                        break;

                    default:
                        ViewData["statusCode"] = statusCode;
                        if (statusCodeResult != null)
                        {
                            _logger.LogError($"Unknown Error Occured. Path = {statusCodeResult.OriginalPath}" +
                                $" and QueryString = {statusCodeResult.OriginalQueryString}");
                        }

                        break;
                }

                return View("NotFound");
            }

            return View();
        }


        [AllowAnonymous]
        [Route("/ErrorHandler")]
        public IActionResult ErrorView()
        {
            // Retrieve the exception Details
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            _logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");

            return View("ErrorView");

            //return Redirect("~/ErrorView");
        }



    }
}
