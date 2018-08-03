﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using _5051;
using _5051.Controllers;
using _5051.Models;
using System.Diagnostics;
using _5051.Backend;

namespace _5051.Tests.Controllers
{
    [TestClass]
    public class PortalControllerTest
    {
        public TestContext TestContext { get; set; }

        #region RosterRegion
        [TestMethod]
        public void Controller_Portal_Roster_Should_Return_NewModel()
        {
            // Arrange
            PortalController controller = new PortalController();

            // Act
            ViewResult result = controller.Roster() as ViewResult;

            var resultStudentViewModel = result.Model as StudentViewModel;

            // Assert
            Assert.IsNotNull(resultStudentViewModel, TestContext.TestName);
        }
        #endregion

        #region LoginStringRegion
        [TestMethod]
        public void Controller_Portal_Login_IDIsNull_Should_Return_RosterPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            string id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Login(id);

            // Assert
            Assert.AreEqual("Roster", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Login_IDValid_ShouldPass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.Create(data).Id;

            // Act
            ViewResult result = controller.Login(id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }
        #endregion

        #region LoginPostRegion
        [TestMethod]
        public void Controller_Portal_Login_Post_ModelIsInvalid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();

            StudentDisplayViewModel data = new StudentDisplayViewModel();

            // Make ModelState Invalid
            controller.ModelState.AddModelError("test", "test");

            // Act
            ViewResult result = controller.Login(data) as ViewResult;

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Login_Post_Invalid_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();

            var data = new StudentDisplayViewModel();
            data = null;
            
            // Act
            var result = (RedirectToRouteResult)controller.Login(data);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);    
        }

        [TestMethod]
        public void Controller_Portal_Login_Post_Invalid_IDIsNull_Should_Return_Model()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel student = new StudentModel();
            var data = new StudentDisplayViewModel(student);
            data.Id = null;

            // Act
            ViewResult result = controller.Login(data) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Assert
            Assert.AreEqual(data.AvatarDescription, resultStudentDisplayViewModel.AvatarDescription, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Login_Post_Invalid_StudentModelIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel student = new StudentModel();
            var data = new StudentDisplayViewModel(student);

            // Act
            var result = (RedirectToRouteResult)controller.Login(data);

            // Reset
            DataSourceBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Login_Post_Valid_Should_Return_IndexPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel student = new StudentModel("Peter", null);
            student.Id = Backend.StudentBackend.Instance.Create(student).Id;
            var data = new StudentDisplayViewModel(student);

            // Act
            var result = (RedirectToRouteResult)controller.Login(data);

            var resultStudent = StudentBackend.Instance.Read(data.Id);

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"], TestContext.TestName);
            Assert.AreEqual(data.Id, resultStudent.Id, TestContext.TestName);
        }
        #endregion

        #region IndexStringRegion
        [TestMethod]
        public void Controller_Portal_Index_IDIsNull_Should_Return_RosterPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            string id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Index(id);

