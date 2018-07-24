﻿using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static _5051UITests.Extensions;

namespace _5051UITests.Views.Portal
{
    [TestClass]
    public class GroupUITests
    {
        private string _Controller = "Portal";
        private string _Action = "Group";

        [TestMethod]
        public void Portal_Group_NavigateToPage_Valid_Should_Pass()
        {
            NavigateToPage(AssemblyTests.CurrentDriver, _Controller, _Action);
        }

        [TestMethod]
        public void Portal_Group_Click_All_Nav_Bar_And_Footer_Links()
        {
            NavigateToPage(AssemblyTests.CurrentDriver, _Controller, _Action);

            Shared._Layout.Click_All_Nav_Bar_Links(AssemblyTests.CurrentDriver, _Controller, _Action);

            Shared._Layout.Click_All_Footer_Links(AssemblyTests.CurrentDriver, _Controller, _Action);
        }
    }
}
