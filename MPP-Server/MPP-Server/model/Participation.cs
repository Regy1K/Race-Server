namespace MPP_Server.model
{
    public class Participation : Identifiable
    {
        public int Race { get; set; }
        public int Participant { get; set; }
        public int Points { get; set; }

        public Participation(int id, int participant, int race, int points) : base(id)
        {
            Race = race;
            Participant = participant;
            Points = points;
        }

        public override string ToString()
        {
            return "Participation " + base.ToString() +
                   $" [participant={Participant}, race={Race}, points={Points}]";
        }
    }
}