using BulkInsertDemo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BulkInsertDemo;

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

        services.AddSwaggerGen();

        // TODO set proper values in connection string
        services.AddDbContext<BulkContext>(options => 
            options.UseSqlServer("Server=localhost;Database=bulktest;User Id=bulker;Password=Bulk1234;"));

        // TODO define inserter to use
        services.AddScoped<IStockUpdateHandler, RegularInserter>();
        //services.AddScoped<IStockUpdateHandler, BulkInserter>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}