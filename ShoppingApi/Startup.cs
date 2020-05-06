using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingApi.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ShoppingApi.Mappers;
using System.Text.Json.Serialization;
using ShoppingApi.Services;
using ShoppingApi.Hubs;

namespace ShoppingApi
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
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                )
            );
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.IgnoreNullValues = true;
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }
            );

            services.AddControllers()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.IgnoreNullValues = true;
                    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });               
            services.AddDbContext<ShoppingDataContext>(options =>
             {
                options.UseSqlServer(Configuration.GetConnectionString("shopping"));

             });

            //services.AddAutoMapper(typeof(Startup));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddSingleton<MapperConfiguration>(mappingConfig);
            services.AddTransient<IMapCurbsideOrders, EfCurbsideMapper > ();
            services.AddSingleton<CurbsideChannel>();  // Defers creation until needed.
            services.AddHostedService<CurbsideOrderProcessor>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CurbsideHub>("curbsidehub");
            });
        }
    }
}
