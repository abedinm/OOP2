namespace SenseAssist.Models;

/// <summary>Base class for everything a user owns and can add, edit or delete.</summary>
public abstract class Entity
{
    public int Id { get; set; }
    public int UserId { get; set; }

    /// <summary>A sentence describing the item, used when it is read aloud.</summary>
    public abstract string Describe();
}
