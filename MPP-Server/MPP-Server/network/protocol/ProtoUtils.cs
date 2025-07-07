using Protocol;
using System.Collections.Generic;

namespace MPP_Server.network.protocol
{
    public static class ProtoUtils
    {
        public static MPP_Server.model.User fromProto(User user)
        {
            return new MPP_Server.model.User(user.Username, user.Password);
        }

        public static MPP_Server.model.Participant fromProto(Participant participant)
        {
            return new MPP_Server.model.Participant(participant.ID, participant.Name, participant.Points);
        }

        public static MPP_Server.model.Race fromProto(Race race)
        {
            return new MPP_Server.model.Race(race.ID, race.Referee);
        }

        public static MPP_Server.model.Participation fromProto(Participation participation)
        {
            return new MPP_Server.model.Participation(participation.ID, participation.Participant, participation.Race, participation.Points);
        }

        public static User ToProto(MPP_Server.model.User user)
        {
            return new User
            {
                Username = user.Username,
                Password = user.Password
            };
        }

        public static Participant ToProto(MPP_Server.model.Participant participant)
        {
            return new Participant
            {
                ID = participant.ID,
                Name = participant.Name,
                Points = participant.Points
            };
        }

        public static Race ToProto(MPP_Server.model.Race race)
        {
            return new Race
            {
                ID = race.ID,
                Referee = race.Referee ?? ""
            };
        }

        public static Participation ToProto(MPP_Server.model.Participation participation)
        {
            return new Participation
            {
                ID = participation.ID,
                Participant = participation.Participant,
                Race = participation.Race,
                Points = participation.Points
            };
        }

        public static Request CreateLoginRequest(MPP_Server.model.User user)
        {
            var userProto = ToProto(user);
            return new Request
            {
                Type = Request.Types.Type.Login,
                User = userProto
            };
        }

        public static Request CreateLogoutRequest(MPP_Server.model.User user)
        {
            var userProto = ToProto(user);
            return new Request
            {
                Type = Request.Types.Type.Logout,
                User = userProto
            };
        }

        public static Request CreateGetAllRequest(MPP_Server.model.User user)
        {
            var userProto = ToProto(user);
            return new Request
            {
                Type = Request.Types.Type.UpdateAll,
                User = userProto
            };
        }

        public static Response CreateOkResponse()
        {
            return new Response
            {
                Type = Response.Types.Type.Ok
            };
        }

        public static Response CreateErrorResponse(string errorMessage)
        {
            return new Response
            {
                Type = Response.Types.Type.Error,
                Error = errorMessage
            };
        }

        public static Response CreateFailResponse()
        {
            return new Response
            {
                Type = Response.Types.Type.Fail
            };
        }

        public static Response CreateUpdateAllResponse(
            IEnumerable<MPP_Server.model.Participant> participants,
            IEnumerable<MPP_Server.model.Race> races,
            IEnumerable<MPP_Server.model.Participation> participations)
        {
            var response = new Response
            {
                Type = Response.Types.Type.UpdateAll
            };

            foreach (var participant in participants)
            {
                response.Participants.Add(ToProto(participant));
            }

            foreach (var race in races)
            {
                response.Races.Add(ToProto(race));
            }

            foreach (var participation in participations)
            {
                response.Participations.Add(ToProto(participation));
            }

            return response;
        }

        public static Response CreateFindResponse(MPP_Server.model.Participant participant)
        {
            var response = new Response
            {
                Type = Response.Types.Type.Find,
                Participant = ToProto(participant)
            };
            return response;
        }

        public static Response CreateFindResponse(MPP_Server.model.Race race)
        {
            var response = new Response
            {
                Type = Response.Types.Type.Find,
                Race = ToProto(race)
            };
            return response;
        }

        public static Response CreateFindResponse(MPP_Server.model.Participation participation)
        {
            var response = new Response
            {
                Type = Response.Types.Type.Find,
                Participation = ToProto(participation)
            };
            return response;
        }

        public static Request CreateAddRequest(MPP_Server.model.Participant participant)
        {
            return new Request
            {
                Type = Request.Types.Type.Add,
                Participant = ToProto(participant)
            };
        }

        public static Request CreateAddRequest(MPP_Server.model.Race race)
        {
            return new Request
            {
                Type = Request.Types.Type.Add,
                Race = ToProto(race)
            };
        }

        public static Request CreateAddRequest(MPP_Server.model.Participation participation)
        {
            return new Request
            {
                Type = Request.Types.Type.Add,
                Participation = ToProto(participation)
            };
        }

        public static Request CreateUpdateRequest(MPP_Server.model.Participant participant)
        {
            return new Request
            {
                Type = Request.Types.Type.Update,
                Participant = ToProto(participant)
            };
        }

        public static Request CreateUpdateRequest(MPP_Server.model.Race race)
        {
            return new Request
            {
                Type = Request.Types.Type.Update,
                Race = ToProto(race)
            };
        }

        public static Request CreateUpdateRequest(MPP_Server.model.Participation participation)
        {
            return new Request
            {
                Type = Request.Types.Type.Update,
                Participation = ToProto(participation)
            };
        }

        public static Request CreateRemoveRequest(MPP_Server.model.Participant participant)
        {
            return new Request
            {
                Type = Request.Types.Type.Remove,
                Participant = ToProto(participant)
            };
        }

        public static Request CreateRemoveRequest(MPP_Server.model.Race race)
        {
            return new Request
            {
                Type = Request.Types.Type.Remove,
                Race = ToProto(race)
            };
        }

        public static Request CreateRemoveRequest(MPP_Server.model.Participation participation)
        {
            return new Request
            {
                Type = Request.Types.Type.Remove,
                Participation = ToProto(participation)
            };
        }
    }
}
