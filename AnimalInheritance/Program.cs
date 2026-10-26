namespace AnimalInheritance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Inheritance: Animal =====\n");

            Dog dog = new Dog("Tommy", 3, "German Shepherd");
            Cat cat = new Cat("Kitty", 2, true);
            Bird bird = new Bird("Tweety", 1, 25.5);

            // Each child uses the members it inherited from Animal plus its own.
            Console.WriteLine("--- Dog ---");
            dog.ShowInfo();
            dog.MakeSound();
            dog.Eat();      // inherited from Animal
            dog.Fetch();    // only in Dog

            Console.WriteLine("\n--- Cat ---");
            cat.ShowInfo();
            cat.MakeSound();
            cat.Eat();
            cat.Climb();

            Console.WriteLine("\n--- Bird ---");
            bird.ShowInfo();
            bird.MakeSound();
            bird.Eat();
            bird.Fly();

            // Polymorphism: an Animal variable can hold any child object,
            // and the overridden MakeSound() of the real object is called.
            Console.WriteLine("\n--- All animals in one list ---");
            List<Animal> animals = new List<Animal> { dog, cat, bird, new Animal("Generic", 5) };
            foreach (Animal animal in animals)
            {
                animal.MakeSound();
            }
        }
    }
}
