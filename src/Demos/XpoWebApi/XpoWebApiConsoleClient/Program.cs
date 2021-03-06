﻿


using BIT.Data.Xpo;
using BIT.Xpo.Providers.WebApi.Client;
using DevExpress.Xpo;
using System;
using XpoDemoOrm;

namespace XpoWebApiConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            XPOWebApi.Register();

            var ConnectionString = XPOWebApi.GetConnectionString("https://localhost:44393", "/XpoWebApi", string.Empty, "db1");
            XpoInitializer xpoInitializer = new XpoInitializer(ConnectionString,typeof(Invoice),typeof(Customer));
            xpoInitializer.InitXpo(XpoDefault.GetConnectionProvider(ConnectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema));
            using (var UoW = xpoInitializer.CreateUnitOfWork())
            {
                var faker = new Bogus.Faker<Customer>().CustomInstantiator(c => new Customer(UoW))
                            .RuleFor(p => p.Name, f => f.Name.FullName())
                            .RuleFor(p => p.Active, p => p.Random.Bool());

                var Customers = faker.Generate(100);
                if (UoW.InTransaction)
                    UoW.CommitChanges();
            }
;
        }
    }
}
