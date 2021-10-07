using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NJBudgetWBackend.Business;
using NJBudgetWBackend.Business.Interface;
using NJBudgetWBackend.Repositories;
using NJBudgetWBackend.Repositories.Interface;
using NJBudgetWBackend.Services;
using NJBudgetWBackend.Services.Interface;
using NJBudgetWBackend.Services.Interface.Interface;

namespace NJBudgetWBackend
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
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.WithOrigins("https://*",
                                                          "http://*")
                                                    .AllowAnyOrigin()
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod();
                                  });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NJBudgetWBackend", Version = "v1" });
            });
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IAppartenanceService, AppartenanceService>();
            services.AddTransient<IOperationsRepository, OperationsRepository>();
            services.AddTransient<IBudgetProcessor, BudgetProcessor>();
            services.AddTransient<IOperationService, OperationService>();
            services.AddTransient<ISyntheseService, SyntheseService>();
            services.AddTransient<ISyntheseRepository, SyntheseRepository>();
            services.AddTransient<IStatusProcessor, StatusProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NJBudgetWBackend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            //.SetIsOriginAllowed(origin =>
            //    origin.Contains("https://njbudgetfront.azurewebsites.net") ||
            //    origin.Contains("http://localhost") ||
            //    origin.Contains("https://njbudgetw.azurewebsites.net")


                //)
            );
            app.UseAuthorization();
            app.UseDeveloperExceptionPage();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
