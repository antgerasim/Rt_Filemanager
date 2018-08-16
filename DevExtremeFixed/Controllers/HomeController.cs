using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExtremeFixed.Models;

namespace DevExtremeFixed.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id, string typename, string fmtype, string sfmtype)
        {
            //http://localhost:2998/?id=6659B6E9-891C-E811-80CA-D3E9C8E3E880&typename=new_t_project
            //http://localhost:2998/?fmtype=small&id=6659B6E9-891C-E811-80CA-D3E9C8E3E880&typename=new_t_project //sfmType1
            //http://localhost:2998/?fmtype=small&sfmtype=sfmType1&id=6659B6E9-891C-E811-80CA-D3E9C8E3E880&typename=new_t_project
            var lastFmType = fmtype ?? "big";
            Session["lastfmtype"] = lastFmType; // lastFileManagerType
            Session["lastsfmtype"] = sfmtype ?? string.Empty; // lastSmallFileManagerType
            try
            {
                TestFilePermissions();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(new IndexViewModel(id, typename));
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial(string id, string typename)
        {
            var typeNameKey = typename; //dont touch typename in args as it is URL param in dynamics project
                                        //if directory not exist, create new    
            var fmtype = Session["lastfmtype"] as string;
            var sfmType = Session["lastsfmtype"] as string;

            var isAjaxRequestTest = Request.IsAjaxRequest();

            if (Request.IsAjaxRequest() || Request.RawUrl.Contains("_Upload")) //Home/FileManagerPartial?DXProgressHandlerKey=e7c314181cd51b09fe25a8a4be03700b&DXHelperUploadingCallback=FileManager_Splitter_Upload
            {
                string rootFolder = null;
                var homeControllerFileManagerSettings = new HomeControllerFileManagerSettings(rootFolder);
                return PartialView("_FileManagerPartial", HomeControllerFileManagerSettings.Model);
            }

            if (!String.IsNullOrEmpty(id)) //typename always there
            {
                id = id.Replace("{", string.Empty).Replace("}", string.Empty);

                var folderPath = "";
                //if (!string.IsNullOrEmpty(Request.Params["fmtype"]) && Request.Params["fmtype"].ToString() == "small")
                if (fmtype == "small")
                {
                    folderPath = String.Format(@"C:\Documents\SmallProjects\{0}\{1}\Home", typeNameKey, id);

                    if (!string.IsNullOrEmpty(sfmType))
                    {
                        //var sfmType = String.Empty;
                        /* * 1. Архитектура решения = sfmType1
                         * * 2. Схема реализации = sfmType2
                         * * 3. КП подрядчиков = sfmType3
                         * * 4. Паспорт сделки = sfmType4
                         * */
                        //folderPath = String.Format(@"C:\Documents\SmallProjects\{0}\{1}\{2}\Home", sfmType, typeNameKey, id);
                        folderPath = String.Format(@"C:\Documents\SmallProjects\{0}\{1}\{2}", typeNameKey, id, sfmType);

                    }
                }
                else if (fmtype == "big")
                {
                    folderPath = String.Format(@"C:\Documents\Projects\{0}\{1}\Home", typeNameKey, id);
                }

                var targetDirectoryExists = Directory.Exists(folderPath);

                if (!targetDirectoryExists)
                {
                    Directory.CreateDirectory(folderPath);
                }
                new HomeControllerFileManagerSettings(folderPath);
                //throw new Exception(folderPath.ToString());
                return PartialView("_FileManagerPartial", HomeControllerFileManagerSettings.Model);
            }
            else
            {
                var baseDirectoryExists = Directory.Exists(@"C:\Documents\Projects\");
                if (!baseDirectoryExists)
                {
                    var excString = baseDirectoryExists ? "Documents-Projects directory exists" : "Documents-Projects directory NOT exists";
                    throw new Exception(String.Format("Guid id and typename are empty {0}", excString)); //todo convert to userfriendly view
                }

                // get string based on typename in webconfig and put into model

                string configTypeNameText = System.Configuration.ConfigurationManager.AppSettings[String.IsNullOrEmpty(typeNameKey) ? "new_t_project" : typeNameKey];

                return View("IdNotFound", new IdNotFoundViewModel(configTypeNameText));
            }
        }

        private void TestFilePermissions()
        {
            string dirVirtualPath = "~/TestDir";
            string dirPhysicalPath = Server.MapPath(dirVirtualPath);
            if (!Directory.Exists(dirPhysicalPath))
            {
                Directory.CreateDirectory(dirPhysicalPath);
            }

            string fileName = "TestFile.txt";
            string fileFullPath = Path.Combine(dirPhysicalPath, fileName);

            System.IO.File.WriteAllText(fileFullPath, "File Content Here...");
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(HomeControllerFileManagerSettings.DownloadSettings,
                HomeControllerFileManagerSettings.Model);
        }

        private static void testFx()
        {

        }
    }

    public class HomeControllerFileManagerSettings
    {
        public static string RootFolder = "";


        public HomeControllerFileManagerSettings(string rootFolder)
        {
            if (rootFolder != null)
            {
                RootFolder = rootFolder;
            }
            // else //ajax rootfolder is null
        }

        public static string Model
        {
            get { return RootFolder; }
        }

        public static DevExpress.Web.Mvc.FileManagerSettings DownloadSettings
        {
            get
            {
                var settings = new DevExpress.Web.Mvc.FileManagerSettings { Name = "FileManager" };
                settings.SettingsEditing.AllowDownload = true;
                return settings;
            }
        }
    }
}