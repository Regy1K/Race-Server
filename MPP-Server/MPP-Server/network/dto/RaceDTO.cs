using System;

namespace MPP_Server.network.dto
{
    [Serializable]
    public class RaceDTO : IdentifiableDTO
    {
        public string Referee { get; set; }

        public RaceDTO() { }

        public RaceDTO(int id, string referee) : base(id)
        {
            Referee = referee;
        }

        public override string ToString() => $"RaceDTO {{ Id = {Id}, Referee = \"{Referee}\" }}";
    }
}