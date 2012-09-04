using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Attribute to accept specific types of uploaded files from a dojo.io.iframe.send context.
/// dojo requires the response to be wrapped in a textarea
/// </summary>
/// 
namespace fileDrop.Helpers
{
    public class HttpPostedFileTypeAttribute : ActionFilterAttribute
    {
        public string AllowedFileTypeExtensions { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            string[] allowedExtensions = null;

            if (AllowedFileTypeExtensions.Contains("|"))
            {
                allowedExtensions = AllowedFileTypeExtensions.Split('|');
            }
            else
            {
                allowedExtensions = new string[1] { AllowedFileTypeExtensions };
            }
            
            foreach (var parameter in filterContext.ActionParameters.Values)
            {
                if (parameter is HttpPostedFileBase)
                {
                    string extension = Path.GetExtension(((HttpPostedFileBase)parameter).FileName).ToLower();              

                    foreach (string allowedExtension in allowedExtensions)
                    {
                        ContentResult result = new ContentResult();
                        if (extension != allowedExtension.ToLower())
                        {
                            result.Content = string.Format("Invalid file type. {0} expected. Given {1}", allowedExtension, extension);
                            filterContext.Result = result;
                            break;
                        }
                        else if (string.IsNullOrEmpty(extension))
                        {
                            result.Content = string.Format("Invalid file type. {0} expected. No extension given.", allowedExtension);
                            filterContext.Result = result;
                            break;
                        }

                        filterContext.Result = result;
                    }
                }
            }
        }
    }
}