using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevExtremeFixed.ViewModels
{
    public class IndexViewModel
    {
        public string Parenttableid { get; set; }
        //public string Contractstatus { get; set; }

        public IndexViewModel(string parenttableid)
        {
            Parenttableid = parenttableid;
            //Contractstatus = contractstatus;
        }
    }
}