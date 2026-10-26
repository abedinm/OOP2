namespace AnimalInheritance
{
    // Base class: everything every animal has in common.
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // virtual = child classes are allowed to replace (override) this method.
        public virtual void MakeSound()
        {
            Console.WriteLine($"{Name} makes a sound.");
        }

        public void Eat()
        {
            Console.WriteLine($"{Name} is eating.");
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Name: {Name}, Age: {Age}");
        }
    }
}
