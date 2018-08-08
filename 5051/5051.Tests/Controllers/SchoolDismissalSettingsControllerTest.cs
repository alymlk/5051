﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using _5051;
using _5051.Controllers;
using _5051.Backend;
using _5051.Models;

namespace _5051.Tests.Controllers
{
    [TestClass]
    public class SchoolDismissalSettingsControllerTest
    {
        public TestContext TestContext { get; set; }

        #region Instantiate
        [TestMethod]
        public void Controller_SchoolDismissalSettings_Instantiate_Default_Should_Pass()
        {
            // Arrange
            var controller = new AdminController();

            // Act
            var result = controller.GetType();

            // Assert
            Assert.AreEqual(result, new AdminController().GetType(), TestContext.TestName);
        }

        #endregion Instantiate

        #region ReadRegion
        [TestMethod]
        public void Controller_SchoolDismissalSettings_Read_Null_Id_Should_Return_Default_Model()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            string id = null;

            // Act
            var result = (ViewResult)controller.Read(id);

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Read_No_Id_Should_Return_Default_Model()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();

            // Act
            var result = (ViewResult)controller.Read();

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Read_Invalid_Id_Should_Return_Error_Page()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            string id = "bogus";

            // Act
            var result = (RedirectToRouteResult)controller.Read(id);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
            Assert.AreEqual("Home", result.RouteValues["controller"], TestContext.TestName);
        }
        #endregion ReadRegion

        #region GetUpdateRegion
        [TestMethod]
        public void Controller_SchoolDismissalSettings_Get_Update_Valid_Id_Should_Return_Model()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            string id = DataSourceBackend.Instance.SchoolDismissalSettingsBackend.GetDefault().Id;

            // Act
            var result = (ViewResult)controller.Update(id);

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Get_Update_Invalid_Id_Should_Return_Error_Page()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            string id = "bogus";

            // Act
            var result = (RedirectToRouteResult)controller.Update(id);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
            Assert.AreEqual("Home", result.RouteValues["controller"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Get_Update_Invalid_Id_Null_Should_Return_Error_Page()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            string id = null;

            // Act
            var result = (ViewResult)controller.Update(id);
            var resultModel = (SchoolDismissalSettingsModel)result.Model;

            var expect = SchoolDismissalSettingsBackend.Instance.GetDefault();

            // Assert
            Assert.AreEqual(expect.Id, resultModel.Id, TestContext.TestName);
        }
        #endregion GetUpdateRegion

        #region PostUpdateRegion
        [TestMethod]
        public void Controller_SchoolDismissalSettings_Post_Update_Invalid_Model_Should_Send_Back_For_Edit()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            SchoolDismissalSettingsModel data = new SchoolDismissalSettingsModel();

            // Make ModelState Invalid
            controller.ModelState.AddModelError("test", "test");

            // Act
            ViewResult result = (ViewResult)controller.Update(data);

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Post_Update_Null_Data_Should_Return_Error()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();

            // Act
            var result = (RedirectToRouteResult)controller.Update((SchoolDismissalSettingsModel)null);

            // Assert
            Assert.AreEqual("Error", result.RouteValues["action"], TestContext.TestName);
            Assert.AreEqual("Home", result.RouteValues["route"], TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Post_Update_Null_Id_Should_Send_Back_For_Edit()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            SchoolDismissalSettingsModel dataNull = new SchoolDismissalSettingsModel();

            // Make data.Id = null
            dataNull.Id = null;

            // Act
            var resultNull = (ViewResult)controller.Update(dataNull);

            // Assert
            Assert.IsNotNull(resultNull, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Post_Update_Empty_Id_Should_Send_Back_For_Edit()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();
            SchoolDismissalSettingsModel dataEmpty = new SchoolDismissalSettingsModel();

            // Make data.Id empty
            dataEmpty.Id = "";

            // Act
            var resultEmpty = (ViewResult)controller.Update(dataEmpty);

            // Assert
            Assert.IsNotNull(resultEmpty, TestContext.TestName);
        }

        [TestMethod]
        public void Controller_SchoolDismissalSettings_Post_Update_Default_Should_Return_Read_SchoolDismissalSettings_Page()
        {
            // Arrange
            var controller = new SchoolDismissalSettingsController();

            // Get default SchoolDismissalSettingsModel
            SchoolDismissalSettingsModel schoolDismissalSettingsModel = DataSourceBackend.Instance.SchoolDismissalSettingsBackend.GetDefault();

            // Act
            var result = (RedirectToRouteResult)controller.Update(schoolDismissalSettingsModel);

            // Assert
            Assert.AreEqual("Read", result.RouteValues["action"], TestContext.TestName);
            Assert.AreEqual("SchoolDismissalSettings", result.RouteValues["controller"], TestContext.TestName);
        }
        #endregion PostUpdateRegion
    }
}
