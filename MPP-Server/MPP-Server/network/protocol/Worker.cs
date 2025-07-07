using Google.Protobuf;
using Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MPP_Server.service;
using Participant = MPP_Server.model.Participant;
using Participation = MPP_Server.model.Participation;
using Race = MPP_Server.model.Race;

namespace MPP_Server.network.protocol
{
    public class Worker : IObserver
    {
        private readonly IService server;
        private readonly TcpClient connection;
        private readonly NetworkStream stream;
        private volatile bool connected;

        public Worker(IService server, TcpClient connection)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.stream = connection.GetStream();
            this.connected = true;
        }

        public void Run()
        {
            while (connected)
            {
                try
                {
                    Request request = Request.Parser.ParseDelimitedFrom(stream);
                    if (request == null)
                    {
                        Disconnect();
                        break;
                    }

                    Response? response = HandleRequest(request);
                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in worker loop: {e}");
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {
            connected = false;
            try
            {
                stream?.Close();
                connection?.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error closing connection: {e}");
            }
        }

        private Response? HandleRequest(Request request)
        {
            try
            {
                switch (request.Type)
                {
                    case Request.Types.Type.Login:
                        Console.WriteLine("Login request ...");
                        var userLogin = ProtoUtils.fromProto(request.User);
                        lock (server)
                        {
                            if (server.Login(userLogin, this))
                                return ProtoUtils.CreateOkResponse();
                        }
                        return ProtoUtils.CreateFailResponse();

                    case Request.Types.Type.Logout:
                        Console.WriteLine("Logout request");
                        var userLogout = ProtoUtils.fromProto(request.User);
                        lock (server)
                        {
                            server.Logout(userLogout);
                        }
                        connected = false;
                        return ProtoUtils.CreateOkResponse();

                    case Request.Types.Type.Add:
                        return HandleAddRequest(request);

                    case Request.Types.Type.Remove:
                        return HandleRemoveRequest(request);

                    case Request.Types.Type.Update:
                        return HandleUpdateRequest(request);

                    case Request.Types.Type.Find:
                        return HandleFindRequest(request);

                    case Request.Types.Type.UpdateAll:
                        ((Service)server).UpdateObserver(ProtoUtils.fromProto(request.User));
                        return null;

                    default:
                        return ProtoUtils.CreateErrorResponse("Unknown request type");
                }
            }
            catch (Exception e)
            {
                return ProtoUtils.CreateErrorResponse(e.Message);
            }
        }

        protected Response HandleAddRequest(Request request)
        {
            try
            {
                bool result = false;
                if (request.Participant != null)
                    result = server.AddParticipant(request.Participant.ID, request.Participant.Name);
                else if (request.Race != null)
                    result = server.AddRace(ProtoUtils.fromProto(request.Race)) != null;
                else if (request.Participation != null)
                {
                    var p = request.Participation;
                    result = server.AddParticipation(p.ID, p.Participant, p.Race, p.Points);
                }
                else
                {
                    return ProtoUtils.CreateErrorResponse("Invalid Add request");
                }

                return result ? ProtoUtils.CreateOkResponse() : ProtoUtils.CreateFailResponse();
            }
            catch (Exception e)
            {
                return ProtoUtils.CreateErrorResponse(e.Message);
            }
        }

        protected Response HandleRemoveRequest(Request request)
        {
            try
            {
                bool result = false;
                if (request.Participant != null)
                    result = server.RemoveParticipant(request.Participant.ID);
                else if (request.Race != null)
                    result = server.RemoveRace(request.Race.ID);
                else if (request.Participation != null)
                    result = server.RemoveParticipation(request.Participation.ID);
                else
                    return ProtoUtils.CreateErrorResponse("Invalid Remove request");

                return result ? ProtoUtils.CreateOkResponse() : ProtoUtils.CreateFailResponse();
            }
            catch (Exception e)
            {
                return ProtoUtils.CreateErrorResponse(e.Message);
            }
        }

        protected Response HandleUpdateRequest(Request request)
        {
            try
            {
                bool result = false;
                if (request.Participant != null)
                    result = server.UpdateParticipant(request.Participant.ID, request.Participant.Name);
                else if (request.Race != null)
                    result = server.UpdateRace(ProtoUtils.fromProto(request.Race));
                else if (request.Participation != null)
                {
                    var p = request.Participation;
                    result = server.UpdateParticipation(p.ID, p.Participant, p.Race, p.Points);
                }
                else
                    return ProtoUtils.CreateErrorResponse("Invalid Update request");

                return result ? ProtoUtils.CreateOkResponse() : ProtoUtils.CreateFailResponse();
            }
            catch (Exception e)
            {
                return ProtoUtils.CreateErrorResponse(e.Message);
            }
        }

        protected Response HandleFindRequest(Request request)
        {
            try
            {
                if (request.Participant != null)
                {
                    var result = server.FindParticipant(request.Participant.ID);
                    return ProtoUtils.CreateFindResponse(result);
                }
                else if (request.Race != null)
                {
                    var result = server.FindRace(request.Race.ID);
                    return ProtoUtils.CreateFindResponse(result);
                }
                else if (request.Participation != null)
                {
                    var result = server.FindParticipation(request.Participation.ID);
                    return ProtoUtils.CreateFindResponse(result);
                }
                else
                {
                    return ProtoUtils.CreateErrorResponse("Invalid Find request");
                }
            }
            catch (Exception e)
            {
                return ProtoUtils.CreateErrorResponse(e.Message);
            }
        }

        private void SendResponse(Response response)
        {
            Console.WriteLine("Sending response: " + response);
            lock (stream)
            {
                response.WriteDelimitedTo(stream);
                stream.Flush();
            }
        }

        public void update(ICollection<Participant> participants, ICollection<Race> races, ICollection<Participation> participations)
        {
            SendResponse(ProtoUtils.CreateUpdateAllResponse(participants, races, participations));
        }
    }
}
    