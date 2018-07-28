﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _5051.Models;

namespace _5051.UnitTests.Models
{
    [TestClass]
    public class FactoryInventoryModelUnitTests
    {
        public TestContext TestContext { get; set; }

        #region Instantiate
        [TestMethod]
        public void Models_FactoryInventoryModel_Default_Instantiate_Should_Pass()
        {

            // Act
            var result = new FactoryInventoryModel();

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);
        }

        [TestMethod]
        public void Models_FactoryInventoryModel_Instantiate_Valid_Should_Pass()
        {
            // Arrange
            var uri = "uri";
            var name = "name";
            var description = "description";
            FactoryInventoryCategoryEnum category = FactoryInventoryCategoryEnum.Music;
            int tokens = 10;
            int quantities = 10; 
            var expect = uri;

            // Act
            var result = new FactoryInventoryModel(uri, name, description,category,tokens,quantities);

            // Assert
            Assert.AreEqual(expect,result.Uri, TestContext.TestName);
        }

        [TestMethod]
        public void Models_FactoryInventoryModel_Instantiate_Valid_Get_Catagory_Should_Pass()
        {
            // Arrange
            var result = new FactoryInventoryModel();

            // Act
            var expect = result.Category;

            // Assert
            Assert.AreEqual(expect, result.Category, TestContext.TestName);
        }
        #endregion Instantiate

        #region Update
        [TestMethod]
        public void Models_FactoryInventoryModel_Update_With_Valid_Data_Should_Pass()
        {
            // Arrange
            var expect = "test";

            var data = new FactoryInventoryModel();
            data.Uri = "bogus";

            var test = new FactoryInventoryModel();
            test.Uri = "test";

            // Act
            data.Update(test);
            var result = data.Uri;

            // Assert
            Assert.AreEqual(expect, result, TestContext.TestName);
        }

        [TestMethod]
        public void Models_FactoryInventoryModel_Update_With_Invalid_Data_Null_Should_Fail()
        {
            // Arrange

            var expect = "test";

            var data = new FactoryInventoryModel();
            data.Id = "test";

            // Act
            data.Update(null);
            var result = data.Id;

            // Assert
            Assert.AreEqual(expect, result, TestContext.TestName);
        }

        #endregion Update
    }
}
