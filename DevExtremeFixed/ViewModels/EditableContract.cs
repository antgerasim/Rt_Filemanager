﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevExtremeFixed.ViewModels
{
    public class EditableContract
    {
        public Guid ContrGuid { get; set; }
        public string Product { get; set; }
        public string ProductGroupProduct { get; set; }
        [Display(Name = "Что то по русский")]
        public decimal? Service1Quarter { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal? Service2Quarter { get; set; }
        public decimal? Service3Quarter { get; set; }
        public decimal? Service4Quarter { get; set; }
        public decimal? Consult1Quarter { get; set; }
        public decimal? Consult2Quarter { get; set; }
        public decimal? Consult3Quarter { get; set; }
        public decimal? Consult4Quarter { get; set; }
        public decimal? NewServiceYear { get; set; }
        public decimal? NewConsultYear { get; set; }
        public decimal? NewProductTotalService { get; set; }
        public decimal? NewProductTotalConsult { get; set; }

        public decimal? NewYearTotal { get; set; }
        public decimal? FirstQuartalTotal { get; set; }
        public decimal? SecondQuartalTotal { get; set; }
        public decimal? ThirdQuartalTotal { get; set; }
        public decimal? FourthQuartalTotal { get; set; }
        public decimal? StatusCode { get; set; }
    }
}