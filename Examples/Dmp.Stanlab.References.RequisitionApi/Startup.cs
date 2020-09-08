using Dmp.Stanlab.References.RequisitionApi.Registrations;
using Dmp.Stanlab.References.RequisitionApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dmp.Stanlab.References.RequisitionApi
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            var authority = _configuration["identity:authority"];
            var audience = "requisition-api:stanlab-ref";

            services.AddJwtBearerAuthorization(authority, audience);

            // Fake in-memory repositories for contracts and submitted requisitions
            var contractRepository = new ContractRepository();
            contractRepository.Seed();

            services.AddSingleton(contractRepository);

            services.AddSingleton<RequisitionRepository>();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
