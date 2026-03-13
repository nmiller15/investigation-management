using Bogus;
using Investigations.Models;
using Npgsql;

namespace Investigations.Tests.Integration.Utilities;

public static class ContactSeeder
{
    public static Contact SeedSingleContact(TestFixture fixture)
    {
        var sampleContact = new Faker<Contact>()
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => new EmailAddress(f.Internet.Email()))
            .Generate();

        using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            using var cmd = new NpgsqlCommand("INSERT INTO contacts (first_name, last_name, email, inserted_datetime, inserted_by_user_key) "
                        + "VALUES (@first_name, @last_name, @email, CURRENT_TIMESTAMP, 100) "
                        + "RETURNING contact_key;", connection);
            cmd.Parameters.AddWithValue("first_name", sampleContact.FirstName);
            cmd.Parameters.AddWithValue("last_name", sampleContact.LastName);
            cmd.Parameters.AddWithValue("email", sampleContact.Email.Address);

            connection.Open();
            sampleContact.ContactKey = (int)(cmd.ExecuteScalar() ?? 0);
        }

        return sampleContact;
    }

    public static List<Contact> SeedContacts(TestFixture fixture, int numberOfContacts)
    {
        var contacts = new List<Contact>();
        for (int i = 0; i < numberOfContacts; i++)
        {
            var seededContact = SeedSingleContact(fixture);
            contacts.Add(seededContact);
        }
        return contacts;
    }

    public static void Cleanup(TestFixture fixture)
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
        using var cmd = new NpgsqlCommand("TRUNCATE TABLE contacts;", connection);
        connection.Open();
        cmd.ExecuteNonQuery();
    }
}
