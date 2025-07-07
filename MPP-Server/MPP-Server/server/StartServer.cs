using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using MPP_Server.network.protocol;
using MPP_Server.repo.db;
using MPP_Server.service;

namespace MPP_Server.server
{
    public class StartServer
    {
        private const int DefaultPort = 12543;
        private const string DefaultIp = "127.0.0.1";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContextFactory<RaceContext>(options =>
                options.UseSqlite("Data Source=..\\..\\..\\database.db"));
            builder.Services.AddScoped<IService, Service>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS support and configure policy for React frontend origin
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            

            // Set REST URL if present in config (must be a full URL)
            var url = ConfigurationManager.AppSettings["REST_port"];
            if (!string.IsNullOrEmpty(url))
            {
                builder.WebHost.UseUrls(url);
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            // Enable CORS middleware with the policy before controllers
            app.UseCors("AllowReactApp");

            app.MapControllers();

            // Start TCP server in background after building the app and DI container
            StartTcpServer(app.Services);

            Console.WriteLine("Starting REST API");
            Console.WriteLine("Postman URL: http://localhost:8080/api/races");
            app.Run();
        }


        private static void StartTcpServer(IServiceProvider services)
        {
            var ip = ConfigurationManager.AppSettings["serverIP"] ?? DefaultIp;
            var portStr = ConfigurationManager.AppSettings["port"];
            int port = int.TryParse(portStr, out var p) ? p : DefaultPort;

            // Create a scope for the IService lifetime
            using var scope = services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IService>();

            var tcpThread = new Thread(() =>
            {
                Console.WriteLine($"Starting TCP server on {ip}:{port}");
                var tcpServer = new ProtoServer(ip, port, service);
                tcpServer.Start();
                Console.WriteLine("TCP server started.");
            })
            {
                IsBackground = true
            };
            tcpThread.Start();
        }
    }

    public class SerialChatServer : ConcurrentServer
    {
        private readonly IService server;

        public SerialChatServer(string host, int port, IService server) : base(host, port)
        {
            this.server = server;
        }

        protected override Thread CreateWorker(TcpClient client)
        {
            var worker = new Worker(server, client);
            return new Thread(worker.Run);
        }
    }

    public class ProtoServer : ConcurrentServer
    {
        private readonly IService server;

        public ProtoServer(string host, int port, IService server)
            : base(host, port)
        {
            this.server = server;
        }

        protected override Thread CreateWorker(TcpClient client)
        {
            var worker = new Worker(server, client);
            return new Thread(worker.Run);
        }
    }
}
