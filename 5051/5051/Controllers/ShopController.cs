﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _5051.Models;
using _5051.Backend;

namespace _5051.Controllers
{
    public class ShopController : BaseController
    {
        private FactoryInventoryViewModel FactoryInventoryViewModel = new FactoryInventoryViewModel();

        // The Backend Data source
        private FactoryInventoryBackend FactoryInventoryBackend = FactoryInventoryBackend.Instance;

        /// <summary>
        /// Index to the Shop
        /// </summary>
        /// <returns></returns>
        // GET: Shop
        public ActionResult Index(string id = null)
        {
            // Get the Student
            var myStudent = StudentBackend.Instance.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(myStudent);
        }

        /// <summary>
        /// What to Buy at the store
        /// </summary>
        /// <returns></returns>
        // GET: Buy
        public ActionResult Factory(string id = null)
        {
            // Load the list of data into the FactoryInventoryList
            var myData = new SelectedFactoryInventoryForStudentViewModel();

            // Get the Student
            var myStudent = StudentBackend.Instance.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }
            myData.Student = myStudent;

            // Get the Inventory
            var InventoryList = FactoryInventoryBackend.Index();

            // Sort the Inventory into List per Category
            // Load the ones
            foreach (var item in Enum.GetValues(typeof(FactoryInventoryCategoryEnum)))
            {
                var temp = new FactoryInventoryViewModel();
                temp.Category = (FactoryInventoryCategoryEnum)item;
                temp.FactoryInventoryList = InventoryList.Where(m => m.Category == (FactoryInventoryCategoryEnum)item).ToList();

                if (temp.FactoryInventoryList.Any())
                {
                    // todo, tag the ones that are already owned
                    myData.FactoryInventoryCategoryList.Add(temp);
                }
            }

            return View(myData);
        }
        [HttpPost]
        public ActionResult Factory([Bind(Include=
                                        "StudentId,"+
                                        "ItemId,"+
                                        "")] ShopBuyViewModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", new { route = "Home", action = "Error" });
            }

