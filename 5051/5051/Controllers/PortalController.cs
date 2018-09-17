﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using _5051.Models;
using _5051.Backend;

namespace _5051.Controllers
{
    public class PortalController : BaseController
    {
        /// <summary>
        /// The list of all the active students in the class, so they can Roster
        /// </summary>
        /// <returns></returns>
        // GET: Portal
        public ActionResult Roster()
        {
            var myDataList = DataSourceBackend.Instance.StudentBackend.Index();
            var StudentViewModel = new StudentViewModel(myDataList);
            return View(StudentViewModel);
        }

        /// <summary>
        /// The Roster in page for the Portal, shows all the Students
        /// </summary>
        /// <returns></returns>
        // GET: Portal
        public ActionResult Login(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Roster", "Portal");
            }

            var myReturn = new StudentDisplayViewModel(myStudent);

            return View(myReturn);
        }

        /// <summary>
        /// Login for the student, take the ID, the rest of the fields are required but not used
        /// </summary>
        /// <param name="data"></param>
        /// <returns>if all is OK, then redirect to the student protal page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include=
                                        "Id,"+
                                        "Name,"+
                                        "Description,"+
                                        "Uri,"+
                                        "AvatarLevel,"+
                                        "Tokens,"+
                                        "Status,"+
                                        "Password,"+
                                        "ExperiencePoints,"+
                                        "Password,"+
                                        "")] StudentDisplayViewModel data)
        {
            // Any password is accepted for now. does not really login...

            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return View(data);
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", "Home");
            }

            if (string.IsNullOrEmpty(data.Id))
            {
                // Send back for Edit
                return RedirectToAction("Error", "Home");
            }

            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(data.Id);
            if (myStudent == null)
            {
                // Send to Error Page
                //return RedirectToAction("Error", "Home");
                return RedirectToAction("Roster", "Portal");
            }

            // When not in testing mode try the password
            if (!DataSourceBackend.GetTestingMode())
            {
                //if (!IdentityDataSourceTable.Instance.LogUserIn(myStudent.Name, data.Password, IdentityDataSourceTable.IdentityRole.Student))
                if(!IdentityBackend.Instance.LogUserIn(myStudent.Name, data.Password, IdentityDataSourceTable.IdentityRole.Student))
                {
                    ModelState.AddModelError("", "Invalid password");
                    return View(data);
                }
            }

            // all is OK, so redirect to the student index page and pass in the student ID for now.
            return RedirectToAction("Index", "Portal", new { id = data.Id });
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns>Student Record as a Student View Model</returns>
        // GET: Portal
        public ActionResult Index(string id = null)
        {
            // Temp hold the Student Id for the Nav, until the Nav can call for Identity.
            ViewBag.StudentId = id;

            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

            if (myStudent == null)
            {
                return RedirectToAction("Roster", "Portal");
            }

            var myReturn = new StudentDisplayViewModel(myStudent);

            //Set the last log in time and emotion status img uri
            if (myReturn.Attendance.Any())
            {
                myReturn.LastLogIn = UTCConversionsBackend.UtcToKioskTime(myReturn.Attendance.OrderByDescending(m => m.In).FirstOrDefault().In);
            }

            var myWeeklyReport = new WeeklyReportViewModel()
            {
                StudentId = id,
                SelectedWeekId = 1
            };

            var myMonthlyReport = new MonthlyReportViewModel()
            {
                StudentId = id,
                SelectedMonthId = 1
            };

            myReturn.WeeklyAttendanceScore = ReportBackend.Instance.GenerateWeeklyReport(myWeeklyReport).Stats.PercAttendedHours;
            myReturn.MonthlyAttendanceScore = ReportBackend.Instance.GenerateMonthlyReport(myMonthlyReport).Stats.PercAttendedHours;

            return View(myReturn);
        }

        /// <summary>
        /// Attendance Page
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns>Student Record as a Student View Model</returns>
        // GET: Portal
        public ActionResult Attendance(string id = null)
        {
            // Temp hold the Student Id for the Nav, until the Nav can call for Identity.
            ViewBag.StudentId = id;

            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Roster", "Portal");
            }

            var myReturn = new StudentDisplayViewModel(myStudent);

            var attendanceListOrdered = myReturn.Attendance.OrderByDescending(m => m.In);

            //Set the last log in time and emotion status img uri
            if (attendanceListOrdered.Any())
            {
                myReturn.LastLogIn = UTCConversionsBackend.UtcToKioskTime(attendanceListOrdered.FirstOrDefault().In);
            }

            //Deep copy Attendance list and convert time zone
            var myAttendanceModels = new List<AttendanceModel>();

            foreach (var item in attendanceListOrdered)
            {
                var myAttendance = new AttendanceModel()
                {
                    //deep copy the AttendanceModel and convert time zone
                    In = UTCConversionsBackend.UtcToKioskTime(item.In),
                    Out = UTCConversionsBackend.UtcToKioskTime(item.Out),
                    Id = item.Id,
                    StudentId = myStudent.Id,
                    Emotion = item.Emotion,
                    EmotionUri = Emotion.GetEmotionURI(item.Emotion)
                };

                myAttendance.Id = item.Id;

                myAttendanceModels.Add(myAttendance);
            }

            myReturn.Attendance = myAttendanceModels;

            return View(myReturn);
        }

        /// <summary>
        /// AttendanceUpdate Page
        /// </summary>
        /// <param name="id">Attendance Id</param>
        /// <returns>Attendance Record as a Attendance Model</returns>
        // GET: Portal

        public ActionResult AttendanceUpdate(string id, string item)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error", "Home");
            }

            if (string.IsNullOrEmpty(item))
            {
                return RedirectToAction("Error", "Home");
            }

            //get the attendance with given id
            var myAttendance = DataSourceBackend.Instance.StudentBackend.ReadAttendance(id, item);
            if (myAttendance == null)
            {
                return RedirectToAction("Error", "Home");
            }

            //Create a new attendance to hold converted times
            var myReturn = new AttendanceModel
            {
                StudentId = myAttendance.StudentId,
                Id = myAttendance.Id,
                In = UTCConversionsBackend.UtcToKioskTime(myAttendance.In),
                Out = UTCConversionsBackend.UtcToKioskTime(myAttendance.Out),
                Emotion = myAttendance.Emotion,
                EmotionUri = Emotion.GetEmotionURI(myAttendance.Emotion),

                IsNew = myAttendance.IsNew
            };

            return View(myReturn);
        }

        /// <summary>
        /// AttendanceUpdate page
        /// </summary>
        /// <returns></returns>
        // POST: Portal

        [HttpPost]
        public ActionResult AttendanceUpdate([Bind(Include=
            "Id,"+
            "StudentId,"+
            "Emotion,"+
            "EmotionUri,"+
            "")] AttendanceModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit
                return View(data);
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", "Home");
            }

            // The emotionURI is passed in as the AttendanceID because of conflicts with the model, it is then converted
            if (string.IsNullOrEmpty(data.EmotionUri))
            {
                // Send back for edit
                return View(data);
            }
            data.Id = data.EmotionUri;  //copy the ID back to Data.Id

            if (string.IsNullOrEmpty(data.StudentId))
            {
                return View(data);
            }

            //get the attendance with given id
            var myAttendance = DataSourceBackend.Instance.StudentBackend.ReadAttendance(data.StudentId,data.Id);

            if (myAttendance == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", "Home");
            }

            //update the emotion
            myAttendance.Emotion = data.Emotion;
            myAttendance.EmotionUri = Emotion.GetEmotionURI(myAttendance.Emotion);
            DataSourceBackend.Instance.StudentBackend.UpdateAttendance(myAttendance);

            return RedirectToAction("Attendance", new { id = myAttendance.StudentId, item = myAttendance.Id });
        }

        /// <summary>
        ///  My Settings
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns>Student Record as a Student View Model</returns>
        // GET: Portal
        public ActionResult Settings(string id = null)
        {
            // Temp hold the Student Id for the Nav, until the Nav can call for Identity.
            ViewBag.StudentId = id;

            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var myReturn = new StudentDisplayViewModel(myStudent);

            return View(myReturn);
        }


        /// <summary>
        /// Student's Avatar page
        /// </summary>
        /// <returns></returns>
        // Post: Portal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings([Bind(Include=
                                        "Id,"+
                                        "Name,"+
                                        "Description,"+
                                        "Uri,"+
                                        "AvatarLevel,"+
                                        "Tokens,"+
                                        "Status,"+
                                        "AvatarUri,"+
                                        "ExperiencePoints,"+
                                        "Password,"+

                                        "")] StudentDisplayViewModel data)
        {
            // If data passed up is not valid, go back to the Index page so the user can try again
            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return View(data);
            }

            // If the Student Id is blank, error out
            if (string.IsNullOrEmpty(data.Id))
            {
                return RedirectToAction("Error", "Home");
            }

            // Lookup the student id, will just replace the Avatar Id on it if it is valid
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(data.Id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            ////if the student name already exists in db, if does, return for edits
            //var idFindResult = IdentityDataSourceTable.Instance.FindUserByUserName(data.Name);
            //if(idFindResult != null)
            //{
            //    ModelState.AddModelError("", "User name already taken");
            //    return View(data);
            //}

            // Set the Avatar ID on the Student and update in data store
            //myStudent.Name = data.Name;
            //DataSourceBackend.Instance.StudentBackend.Update(myStudent);
            //IdentityDataSourceTable.Instance.ChangeUserName(myStudent.Id, data.Name);
            IdentityBackend.Instance.ChangeUserName(myStudent.Id, data.Name);

            // Editing is done, so go back to the Student Portal and pass the Student Id
            return RedirectToAction("Index", "Portal", new { Id = myStudent.Id });
        }

        // GET: WeeklyReport
        /// <summary>
        /// Returns an individual weekly report for a student
        /// </summary>
        /// <param name="id">Student ID to generate Report for</param>
        /// <returns>Report data</returns>
        public ActionResult WeeklyReport(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var myReport = new WeeklyReportViewModel()
            {
                StudentId = id,
                SelectedWeekId = 1
            };

            var myReturn = ReportBackend.Instance.GenerateWeeklyReport(myReport);

            return View(myReturn);
        }

        /// <summary>
        /// Generate report using the WeeklyReportViewModel passed in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeeklyReport([Bind(Include=
            "StudentId,"+
            "SelectedWeekId"+
            "")] WeeklyReportViewModel data)
        {

            if (data == null)
            {
                // Send to error page
                return RedirectToAction("Error", "Home");
            }

            //generate the report
            var myReturn = ReportBackend.Instance.GenerateWeeklyReport(data);

            return View(myReturn);
        }

        // GET: Report
        /// <summary>
        /// Returns an individual monthly report for a student
        /// </summary>
        /// <param name="id">Student ID to generate Report for</param>
        /// <returns>Report data</returns>
        public ActionResult MonthlyReport(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }


            var myReport = new MonthlyReportViewModel()
            {
                StudentId = id,
                SelectedMonthId = 1
            };

            var myReturn = ReportBackend.Instance.GenerateMonthlyReport(myReport);

            return View(myReturn);
        }
        /// <summary>
        /// Generate report using the StudentReportViewModel passed in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MonthlyReport([Bind(Include=
            "StudentId,"+
            "SelectedMonthId"+
            "")] MonthlyReportViewModel data)
        {

            if (data == null)
            {
                // Send to error page
                return RedirectToAction("Error", "Home");
            }

            //generate the report
            var myReturn = ReportBackend.Instance.GenerateMonthlyReport(data);

            return View(myReturn);
        }

        // GET: SemesterReport
        /// <summary>
        /// Returns an individual semester report for a student
        /// </summary>
        /// <param name="id">Student ID to generate Report for</param>
        /// <returns>Report data</returns>
        public ActionResult SemesterReport(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }


            var myReport = new SemesterReportViewModel()
            {
                StudentId = id,
                SelectedSemesterId = 1
            };

            var myReturn = ReportBackend.Instance.GenerateSemesterReport(myReport);

            return View(myReturn);
        }

        /// <summary>
        /// Generate report using the SemesterReportViewModel passed in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SemesterReport([Bind(Include=
            "StudentId,"+
            "SelectedSemesterId"+
            "")] SemesterReportViewModel data)
        {
            if (data == null)
            {
                // Send to error page
                return RedirectToAction("Error", "Home");
            }

            //generate the report
            var myReturn = ReportBackend.Instance.GenerateSemesterReport(data);

            return View(myReturn);
        }

        // GET: QuarterReport
        /// <summary>
        /// Returns an individual quarter report for a student
        /// </summary>
        /// <param name="id">Student ID to generate Report for</param>
        /// <returns>Report data</returns>
        public ActionResult QuarterReport(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }


            var myReport = new QuarterReportViewModel()
            {
                StudentId = id,
                SelectedQuarterId = 1
            };

            var myReturn = ReportBackend.Instance.GenerateQuarterReport(myReport);

            return View(myReturn);
        }

        /// <summary>
        /// Generate report using the QuarterReportViewModel passed in
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuarterReport([Bind(Include=
            "StudentId,"+
            "SelectedQuarterId"+
            "")] QuarterReportViewModel data)
        {
            if (data == null)
            {
                // Send to error page
                return RedirectToAction("Error", "Home");
            }

            //generate the report
            var myReturn = ReportBackend.Instance.GenerateQuarterReport(data);

            return View(myReturn);
        }

        // GET: Report
        /// <summary>
        /// Returns an individual overall report for a student
        /// </summary>
        /// <param name="id">Student ID to generate Report for</param>
        /// <returns>Report data</returns>
        public ActionResult OverallReport(string id = null)
        {
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }


            var myReport = new SchoolYearReportViewModel()
            {
                StudentId = id,
            };

            var myReturn = ReportBackend.Instance.GenerateOverallReport(myReport);

            return View(myReturn);
        }

        ///// <summary>
        ///// My Attendance Reports
        ///// </summary>
        ///// <param name="id">Student Id</param>
        ///// <returns>Student Record as a Student View Model</returns>
        //// GET: Portal
        //public ActionResult Report(string id = null)
        //{
        //    var myStudent = DataSourceBackend.Instance.StudentBackend.Read(id);

        //    if (myStudent == null)
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }


        //    //var myReport = new StudentReportViewModel
        //    //{
        //    //    StudentId = id,
        //    //    Year = DateTimeHelper.Instance.GetDateTimeNowUTC().Year,
        //    //    Month = DateTimeHelper.Instance.GetDateTimeNowUTC().Month
        //    //};

        //    //var myReturn = ReportBackend.Instance.GenerateMonthlyReport(myReport);

        //    return RedirectToAction("Index", "Portal", new { id = myStudent.Id });

        //    // return RedirectToAction("MonthlyReport", "Admin", new { id });
        //}
    }
}