            // Assert
            Assert.AreEqual("Roster", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_IDValid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.Create(data).Id;

            // Act
            ViewResult result = controller.Index(id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_VeryHappy_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.VeryHappy
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Index(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Happy_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Happy
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Index(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Neutral_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Neutral
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Index(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Sad_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Sad
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Index(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Index_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_VerySad_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.VerySad
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Index(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }
        #endregion

        #region AttendanceStringRegion
        [TestMethod]
        public void Controller_Portal_Attendance_IDIsNull_Should_Return_RosterPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            string id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Attendance(id);

            // Assert
            Assert.AreEqual("Roster", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_IDValid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.Create(data).Id;

            // Act
            ViewResult result = controller.Attendance(id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_VeryHappy_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.VeryHappy
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Attendance(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Happy_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Happy
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Attendance(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Neutral_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Neutral
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Attendance(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_Sad_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.Sad
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Attendance(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Attendance_ID_Valid_Attendance_Not_Null_Or_Empty_Emotion_Is_VerySad_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = StudentBackend.Instance.GetDefault();
            data.Attendance = new List<AttendanceModel>();
            var myAttendance = new AttendanceModel()
            {
                StudentId = data.Id,
                In = DateTime.UtcNow,
                Emotion = EmotionStatusEnum.VerySad
            };
            data.Attendance.Add(myAttendance);
            // Act
            ViewResult result = controller.Attendance(data.Id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }
        #endregion

        #region AvatarPostRegion
        [TestMethod]
        public void Controller_Portal_Avatar_Post_ModelIsInvalid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();

            StudentAvatarModel data = new StudentAvatarModel();

            // Make ModelState Invalid
            controller.ModelState.AddModelError("test", "test");

            // Act
            ViewResult result = controller.Avatar(data) as ViewResult;

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Post_Invalid_AvatarIDIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var data = new StudentAvatarModel();
            data.AvatarId = null;

            // Act
            var result = (RedirectToRouteResult)controller.Avatar(data);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Post_Invalid_StudentIDIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var avartar = new AvatarModel();
            avartar.Id = Backend.AvatarBackend.Instance.Create(avartar).Id;
            var data = new StudentAvatarModel();
            data.AvatarId = avartar.Id;
            data.StudentId = null;

            // Act
            var result = (RedirectToRouteResult)controller.Avatar(data);

            // Reset AvatarBackend
            AvatarBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Post_Invalid_StudentModelIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var avartar = new AvatarModel();
            avartar.Id = Backend.AvatarBackend.Instance.Create(avartar).Id;
            StudentModel student = new StudentModel();
            var data = new StudentAvatarModel();
            data.AvatarId = avartar.Id;
            data.StudentId = student.Id;
            

            // Act
            var result = (RedirectToRouteResult)controller.Avatar(data);

            // Reset AvatarBackend
            AvatarBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Post_Valid_Should_Return_IndexPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            AvatarModel avartar = new AvatarModel();
            avartar.Id = Backend.AvatarBackend.Instance.Create(avartar).Id;
            StudentModel student = new StudentModel();
            student.Id = Backend.StudentBackend.Instance.Create(student).Id;
            var data = new StudentAvatarModel();
            data.AvatarId = avartar.Id;
            data.StudentId = student.Id;

            // Act
            var result = (RedirectToRouteResult)controller.Avatar(data);

            var resultAvatar = StudentBackend.Instance.Read(data.AvatarId);

            // Reset AvatarBackend and StudentBackend
            AvatarBackend.Instance.Reset();
            StudentBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"], TestContext.TestName);           
        }
        #endregion

        #region AvatarStringRegion
        [TestMethod]
        public void Controller_Portal_Avatar_IDIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            string id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Avatar(id);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Invalid_AvatarModelIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.Create(data).Id;
            var avatar = new AvatarModel();
            avatar.Id = null;
       
            // Act
            var result = (RedirectToRouteResult)controller.Avatar(id);

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Avatar_Valid_Should_Return_NewModel()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel student = new StudentModel("Peter", null);
            string id = Backend.StudentBackend.Instance.Create(student).Id;

            // Act
            ViewResult result = controller.Avatar(id) as ViewResult;
            var resultSelectedAvatarForStudentViewModel = result.Model as SelectedAvatarForStudentViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultSelectedAvatarForStudentViewModel, TestContext.TestName);
        }

        #endregion

       #region SettingsStringRegion
        [TestMethod]
        public void Controller_Portal_Settings_IDIsNull_Should_Return_RosterPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            string id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Settings(id);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }
        
        [TestMethod]
        public void Controller_Portal_Settings_IDValid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.Create(data).Id;

            // Act
            ViewResult result = controller.Settings(id) as ViewResult;

            var resultStudentDisplayViewModel = result.Model as StudentDisplayViewModel;

            // Reset StudentBackend
            StudentBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(resultStudentDisplayViewModel, TestContext.TestName);
        }
        #endregion

        #region SettingsPostRegion
        [TestMethod]
        public void Controller_Portal_Settings_Post_ModelIsInvalid_Should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();

            StudentDisplayViewModel data = new StudentDisplayViewModel();

            // Make ModelState Invalid
            controller.ModelState.AddModelError("test", "test");

            // Act
            ViewResult result = controller.Settings(data) as ViewResult;

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Settings_Post_Invalid_AvatarIDIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var data = new StudentDisplayViewModel();
            data.AvatarId = null;

            // Act
            var result = (RedirectToRouteResult)controller.Settings(data);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Settings_Post_Invalid_IDIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var avartar = new AvatarModel();
            avartar.Id = Backend.AvatarBackend.Instance.Create(avartar).Id;
            var data = new StudentDisplayViewModel();
            data.AvatarId = avartar.Id;
            data.Id = null;

            // Act
            var result = (RedirectToRouteResult)controller.Settings(data);

            // Reset AvatarBackend
            AvatarBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Settings_Post_Invalid_StudentModelIsNull_Should_Return_ErrorPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            var avartar = new AvatarModel();
            avartar.Id = Backend.AvatarBackend.Instance.Create(avartar).Id;
            StudentModel student = new StudentModel();
            var data = new StudentDisplayViewModel();
            data.AvatarId = avartar.Id;
            data.Id = student.Id;


            // Act
            var result = (RedirectToRouteResult)controller.Settings(data);

            // Reset AvatarBackend
            AvatarBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_Portal_Settings_Post_Valid_Should_Return_IndexPage()
        {
            // Arrange
            PortalController controller = new PortalController();
            AvatarModel avatar = new AvatarModel();
            avatar.Id = Backend.AvatarBackend.Instance.Create(avatar).Id;
            StudentModel student = new StudentModel();
            student.Id = Backend.StudentBackend.Instance.Create(student).Id;
            var data = new StudentDisplayViewModel();
            data.AvatarId = avatar.Id;
            data.Id = student.Id;

            // Act
            var result = (RedirectToRouteResult)controller.Settings(data);

            var resultAvatar = StudentBackend.Instance.Read(data.AvatarId);

            // Reset AvatarBackend and StudentBackend
            AvatarBackend.Instance.Reset();
            StudentBackend.Instance.Reset();

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"], TestContext.TestName);
        }
        #endregion

        #region ReportRegion
        

        [TestMethod]
        public void Controller_Portal_Report_Default_should_Pass()
        {
            // Arrange
            PortalController controller = new PortalController();
            StudentModel data = new StudentModel();
            string id = Backend.StudentBackend.Instance.GetDefault().Id;

            // Act
            var result = controller.Report(id) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(result.RouteValues["Controller"], "Admin",TestContext.TestName);
        }
        #endregion
    }
}
