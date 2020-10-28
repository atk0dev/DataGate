﻿namespace DataGate.Web.ViewModels.Reports
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DataGate.Common;
    public class TSReportOverviewViewModel
    {
        //[Required(ErrorMessage = ValidationMessages.DateRequired)]
        //[Display(Name = "Valid From")]
        public DateTime StartDate { get; set; }

        //[Required(ErrorMessage = ValidationMessages.DateRequired)]
        //[Display(Name = "Valid Until")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.FundRequired)]
        public string Entity { get; set; }

        [Required(ErrorMessage = ValidationMessages.ContainerRequired)]
        [Display(Name = "Time Serie Type")]
        public string TimeSeriesType { get; set; }

        public TimeSerieReport TimeSerieReport { get; set; }

        public string AreaName { get; set; }
    }
}
