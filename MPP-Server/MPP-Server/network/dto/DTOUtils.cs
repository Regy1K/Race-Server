using MPP_Server.network.dto;
using MPP_Server.model;
using System;

namespace MPP_Server.network.dto
{
    public class DTOUtils
    {
        public static ParticipantDTO GetDTO(Participant p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            return new ParticipantDTO(p.ID, p.Name, p.Points);
        }

        public static Participant GetFromDTO(ParticipantDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Participant(dto.Id, dto.Name, dto.Points);
        }

        public static RaceDTO GetDTO(Race r)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));
            return new RaceDTO(r.ID, r.Referee);
        }

        public static Race GetFromDTO(RaceDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Race(dto.Id, dto.Referee);
        }

        public static ParticipationDTO GetDTO(Participation p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            return new ParticipationDTO(p.ID, p.Participant, p.Race, p.Points);
        }

        public static Participation GetFromDTO(ParticipationDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Participation(dto.Id, dto.Participant, dto.Race, dto.Points);
        }

        public static ParticipantDTO[] GetDTO(Participant[] participants)
        {
            if (participants == null) throw new ArgumentNullException(nameof(participants));
            var dtos = new ParticipantDTO[participants.Length];
            for (int i = 0; i < participants.Length; i++)
            {
                dtos[i] = GetDTO(participants[i]);
            }
            return dtos;
        }

        public static Participant[] GetFromDTO(ParticipantDTO[] dtos)
        {
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));
            var participants = new Participant[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                participants[i] = GetFromDTO(dtos[i]);
            }
            return participants;
        }

        public static RaceDTO[] GetDTO(Race[] races)
        {
            if (races == null) throw new ArgumentNullException(nameof(races));
            var dtos = new RaceDTO[races.Length];
            for (int i = 0; i < races.Length; i++)
            {
                dtos[i] = GetDTO(races[i]);
            }
            return dtos;
        }

        public static Race[] GetFromDTO(RaceDTO[] dtos)
        {
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));
            var races = new Race[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                races[i] = GetFromDTO(dtos[i]);
            }
            return races;
        }

        public static ParticipationDTO[] GetDTO(Participation[] participations)
        {
            if (participations == null) throw new ArgumentNullException(nameof(participations));
            var dtos = new ParticipationDTO[participations.Length];
            for (int i = 0; i < participations.Length; i++)
            {
                dtos[i] = GetDTO(participations[i]);
            }
            return dtos;
        }

        public static Participation[] GetFromDTO(ParticipationDTO[] dtos)
        {
            if (dtos == null) throw new ArgumentNullException(nameof(dtos));
            var participations = new Participation[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                participations[i] = GetFromDTO(dtos[i]);
            }
            return participations;
        }

        public static UserDTO GetDTO(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return new UserDTO(user.Username, user.Password);
        }

        public static User GetFromDTO(UserDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new User(dto.Username, dto.Password);
        }
    }
}
