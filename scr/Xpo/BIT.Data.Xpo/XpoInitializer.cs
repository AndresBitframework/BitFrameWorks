﻿using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;

namespace BIT.Data.Xpo
{
    public class XpoInitializer
    {

        readonly Type[] entityTypes;
        readonly string connectionString;
        public XpoInitializer(string connectionString, params Type[] entityTypes)
        {
            this.entityTypes = entityTypes;
            this.connectionString = connectionString;
        }

        public void InitXpo()
        {
            var dictionary = PrepareDictionary();



            if (XpoDefault.DataLayer == null)
            {
                using (var updateDataLayer = XpoDefault.GetDataLayer(connectionString, dictionary, AutoCreateOption.DatabaseAndSchema))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(entityTypes));
                }
            }

            var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


        }
        public void InitXpo(IDataStore DataStore)
        {
            var dictionary = PrepareDictionary();



            if (XpoDefault.DataLayer == null)
            {
                using (var updateDataLayer = new SimpleDataLayer(dictionary, DataStore))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(entityTypes));
                }
            }

            var dataStore = DataStore;
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


        }
        public UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
        XPDictionary PrepareDictionary()
        {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(entityTypes);
            return dict;
        }
    }
}
