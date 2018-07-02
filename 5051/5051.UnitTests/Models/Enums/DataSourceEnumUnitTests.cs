﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using _5051.Models;

namespace _5051.UnitTests.Models
{
    [TestClass]
    public class DataSourceEnumUnitTests
    {
        public TestContext TestContext { get; set; }

        #region Instantiate
        [TestMethod]
        public void Models_DataSourceEnumUnitTests_Values_Should_Pass()
        {
            // Assert

            // Make sure there are no additional values
            var enumCount = DataSourceEnum.GetNames(typeof(DataSourceEnum)).Length;
            Assert.AreEqual(3, enumCount, TestContext.TestName);

            // Check each value against their expected value.
            Assert.AreEqual(2, (int)DataSourceEnum.SQL, TestContext.TestName);
            Assert.AreEqual(1, (int)DataSourceEnum.Mock, TestContext.TestName);
            Assert.AreEqual(0, (int)DataSourceEnum.Unknown, TestContext.TestName);
        }
        #endregion Instantiate
    }
}