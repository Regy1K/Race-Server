using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MPP_Server.server
{
    public abstract class AbstractServer
    {
        private TcpListener server;
        private readonly string host;
        private readonly int port;
        private volatile bool running = false;  // to allow graceful stop

        public AbstractServer(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Start()
        {
            var adr = IPAddress.Parse(host);
            var ep = new IPEndPoint(adr, port);
            server = new TcpListener(ep);
            server.Start();
            running = true;
            Console.WriteLine($"Server started at {host}:{port}");

            try
            {
                while (running)
                {
                    Console.WriteLine("Waiting for clients ...");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected ...");
                    ProcessRequest(client);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            finally
            {
                Stop();
            }
        }

        public void Stop()
        {
            running = false;
            server?.Stop();
            Console.WriteLine("Server stopped.");
        }

        protected abstract void ProcessRequest(TcpClient client);
    }

    public abstract class ConcurrentServer : AbstractServer
    {
        protected ConcurrentServer(string host, int port) : base(host, port)
        {
        }

        protected override void ProcessRequest(TcpClient client)
        {
            var workerThread = CreateWorker(client);
            workerThread.Start();
        }

        protected abstract Thread CreateWorker(TcpClient client);
    }
}