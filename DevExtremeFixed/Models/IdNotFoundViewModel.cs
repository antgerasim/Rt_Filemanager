using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevExtremeFixed.Models
{
    public class IdNotFoundViewModel
    {
        public string TypeNameText { get; private set; }

        public IdNotFoundViewModel(string typeNameText)
        {
            TypeNameText = typeNameText;
        }
    }
}