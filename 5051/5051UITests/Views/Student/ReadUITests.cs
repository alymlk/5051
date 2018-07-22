﻿using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static _5051UITests.Extensions;

namespace _5051UITests.Views.Student
{
    [TestClass]
    public class ReadUITests
    {
        private string _Controller = "Student";
        private string _Action = "Read";
        private string _DataFirstStudentID = GetFirstStudentID(AssemblyTests.CurrentDriver);

        [TestMethod]
        public void Student_Read_NavigateToPage_Valid_Should_Pass()
        {
            NavigateToPage(AssemblyTests.CurrentDriver, _Controller, _Action, _DataFirstStudentID);
        }

    }
}
