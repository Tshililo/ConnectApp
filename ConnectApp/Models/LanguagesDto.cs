using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConnectApp.Models
{ 
    public class LanguagesDto
    {
        public List<SelectListItem> LanguageDto { get; set; }
        public Guid ObjId { get; set; }
        public Guid? LanguageId { get; set; }

        public string LanguageName { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }
        

    }
}