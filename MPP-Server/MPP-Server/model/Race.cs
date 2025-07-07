namespace MPP_Server.model
{
    public class Race : Identifiable
    {
        public string Referee { get; set; } = "";

        public Race() { }

        public Race(int id, string referee) : base(id)
        {
            Referee = referee;
        }

        public override string ToString()
        {
            return "Race " + base.ToString() + $", referee: {Referee}";
        }
    }
}