            if (string.IsNullOrEmpty(data.StudentId))
            {
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            if (string.IsNullOrEmpty(data.ItemId))
            {
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // Get Student
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(data.StudentId);
            if (myStudent == null)
            {
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            

            // Get Item
            var myItem = DataSourceBackend.Instance.FactoryInventoryBackend.Read(data.ItemId);
            if (myItem == null)
            {
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // Check the Student Token amount, If not enough, return error
            if (myStudent.Tokens < myItem.Tokens)
            {
                // Not enough money...
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // check the quantities of item
            if (myItem.Quantities < 1)
            {
                // Not enough quantity...
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // Check the Item amount, If not enough, return error
            if (myItem.Tokens < 1)
            {
                // Not enough money...
                // Send back for Edit
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // Check to see if the student already has the item.  If so, don't buy again, only 1 item per student
            var ItemAlreadyExists = myStudent.Inventory.Exists(m => m.Id == myItem.Id);
            if (ItemAlreadyExists)
            {
                // Already own it.
                return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
            }

            // Reduce the Student Tokens by Item Price
            myStudent.Tokens -= myItem.Tokens;

            // Time to buy !
            // Check if the item is limited or not
            if (myItem.IsLimitSupply == true)
            {
                // Reduce the quantities of Item
                myItem.Quantities -= 1;
            }                           

            // Add Item to Student Inventory
            // TODO:  Mike, add inventory to Students...
            myStudent.Inventory.Add(myItem);

            // Update Student
            DataSourceBackend.Instance.StudentBackend.Update(myStudent);

            return RedirectToAction("Factory", "Shop", new { id = data.StudentId });
        }

        /// <summary>
        /// Things on sale at the store
        /// </summary>
        /// <returns></returns>
        // GET: Discounts
        public ActionResult Discounts()
        {
            return View();
        }

        /// <summary>
        /// Unique items to get at the store
        /// </summary>
        /// <returns></returns>
        // GET: Specials
        public ActionResult Specials()
        {
            return View();
        }

        /// <summary>
        /// store items of the shop
        /// </summary>
        /// <returns></returns>
        // GET: Inventory
        public ActionResult Inventory(string id = null)
        {
            var myData = new SelectedFactoryInventoryForStudentViewModel();

            // Get the Student
            var myStudent = StudentBackend.Instance.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }
            myData.Student = myStudent;

            var InventoryList = myStudent.Inventory;


            foreach (var item in Enum.GetValues(typeof(FactoryInventoryCategoryEnum)))
            {
                var temp = new FactoryInventoryViewModel();
                temp.Category = (FactoryInventoryCategoryEnum)item;
                temp.FactoryInventoryList = InventoryList.Where(m => m.Category == (FactoryInventoryCategoryEnum)item).ToList();

                if (temp.FactoryInventoryList.Any())
                {
                    // todo, tag the ones that are already owned
                    myData.FactoryInventoryCategoryList.Add(temp);
                }

            }
            return View(myData);
        }

        [HttpPost]
        public ActionResult Inventory([Bind(Include=
                                        "StudentId,"+
                                        "ItemId,"+
                                        "")] ShopBuyViewModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", new { route = "Home", action = "Error" });
            }

            if (string.IsNullOrEmpty(data.StudentId))
            {
                // Send back for Edit
                return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });
            }

            if (string.IsNullOrEmpty(data.ItemId))
            {
                // Send back for Edit
                return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });
            }

            // Get Student
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(data.StudentId);
            if (myStudent == null)
            {
                // Send back for Edit
                return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });
            }

            // Get Item
            var myItem = DataSourceBackend.Instance.FactoryInventoryBackend.Read(data.ItemId);
            if (myItem == null)
            {
                // Send back for Edit
                return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });
            }

            // Reduce Item to Student Inventory
            // TODO:  Mike, remove inventory to Students...
            myStudent.Inventory.Remove(myItem);

            // Update Student
            DataSourceBackend.Instance.StudentBackend.Update(myStudent);

            return RedirectToAction("Inventory", "Shop", new { id = data.StudentId });

        }
        /// <summary>
        /// Edit The Shop Details
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id=null)
        {
            // Allow Editing of the Shop details including
            // Shop Name
            // Truck Items

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error", "Home");
            }

            var studentdata = DataSourceBackend.Instance.StudentBackend.Read(id);
            if (studentdata == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (studentdata.Truck == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var data = FactoryInventoryBackend.GetShopTruckViewModel(studentdata);

            //Return Truck Data
            return View(data);
        }

        /// <summary>
        /// Update the Current Inventory Slot to use tha Item passed up.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include=
                                        "StudentId,"+
                                        "ItemId,"+
                                        "Position,"+
                                        "")] ShopTruckInputModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            if (data == null)
            {
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            if (string.IsNullOrEmpty(data.StudentId))
            {
                // Send back for Edit
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            if (string.IsNullOrEmpty(data.ItemId))
            {
                // Send back for Edit
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            // Get Student
            var myStudent = DataSourceBackend.Instance.StudentBackend.Read(data.StudentId);
            if (myStudent == null)
            {
                // Send back for Edit
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            // Get Item
            var myItem = DataSourceBackend.Instance.FactoryInventoryBackend.Read(data.ItemId);
            if (myItem == null)
            {
                // Send back for Edit
                return RedirectToAction("Index", "Shop", new { id = data.StudentId });
            }

            switch (data.Position)
            {
                case FactoryInventoryCategoryEnum.Truck:
                    myStudent.Truck.Truck = myItem.Id;
                    break;

                case FactoryInventoryCategoryEnum.Topper:
                    myStudent.Truck.Topper = myItem.Id;
                    break;

                case FactoryInventoryCategoryEnum.Menu:
                    myStudent.Truck.Menu = myItem.Id;
                    break;

                case FactoryInventoryCategoryEnum.Sign:
                    myStudent.Truck.Sign = myItem.Id;
                    break;

                case FactoryInventoryCategoryEnum.Wheels:
                    myStudent.Truck.Wheels = myItem.Id;
                    break;

                case FactoryInventoryCategoryEnum.Trailer:
                    myStudent.Truck.Trailer = myItem.Id;
                    break;
            }

            // Update Student
            DataSourceBackend.Instance.StudentBackend.Update(myStudent);

            return RedirectToAction("Index", "Shop", new { id = data.StudentId });
        }

        /// <summary>
        /// Shows the ice cream flavor preference of celebrities 
        /// </summary>
        /// <returns></returns>
        // GET: Poster
        public ActionResult CelebrityPoster()
        {
            // To do
            return View();
        }

        /// <summary>
        ///  List of Shops to Visit
        /// </summary>
        /// <returns>List of Students that you can visit</returns>
        public ActionResult Visit()
        {
            // Get the list of other students' shop
            // it will show the name of the student's shop and its owner 

            // then, once students click on the specific student's shop
            // he/she can go visiting it

            // TODO
            // Make a List of the Student IDs for now.  Update this to a Shop Datastructure
            var data = StudentBackend.Instance.Index();
            return View(data);
        }

        /// <summary>
        ///  The Shop of the Student passed in
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns>Shop of the student passed in</returns>
        public ActionResult VisitShop(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error", "Home");
            }

            var myStudent = Backend.StudentBackend.Instance.Read(id);
            if (myStudent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            //Todo, return the shop information for the student.
            var data = myStudent;

            return View(data);
        }
    }
}