
using Microsoft.EntityFrameworkCore;
using TicketAPI.Context.DB;
using TicketAPI.Services.DB;
using TicketAPI.Settings.DB;

namespace TicketAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //IConfigurationSection section = builder.Configuration.GetSection("DBSettingsBase");
        builder.Services.Configure<DBSettingsBase>(builder.Configuration.GetSection("DBSettingsBase"));
        builder.Services.AddSingleton<MongoDBService>();
        
        builder.Services.AddDbContext<DataContext>(options =>
        {
            //options.UseMongoDB()
        }
        );

        List<string> urls = [];

        if (urls != null && urls.Count > 0)
            builder.WebHost.UseUrls(urls.ToArray());
        else builder.WebHost.UseUrls("http://localhost:35053", "https://localhost:35054");



        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();

        
        List<string>? getURLs() {
            IConfigurationSection section1 = builder.Configuration.GetSection("AppHTTPSettings");

            string? http = section1?.GetSection("HTTPPort")?.Value;
            string? https = section1?.GetSection("HTTPSPort")?.Value;

            if (!string.IsNullOrEmpty(http) && !string.IsNullOrEmpty(https))
            {
                List<string> urls = new List<string>();
                urls.Add("http://localhost:" + http);
                urls.Add("https://localhost:" + https);

                http = section1?.GetSection("HTTPPort_LoopBack")?.Value;
                https = section1?.GetSection("HTTPSPort_LoopBack")?.Value;
                if (!string.IsNullOrEmpty(http) && !string.IsNullOrEmpty(https))
                {
                    urls.Add("http://127.0.0.1:" + http);
                    urls.Add("https://127.0.0.1:" + https);
                }
                string localIP = Miths.Utils.NetUtils.LocalIPAddress()?.ToString();
                http = section1?.GetSection("HTTPPort_LocalIP")?.Value;
                https = section1?.GetSection("HTTPSPort_LocalIP")?.Value;
                if (!string.IsNullOrEmpty(localIP) && !string.IsNullOrEmpty(http) && !string.IsNullOrEmpty(https))
                {
                    urls.Add("http://" + localIP + ":" + http);
                    urls.Add("https://" + localIP + ":" + https);
                }
                return urls;
            }
            else return null;

        }
    }
}
