using coreAPI.Data;
using coreAPI.Services;
using coreAPI.Utilities;
using CoreAPI.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace coreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Memory cache - WIP - static content - e.g. ContentType entity
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "coreAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddDbContext<UserDBContext>(op => op.UseSqlServer(Configuration.GetConnectionString("UserDB")));
   
            services.AddControllers().AddNewtonsoftJson();   //@  patch request(s)

            //suppress log(log4net)  status messages
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

            //set up mappinggs for all endponts - ref. classes @ Profiles folder
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

           // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContentTypeRepository, ContentTypeRepository>();

            #region Versioning  - currently disabled 
            //services.AddApiVersioning(opt =>
            //{
            //    opt.AssumeDefaultVersionWhenUnspecified = true;
            //     opt.DefaultApiVersion = new ApiVersion(1, 1);
            //    //opt.ReportApiVersions = true;
            //    //    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            //    //    //opt.ApiVersionReader = ApiVersionReader.Combine(
            //    //    //  new HeaderApiVersionReader("X-Version"),
            //    //    //  new QueryStringApiVersionReader("ver", "version"));

            //});
            #endregion
           
            services.AddControllers();
            services.AddCors();

            //CORS  - enable cross-origin http requests
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddMvc(opt => opt.EnableEndpointRouting = false);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreAPI  v1"));

            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }


            //custom jwt auth middleware;
            // global exeption handler
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));      

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();

           app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }


}
