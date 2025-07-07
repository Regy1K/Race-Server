using System;

namespace MPP_Server.network.dto
{
    [Serializable]
    public class IdentifiableDTO
    {
        public int Id { get; set; }

        public IdentifiableDTO() { }

        public IdentifiableDTO(int id)
        {
            Id = id;
        }

        public override string ToString() => $"Id: {Id}";
    }
}