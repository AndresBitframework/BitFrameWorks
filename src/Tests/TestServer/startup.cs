using BIT.Data.Services;
using BIT.Xpo;
using BIT.Xpo.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DevExpress.Xpo.DB;
using BIT.Data.DataTransfer;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace TestServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            IConfigResolver<IDataStore> DataStoreResolver = new XpoDataStoreResolver("appsettings.json");
            IStringSerializationService stringSerializationHelper = new StringSerializationHelper();
            IObjectSerializationService objectSerializationHelper = new SimpleObjectSerializationService();
            IFunction function = new DataStoreFunctionServer(DataStoreResolver, objectSerializationHelper);
            services.AddSingleton<IConfigResolver<IDataStore>>(DataStoreResolver);
            services.AddSingleton<IStringSerializationService>(stringSerializationHelper);
            services.AddSingleton<IObjectSerializationService>(objectSerializationHelper);
            services.AddSingleton<IFunction>(function);
            //services.AddXpoWebApi();

            //TODO review this code, at the momentis needed to use the  await for the operations in the fucntion rest client
            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
