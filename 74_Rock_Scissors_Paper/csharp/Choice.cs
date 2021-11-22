namespace RockScissorsPaper
{
    public class Choice
    {
        public string Selector {get; private set; }
        public string Name { get; private set; }
        internal Choice CanBeat { get; set; }

        public Choice(string selector, string name) {
            Selector = selector;
            Name = name;
        }

        public bool Beats(Choice choice)
        {
            return choice == CanBeat;
        }
    }
}
