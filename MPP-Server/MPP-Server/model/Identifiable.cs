namespace MPP_Server.model
{
    public class Identifiable
    {
        public int ID { get; set; }

        public Identifiable() { }

        public Identifiable(int id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return $"{ID}";
        }
    }
}