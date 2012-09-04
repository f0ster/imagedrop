using fileDrop.Models;
using fileDrop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

class HomeController : Controller {

    private fileDropEntities _fileDropEntities;

    public ActionResult Index()
    {
        
        List<DroppedFile> files = _fileDropEntities.DroppedFiles.ToList();
        if(files.Count > 0)
            ViewData["files"] = files;

        return View();

    }

    //using custom filter to check filetype on posted file
    [AcceptVerbs(HttpVerbs.Post), HttpPostedFileType(AllowedFileTypeExtensions = ".jpg|.gif|.png|.jpeg")]
    public ActionResult Upload(HttpPostedFileBase file)
    {
        ContentResult c = new ContentResult();

        try
        {
            if (file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("/"), fileName);
                file.SaveAs(path);
                // now create model
                DroppedFile d = new DroppedFile()
                {
                    FName = fileName
                };
                
                _fileDropEntities.DroppedFiles.AddObject(d);
                _fileDropEntities.SaveChanges();

                c.Content = fileName + ":uploaded successfully";
            }
            else
                c.Content = "Error, file content empty";

            return c;
            
        }
        catch (Exception e)
        {        
            c.Content = e.Message;
            return c;
        }
        
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

}
