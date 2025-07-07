using System;

namespace MPP_Server.network.dto
{
    [Serializable]
    public class ParticipationDTO : IdentifiableDTO
    {
        public int Participant { get; set; }
        public int Race { get; set; }
        public int Points { get; set; }

        public ParticipationDTO() { }

        public ParticipationDTO(int id, int participant, int race, int points) : base(id)
        {
            Participant = participant;
            Race = race;
            Points = points;
        }

        public override string ToString() => $"ParticipationDTO {{ Id = {Id}, Participant = {Participant}, Race = {Race}, Points = {Points} }}";
    }
}