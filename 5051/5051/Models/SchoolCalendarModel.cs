﻿using System;
using _5051.Models.Enums;

namespace _5051.Models
{
    /// <summary>
    /// Track the School Calendar
    /// </summary>
    public class SchoolCalendarModel
    {
        /// <summary>
        /// The Id for the Record
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Date including year
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The number of hours and minutes in the school day
        /// </summary>
        public TimeSpan TimeMax { get; set; }

        /// <summary>
        /// The start of school time
        /// </summary>
        public TimeSpan TimeStart { get; set; }

        /// <summary>
        /// The End of school time  (full day, or part time)
        /// </summary>
        public TimeSpan TimeEnd { get; set; }

        /// <summary>
        /// School day Starts normal, early, or late (snow day)
        /// </summary>
        public SchoolCalendarDismissalEnum DayStart { get; set; }

        /// <summary>
        /// School day Ends normal, early (wed early dismissal), or late
        /// </summary>
        public SchoolCalendarDismissalEnum DayEnd { get; set; }

        /// <summary>
        /// Create the default values
        /// </summary>
        public void Initialize()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// New SchoolCalendar
        /// </summary>
        public SchoolCalendarModel()
        {
            Initialize();
        }

        /// <summary>
        /// Make an SchoolCalendar from values passed in
        /// </summary>
        /// <param name="data">a valid model</param>
        public SchoolCalendarModel(SchoolCalendarModel data)
        {
            Initialize();

            if (data == null)
            {
                // If the data is null, just return a regular initialized one
                return;
            }

            Date = data.Date;
            TimeMax = data.TimeMax;
            TimeStart = data.TimeStart;
            TimeEnd = data.TimeEnd;
            DayStart = data.DayStart;
            DayEnd = data.DayEnd;
        }

        /// <summary>
        /// Used to Update Before doing a data save
        /// Updates everything except for the ID
        /// </summary>
        /// <param name="data">Data to update</param>
        public void Update (SchoolCalendarModel data)
        {
            if (data == null)
            {
                return;
            }

            Date = data.Date;
            TimeMax = data.TimeMax;
            TimeStart = data.TimeStart;
            TimeEnd = data.TimeEnd;
            DayStart = data.DayStart;
            DayEnd = data.DayEnd;
        }
    }
}