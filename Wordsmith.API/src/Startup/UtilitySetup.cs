using Wordsmith.Utils;
using Wordsmith.Utils.EBookFileHelper;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.API.Startup;

public static class UtilitySetup
{
    public static void Init(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Logger.Init(configuration["Logging:NLog:LogLevel"] ?? "Debug");
        RabbitService.Init(
            configuration["Connection:RabbitMQ:Host"],
            configuration["Connection:RabbitMQ:User"],
            configuration["Connection:RabbitMQ:Password"]);
        
        var webRootPath = environment.WebRootPath;
        var imageSettings = configuration.GetSection("ImageSettings");
        var eBookSettings = configuration.GetSection("EBookSettings");

        ImageHelper.Init(webRootPath, imageSettings["AllowedSize"], imageSettings["AllowedFormats"]);
        EBookFileHelper.Init(eBookSettings["SavePath"]);
    }
}