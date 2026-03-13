using Bogus;
using Investigations.Models;
using Npgsql;

namespace Investigations.Tests.Integration.Utilities;

public static class ClientSeeder
{
    public static Client SeedSingleClient(TestFixture fixture)
    {
        var sampleContact = new Faker<Contact>()
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => new EmailAddress(f.Internet.Email()))
            .Generate();

        var sampleClient = new Faker<Client>()
            .RuleFor(c => c.ClientName, f => f.Company.CompanyName())
            .Generate();

        using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO contacts (first_name, last_name, email) VALUES (@contact_first_name, @contact_last_name, @contact_email) RETURNING contact_key;", connection))
            {
                cmd.Parameters.AddWithValue("contact_first_name", sampleContact.FirstName);
                cmd.Parameters.AddWithValue("contact_last_name", sampleContact.LastName);
                cmd.Parameters.AddWithValue("contact_email", sampleContact.Email?.Address!);

                sampleContact.ContactKey = (int)(cmd.ExecuteScalar() ?? 0);
                sampleClient.PrimaryContact = sampleContact;
            }

            using (var cmd = new NpgsqlCommand("INSERT INTO clients (client_name, primary_contact_key) VALUES (@client_name, @primary_contact_key) RETURNING client_key;", connection))
            {
                cmd.Parameters.AddWithValue("client_name", sampleClient.ClientName);
                cmd.Parameters.AddWithValue("primary_contact_key", sampleClient.PrimaryContact?.ContactKey ?? 0);

                sampleClient.ClientKey = (int)(cmd.ExecuteScalar() ?? 0);
            }
        }

        return sampleClient;
    }

    public static List<Client> SeedClients(TestFixture fixture, int numberOfClients)
    {
        var clients = new List<Client>();
        for (int i = 0; i < numberOfClients; i++)
        {
            var seededClient = SeedSingleClient(fixture);
            clients.Add(seededClient);
        }
        return clients;
    }

    public static void Cleanup(TestFixture fixture)
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
        using var cmd = new NpgsqlCommand("TRUNCATE TABLE clients; TRUNCATE TABLE contacts;", connection);
        connection.Open();
        cmd.ExecuteNonQuery();
    }
}
