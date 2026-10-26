namespace AnimalInheritance
{
    // Dog inherits from Animal, so it gets Name, Age, Eat() and ShowInfo() for free.
    public class Dog : Animal
    {
        public string Breed { get; set; }

        // : base(name, age) passes the shared values up to the Animal constructor.
        public Dog(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof! Woof!");
        }

        public override void ShowInfo()
        {
            base.ShowInfo(); // reuse the parent's version, then add more
            Console.WriteLine($"Breed: {Breed}");
        }

        // A method only dogs have.
        public void Fetch()
        {
            Console.WriteLine($"{Name} is fetching the ball.");
        }
    }
}
