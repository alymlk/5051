﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using _5051.Models;
namespace _5051.Backend
{
    /// <summary>
    /// Backend Mock DataSource for ShopInventorys, to manage them
    /// </summary>
    public class ShopInventoryDataSourceMock : IShopInventoryInterface
    {
        /// <summary>
        /// Make into a Singleton
        /// </summary>
        private static volatile ShopInventoryDataSourceMock instance;
        private static object syncRoot = new Object();

        private ShopInventoryDataSourceMock() { }

        public static ShopInventoryDataSourceMock Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ShopInventoryDataSourceMock();
                            instance.Initialize();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// The ShopInventory List
        /// </summary>
        private List<ShopInventoryModel> ShopInventoryList = new List<ShopInventoryModel>();

        /// <summary>
        /// Makes a new ShopInventory
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ShopInventory Passed In</returns>
        public ShopInventoryModel Create(ShopInventoryModel data)
        {
            ShopInventoryList.Add(data);
            return data;
        }

        /// <summary>
        /// Return the data for the id passed in
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Null or valid data</returns>
        public ShopInventoryModel Read(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var myReturn = ShopInventoryList.Find(n => n.Id == id);
            return myReturn;
        }

        /// <summary>
        /// Update all attributes to be what is passed in
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Null or updated data</returns>
        public ShopInventoryModel Update(ShopInventoryModel data)
        {
            if (data == null)
            {
                return null;
            }
            var myReturn = ShopInventoryList.Find(n => n.Id == data.Id);

            myReturn.Update(data);

            return myReturn;
        }

        /// <summary>
        /// Remove the Data item if it is in the list
        /// </summary>
        /// <param name="data"></param>
        /// <returns>True for success, else false</returns>
        public bool Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return false;
            }

            var myData = ShopInventoryList.Find(n => n.Id == Id);
            var myReturn = ShopInventoryList.Remove(myData);
            return myReturn;
        }

        /// <summary>
        /// Return the full dataset
        /// </summary>
        /// <returns>List of ShopInventorys</returns>
        public List<ShopInventoryModel> Index()
        {
            return ShopInventoryList;
        }

        /// <summary>
        /// Reset the Data, and reload it
        /// </summary>
        public void Reset()
        {
            Initialize();
        }

        /// <summary>
        /// Create Placeholder Initial Data
        /// </summary>
        public void Initialize()
        {
            LoadDataSet(DataSourceDataSetEnum.Default);
        }

        /// <summary>
        /// Clears the Data
        /// </summary>
        private void DataSetClear()
        {
            ShopInventoryList.Clear();
        }

        /// <summary>
        /// The Defalt Data Set
        /// </summary>
        private void DataSetDefault()
        {
            DataSetClear();
            var count = 0;
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Police", "Happy Officer", 1));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Kunoichi", "Ninja Lady", 2));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Angry", "Angry, but happy", 1));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Playfull", "Anyone want a ride?", 1));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Pirate", "Where is my ship?", 2));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Blue", "Having a Blue Day", 3));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Pigtails", "Love my hair", 3));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Ninja", "Taste my Katana", 2));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Circus", "Swinging from the Trapeese", 4));
            Create(new ShopInventoryModel("ShopInventory" + count++.ToString() + ".png", "Chef", "I love to cook", 4));
        }

        /// <summary>
        /// Data set for demo
        /// </summary>
        private void DataSetDemo()
        {
            DataSetDefault();
        }

        /// <summary>
        /// Unit Test data set
        /// </summary>
        private void DataSetUnitTest()
        {
            DataSetDefault();
        }

        /// <summary>
        /// Set which data to load
        /// </summary>
        /// <param name="setEnum"></param>
        public void LoadDataSet(DataSourceDataSetEnum setEnum)
        {
            switch (setEnum)
            {
                case DataSourceDataSetEnum.Demo:
                    DataSetDemo();
                    break;

                case DataSourceDataSetEnum.UnitTest:
                    DataSetUnitTest();
                    break;

                case DataSourceDataSetEnum.Default:
                default:
                    DataSetDefault();
                    break;
            }
        }
    }
}