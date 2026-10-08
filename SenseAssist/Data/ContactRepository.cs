using SenseAssist.Models;

namespace SenseAssist.Data;

public sealed class ContactRepository : IRepository<Contact>
{
    public List<Contact> GetAll(int userId)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "SELECT Id, UserId, Name, Relation, Phone, Notes, IsEmergency FROM dbo.Contacts WHERE UserId = @user ORDER BY IsEmergency DESC, Name",
            ("@user", userId));
        using var reader = command.ExecuteReader();

        var contacts = new List<Contact>();
        while (reader.Read())
        {
            contacts.Add(new Contact
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Name = reader.GetString(2),
                Relation = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Phone = reader.GetString(4),
                Notes = reader.IsDBNull(5) ? "" : reader.GetString(5),
                IsEmergency = reader.GetBoolean(6),
            });
        }
        return contacts;
    }

    public void Add(Contact item)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "INSERT INTO dbo.Contacts (UserId, Name, Relation, Phone, Notes, IsEmergency) OUTPUT INSERTED.Id VALUES (@user, @name, @relation, @phone, @notes, @emergency)",
            ("@user", item.UserId), ("@name", item.Name), ("@relation", item.Relation), ("@phone", item.Phone), ("@notes", item.Notes), ("@emergency", item.IsEmergency));
        item.Id = (int)command.ExecuteScalar()!;
    }

    public void Update(Contact item)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "UPDATE dbo.Contacts SET Name = @name, Relation = @relation, Phone = @phone, Notes = @notes, IsEmergency = @emergency WHERE Id = @id AND UserId = @user",
            ("@name", item.Name), ("@relation", item.Relation), ("@phone", item.Phone), ("@notes", item.Notes), ("@emergency", item.IsEmergency), ("@id", item.Id), ("@user", item.UserId));
        command.ExecuteNonQuery();
    }

    public void Delete(int id, int userId)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection, "DELETE FROM dbo.Contacts WHERE Id = @id AND UserId = @user", ("@id", id), ("@user", userId));
        command.ExecuteNonQuery();
    }
}
