using System;

namespace MPP_Server.network.dto
{
    [Serializable]
    public class ParticipantDTO : IdentifiableDTO
    {
        public int Points { get; set; }
        public string? Name { get; set; }

        public ParticipantDTO() { }

        public ParticipantDTO(int id, string name, int points) : base(id)
        {
            Name = name;
            Points = points;
        }

        public override string ToString() => $"ParticipantDTO {{ Id = {Id}, Name = {Name}, Points = {Points} }}";
    }
}