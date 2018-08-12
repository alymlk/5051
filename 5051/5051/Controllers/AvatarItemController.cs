﻿using System.Web.Mvc;
using System.Linq;
using _5051.Models;
using _5051.Backend;

namespace _5051.Controllers
{
    /// <summary>
    /// AvatarItem Contoller manages the AvatarItem web pages including how to make new ones, change them, and delete them
    /// </summary>
    public class AvatarItemController : BaseController
    {
        // A ViewModel used for the AvatarItem that contains the AvatarItemList
        private AvatarItemViewModel AvatarItemViewModel = new AvatarItemViewModel();

        // The Backend Data source
        private AvatarItemBackend AvatarItemBackend = AvatarItemBackend.Instance;

        // GET: AvatarItem
        /// <summary>
        /// Index, the page that shows all the AvatarItems
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Load the list of data into the AvatarItemList
            AvatarItemViewModel.AvatarItemList = AvatarItemBackend.Index();

            return View(AvatarItemViewModel);
        }

        /// <summary>
        /// Read information on a single AvatarItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AvatarItem/Details/5
        public ActionResult Read(string id = null)
        {
            var myData = AvatarItemBackend.Read(id);
            return View(myData);
        }

        /// <summary>
        /// This opens up the make a new AvatarItem screen
        /// </summary>
        /// <returns></returns>
        // GET: AvatarItem/Create
        public ActionResult Create()
        {
            var myData = new AvatarItemModel();
            return View(myData);
        }

        /// <summary>
        /// Make a new AvatarItem sent in by the create AvatarItem screen
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        // POST: AvatarItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include=
                                        "Id,"+
                                        "Name,"+
                                        "Description,"+
                                        "Uri,"+
                                        "Level,"+
                                        "")] AvatarItemModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit, with Error Message
                return View(data);
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", new { route = "Home", action = "Error" });
            }

            if (string.IsNullOrEmpty(data.Id))
            {
                // Sind back for Edit
                return View(data);
            }

            AvatarItemBackend.Create(data);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This will show the details of the AvatarItem to update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AvatarItem/Edit/5
        public ActionResult Update(string id = null)
        {
            var myData = AvatarItemBackend.Read(id);
            return View(myData);
        }

        /// <summary>
        /// This updates the AvatarItem based on the information posted from the udpate page
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST: AvatarItem/Update/5
        [HttpPost]
        public ActionResult Update([Bind(Include=
                                        "Id,"+
                                        "Name,"+
                                        "Description,"+
                                        "Uri,"+
                                        "Level,"+
                                        "")] AvatarItemModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit
                return View(data);
            }

            if (data == null)
            {
                // Send to error page
                return RedirectToAction("Error", new { route = "Home", action = "Error" });
            }

            if (string.IsNullOrEmpty(data.Id))
            {
                // Send back for Edit
                return View(data);
            }

            AvatarItemBackend.Update(data);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This shows the AvatarItem info to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AvatarItem/Delete/5
        public ActionResult Delete(string id = null)
        {
            var myData = AvatarItemBackend.Read(id);
            return View(myData);
        }

        /// <summary>
        /// This deletes the AvatarItem sent up as a post from the AvatarItem delete page
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST: AvatarItem/Delete/5
        [HttpPost]
        public ActionResult Delete([Bind(Include=
                                        "Id,"+
                                        "Name,"+
                                        "Description,"+
                                        "Uri,"+
                                        "Level,"+
                                        "")] AvatarItemModel data)
        {
            if (!ModelState.IsValid)
            {
                // Send back for edit
                return View(data);
            }

            if (data == null)
            {
                // Send to Error Page
                return RedirectToAction("Error", new { route = "Home", action = "Error" });
            }

            if (string.IsNullOrEmpty(data.Id))
            {
                // Send back for Edit
                return View(data);
            }

            AvatarItemBackend.Delete(data.Id);

            return RedirectToAction("Index");
        }
    }
}
