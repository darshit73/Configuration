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

namespace Configuration
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use key that is store in appsettings.json file keys are case insensitive 
            Console.WriteLine($"Mykey : {Configuration["MyKey"]}");
            // if key define in both file development.json and appsetting.json than development file run first.
            
            //low to high priority
            //appsettings.json < appsettings.Development.json < command line < Environment(on computer) 
            // for hierarchy use : operator
            
            
            // we can also use middleware 

            // app.Use(async (context, next) =>
            // {
            //     Console.WriteLine($"MyKey : {Configuration["MyKey"]}");
            //     Console.WriteLine($"MyApi.Url Value : {Configuration["MyApi:Url"]}");
            //     Console.WriteLine($"MyKey.ApiKey Value : {Configuration["MyApi:ApiKey"]}");
            //
            //     await next();
            // });
            
            // we can use option pattern for retrieving those value 
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"MyKey : {Configuration["MyKey"]}");
                var apiOptions = new MyOptions();  //object of the class
                // GetSection Method look for MyApi key if that key found than it Match the fields
                // with apiOptions class using Bind method
                Configuration.GetSection("MyApi").Bind(apiOptions);
                
                // above two line of code also write like this
                // var apiOptions = Configuration.GetSection("MyApi").Get<MyOptions>();
                
                Console.WriteLine($"MyApi.Url Value : {apiOptions.Url}");
                Console.WriteLine($"MyKey.ApiKey Value : {apiOptions.ApiKey}");
            
                await next();
            });
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}