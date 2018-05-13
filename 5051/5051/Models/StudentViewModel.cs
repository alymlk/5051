﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5051.Models
{
    /// <summary>
    /// View Model for the Student Index, this will have the list of the students in it convered to a StudentDisplayViewModel
    /// </summary>
    public class StudentViewModel
    {
        public List<StudentDisplayViewModel> StudentList = new List<StudentDisplayViewModel>();

        public StudentViewModel() { }

        /// <summary>
        /// Take the data list passed in, and convert each to a new StudentDisplayViewModel item and add that to the StudentList
        /// </summary>
        /// <param name="dataList"></param>
        public StudentViewModel(List<StudentModel> dataList)
        {
            foreach (var item in dataList)
            {
                StudentList.Add(new StudentDisplayViewModel(item));
            }
        }
    }
}