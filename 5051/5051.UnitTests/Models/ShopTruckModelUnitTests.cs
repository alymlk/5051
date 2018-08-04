﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _5051.Models;
using _5051.Backend;

namespace _5051.UnitTests.Models
{
    [TestClass]
    public class ShopTruckUnitTests
    {
        public TestContext TestContext { get; set; }

        #region Instantiate
        [TestMethod]
        public void Models_ShopTruck_Default_Instantiate_Get_Set_Should_Pass()
        {
            // Arange

            // Act
            var result = new ShopTruckModel();

            // Reset
            DataSourceBackend.Instance.Reset();

            // Assert
            Assert.IsNotNull(result, TestContext.TestName);

            Assert.IsNotNull(result.Truck, "Truck " + TestContext.TestName);
            Assert.IsNotNull(result.Menu, "Menu "+ TestContext.TestName);
            Assert.IsNotNull(result.Sign, "Sign " + TestContext.TestName);
            Assert.IsNotNull(result.Trailer, "Trailer " + TestContext.TestName);
            Assert.IsNotNull(result.Topper, "Topper " + TestContext.TestName);
            Assert.IsNotNull(result.Wheels, "Wheels " + TestContext.TestName);
            Assert.IsNotNull(result.IsClosed, "Closed " + TestContext.TestName);
        }

        #endregion Instantiate
    }
}
