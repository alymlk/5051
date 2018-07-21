﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5051.Models
{
    /// <summary>
    /// Monthly report view model, for generating monthly report view
    /// </summary>
    public class MonthlyReportViewModel
    {
        /// <summary>
        /// Contains a select list for month selection dropdown
        /// </summary>
        public List<SelectListItem> Months { get; set; }

        /// <summary>
        /// The selected month's id
        /// </summary>
        public int SelectedMonthId { get; set; }
    }
}