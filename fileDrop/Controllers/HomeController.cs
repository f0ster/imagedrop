using fileDrop.Models;
using fileDrop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace fileDrop.Controllers
{

    public class HomeController : Controller
    {

        private fileDropEntities _fileDropEntities;
        private string _imageDirectoryPrefix;

        public HomeController()
        {
            _fileDropEntities = new fileDropEntities();
            _imageDirectoryPrefix = System.Configuration.ConfigurationManager.AppSettings["imageDirectoryPrefix"];
        }

        public ActionResult Index()
        {

            List<DroppedFile> files = _fileDropEntities.DroppedFiles.ToList();
            if (files.Count > 0)
                ViewData["files"] = files;

            return View();

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload()
        {
            ContentResult c = new ContentResult();
            if (Request.Form["value"] != null && Request.Form["name"] != null)
            {
                try
                {
                    //TODO: change file name to (G|U)UID perhaps?
                    string b64_data = (Request.Form["value"].Split(',')[1]).Replace(' ', '+');
                    string baseDir = Path.Combine(Server.MapPath("/"), this._imageDirectoryPrefix);
                    string fileName = Path.GetFileName(Request.Form["name"]);
                    string path = baseDir + fileName;
                    //check if dir exists, if not: make it
                    if (!System.IO.File.Exists(baseDir))
                        System.IO.Directory.CreateDirectory(baseDir);
                    System.IO.File.WriteAllBytes(path, Convert.FromBase64String(b64_data));
                    // now create model
                    DroppedFile d = new DroppedFile()
                    {
                        FName = fileName,
                        DateCreated = DateTime.Now
                    };

                    _fileDropEntities.DroppedFiles.AddObject(d);
                    _fileDropEntities.SaveChanges();

                    c.Content = fileName + ":uploaded successfully:" + d.id;
                }
                catch (Exception e)
                {
                    c.Content = "-1";
                }
            }
            else
                c.Content = "-1";

            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateCaption(int id, string caption)
        {

            DroppedFile d = _fileDropEntities.DroppedFiles.Where(file => file.id == id).FirstOrDefault();

            if (d != null)
            {
                d.Caption = caption;
                _fileDropEntities.ApplyCurrentValues(d.EntityKey.EntitySetName, d);
                _fileDropEntities.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            else
                return new HttpStatusCodeResult(500);

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            DroppedFile d = _fileDropEntities.DroppedFiles.Where(file => file.id == id).FirstOrDefault();

            if (d != null)
            {
                _fileDropEntities.DeleteObject(d);
                _fileDropEntities.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            else
                return new HttpStatusCodeResult(500);

        }

    }
}