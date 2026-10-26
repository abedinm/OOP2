namespace AnimalInheritance
{
    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public Cat(string name, int age, bool isIndoor) : base(name, age)
        {
            IsIndoor = isIndoor;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Indoor cat: {(IsIndoor ? "Yes" : "No")}");
        }

        public void Climb()
        {
            Console.WriteLine($"{Name} is climbing a tree.");
        }
    }
}
