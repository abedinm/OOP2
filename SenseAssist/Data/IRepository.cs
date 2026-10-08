using SenseAssist.Models;

namespace SenseAssist.Data;

/// <summary>Add / read / save / delete for one kind of user-owned item.</summary>
public interface IRepository<T> where T : Entity
{
    List<T> GetAll(int userId);
    void Add(T item);
    void Update(T item);
    void Delete(int id, int userId);
}
