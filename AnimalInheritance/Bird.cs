namespace AnimalInheritance
{
    public class Bird : Animal
    {
        public double WingSpan { get; set; } // in centimetres

        public Bird(string name, int age, double wingSpan) : base(name, age)
        {
            WingSpan = wingSpan;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Tweet tweet!");
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Wing span: {WingSpan} cm");
        }

        public void Fly()
        {
            Console.WriteLine($"{Name} is flying.");
        }
    }
}
