using Castle.DynamicProxy;
using System.Text.RegularExpressions;
using BaMan.Application;
using BaMan.Shared.Types.Messages;
using BaMan.Shared.ManagedRedis;
using BaMan.Application.Features.Consumers.RPC;
using BaMan.Application.Features.Services.RPC;
using BaMan.Shared.ManagedChannels;

namespace BaMan;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddMediatR(config => { config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyRefrence>(); });

        builder.Services.AddManagedRedis(builder.Configuration)
        .AddManagedPublisher<RPCMessage>()
            .RegisterManagedConsumer<RPCMessage, RPCMessageManagedConsumer>();


        builder.Services.AddSingleton<IRPCService, RPCService>();
        builder.Services.AddManagedChannel<RPCResult, RPCResultProcessorService>();


        // infrastructure
        builder.Services.AddScoped<IRemoteService, RemoteService>();


        




        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();




        var app = builder.Build();

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
    }
}
