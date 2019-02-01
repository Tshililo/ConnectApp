using ConnectApp;
using ConnectApp.Models;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConnectApp.Controllers
{
    public class SubjectsController : BaseController
    {
        ConnectDBEntities db = new ConnectDBEntities();
        // GET: Subjects
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportViewPartial(string reportParams)
        {

            // var report = GetReport(reportParams);

            //return PartialView("ReportViewPartial", report);
            return null;
        }

        public ActionResult TurtorialsLanding(string headerObjId)
        {
            var mymodel = db.LanguageBases;
            var ApplicationsInfo = mymodel.Where(s => s.ObjId.ToString() == headerObjId).FirstOrDefault();

            ViewBag.TopicLink = ApplicationsInfo.Link;
            return PartialView("TurtorialsLanding");
        }
        //XtraReport GetReport(string passed_params)
        //{
        //    List<string> p = passed_params.Split(',').ToList();
        //    var rep = new StudentsReportXrMvc();
        //    rep.ObjId.Value = p[0];
        //    rep.Attention.Value = p[1];
        //  //  rep.FromDate.Value = p[2];
        //    //rep.ToDate.Value = p[4];
        //    return rep;
        //}

        private List<Language> GetLanguages()
        {
            return db.Languages.ToList();
        }

        private List<LanguagesDto> GetLanguagesDto()
        {
            var lang = db.Languages.ToList();
            var repo = db.LanguageBases.ToList();

            var model = (from lan in repo
                         from rpLang in  lang.Where(c => c.ObjId == lan.LangauageId).DefaultIfEmpty()
                         select new LanguagesDto
                         {
                             ObjId = lan.ObjId,
                             LanguageId = lan.LangauageId,
                             LanguageName = rpLang.Name,
                             Description = lan.Description,
                             Link = lan.Link
                         });

            return model.ToList();
        }
        public ActionResult EditHeaderFormPartial(Guid ObjId, LanguagesDto model)
		{
            ViewData["Languages"] = GetLanguages();

            var mymodel = GetLanguagesDto();

            var ApplicationsInfo = mymodel.Where(s => s.ObjId == ObjId).FirstOrDefault();

			if (ApplicationsInfo == null)
			{
				model.ObjId = ObjId;
				return PartialView("CreateSubjectsEditPartial", model);
			}

			if (ApplicationsInfo != null)
			{
                return PartialView("CreateSubjectsEditPartial", ApplicationsInfo);
			}

			return null;

		}      
    
        public ActionResult SubjectsAddEdit(LanguagesDto item, string headerObjId)
        {
  
            var modelRepo = db.LanguageBases;
            var exists = modelRepo.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
            LanguageBase Tosave = new LanguageBase();
            Tosave.LangauageId = item.LanguageId;
            if (exists == null)
            {
                CopyProperties(item, Tosave);
             
                modelRepo.Add(Tosave);
                db.SaveChanges();
            }
            if (exists != null)
            {
                CopyProperties(item, exists);
                this.UpdateModel(exists);
                // modelRepo.Attach(exists);
                db.SaveChanges();
            }

            var BurialRecords = GetLanguagesDto();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("_SubjectsGridViewPartial", BurialRecords);

        }
       [HttpPost, ValidateInput(false)]
        public ActionResult SubjectsDelete(Guid ObjId)
        {
            var model = db.LanguageBases;
            if (ObjId != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ObjId == ObjId);
                    if (item != null)
                    {
                        model.Remove(item);

                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var BurialRecords = GetLanguagesDto();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("_SubjectsGridViewPartial", BurialRecords);
        }
        public ActionResult SubjectsGridViewPartial()
        {
          //  var model = db.Lunguages;
            var mymodel = GetLanguagesDto();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("_SubjectsGridViewPartial", GetLanguagesDto().OrderBy(r => r.Description));
        }
    }
}