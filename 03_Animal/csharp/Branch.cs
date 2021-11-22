namespace Animal
{
    public class Branch
    {
        public string Text { get; set; }

        public bool IsEnd => Yes == null && No == null;

        public Branch Yes { get; set; }

        public Branch No { get; set; }

        public override string ToString()
        {
            return $"{Text} : IsEnd {IsEnd}";
        }
    }
}