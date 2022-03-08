using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net.Http;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using WikipediaChecks.Interfaces;
using WikipediaChecks.Services;

namespace WikipediaChecks
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
                options.AddPolicy(name: "AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("https://vx.azurewebsites.net")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddSingleton<HttpClient>();
            services.AddScoped<IWikipediaWebClient, WikipediaWebClient>();
            services.AddScoped<IWikiTextService, WikiTextService>();
            services.AddScoped<IWikipediaService, WikipediaService>();
            services.AddScoped<IWikidataService, WikidataService>();
            services.AddScoped<IToolforgeService, ToolforgeService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" }
            });
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
