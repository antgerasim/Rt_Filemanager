using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevExtremeFixed.Models
{
    public class IndexViewModel
    {
       
        public string Url { get; set; }
        public string Typename { get; set; }

        public IndexViewModel(string url, string typename)
        {
            Url = url;
            Typename = typename;
        }
    }
}