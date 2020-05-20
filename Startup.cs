using Asnapper.Hal101.Data;
using Asnapper.Hal101.Models;
using Asnapper.Hal101.Models.Hypermedia;
using Asnapper.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Asnapper.Hal101
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new HalJsonOutputFormatter());
            });

            services.AddSingleton<AddressRelationRepository>();
            services.AddSingleton<PeopleRepository>();
            services.AddSingleton<AddressRepository>();
            
            services.AddTransient<PersonRepresentation>();
            services.AddTransient<Hal<Person>, PersonRepresentation>();
            services.AddTransient<Hal<PagedList<Person>>, PersonListRepresentation>();

            services.AddTransient<AddressRepresentation>();
            services.AddTransient<Hal<Address>, AddressRepresentation>();
            services.AddTransient<Hal<PagedList<Address>>, AddressListRepresentation>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}