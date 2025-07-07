namespace MPP_Server.model
{
    public class Participant : Identifiable
    {
        public string Name { get; set; }
        public int Points { get; set; } = 0;

        public Participant(int id, string name, int points = 0) : base(id)
        {
            Name = name;
            Points = points;
        }

        public override string ToString()
        {
            return "Participant " + base.ToString() + $" {Name}, " + Points + " points.";
        }
    }
}