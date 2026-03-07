using Bogus;
using Investigations.Models;
using Npgsql;

namespace Investigations.Tests.Integration.Utilities;

public static class CaseSeeder
{
    public async static Task<List<Case>> SeedCases(TestFixture fixture, int numberOfCases)
    {
        var cases = new List<Case>();
        for (int i = 0; i < numberOfCases; i++)
        {
            var seededCase = await SeedSingleCase(fixture);
            cases.Add(seededCase);
        }
        return cases;
    }

    public async static Task<Case> SeedSingleCase(TestFixture fixture)
    {
        var sampleSubject = new Faker<Subject>()
            .RuleFor(s => s.FirstName, f => f.Name.FirstName())
            .RuleFor(s => s.LastName, f => f.Name.LastName())
            .RuleFor(s => s.Gender, f => f.Random.Enum<Subject.Genders>())
            .RuleFor(s => s.MaritalStatus, f => f.Random.Enum<Subject.MaritalStatuses>())
            .Generate();

        var sampleContact = new Faker<Contact>()
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.MobilePhone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.WorkPhone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.HomePhone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Notes, f => f.Lorem.Sentence())
            .Generate();

        var sampleClient = new Faker<Client>()
            .RuleFor(c => c.ClientName, f => f.Company.CompanyName())
            .Generate();

        var sampleCase = new Faker<Case>()
            .RuleFor(c => c.CaseNumber, f => f.Random.Replace("CASE-##-####"))
            .RuleFor(c => c.DateOfReferral, f => f.Date.Past(1))
            .RuleFor(c => c.Type, f => f.Random.Enum<Case.Types>())
            .Generate();

        await using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            connection.Open();

            await using (var cmd = new NpgsqlCommand("INSERT INTO subjects (first_name, last_name, marital_status_code_key, gender_code_key, inserted_datetime, inserted_by_user_key) "
                        + "VALUES (@subject_first_name, @subject_last_name, @subject_marital_status_code_key, @subject_gender_code_key, CURRENT_TIMESTAMP, 100) "
                        + "RETURNING subject_key;", connection))
            {
                cmd.Parameters.AddWithValue("subject_first_name", sampleSubject.FirstName);
                cmd.Parameters.AddWithValue("subject_last_name", sampleSubject.LastName);
                cmd.Parameters.AddWithValue("subject_marital_status_code_key", (int)sampleSubject.MaritalStatus);
                cmd.Parameters.AddWithValue("subject_gender_code_key", (int)sampleSubject.Gender);
                sampleSubject.SubjectKey = (int)(await cmd.ExecuteScalarAsync() ?? 0);
                sampleCase.Subject = sampleSubject;
            }

            await using (var cmd = new NpgsqlCommand("INSERT INTO contacts (first_name, last_name, email, mobile_phone, work_phone, home_phone, notes, inserted_datetime, inserted_by_user_key) "
                        + "VALUES (@contact_first_name, @contact_last_name, @contact_email, @contact_mobile_phone, @contact_work_phone, @contact_home_phone, @contact_notes, CURRENT_TIMESTAMP, 100) "
                        + "RETURNING contact_key;", connection))
            {
                cmd.Parameters.AddWithValue("contact_first_name", sampleContact.FirstName);
                cmd.Parameters.AddWithValue("contact_last_name", sampleContact.LastName);
                cmd.Parameters.AddWithValue("contact_email", sampleContact.Email);
                cmd.Parameters.AddWithValue("contact_mobile_phone", sampleContact.MobilePhone);
                cmd.Parameters.AddWithValue("contact_work_phone", sampleContact.WorkPhone);
                cmd.Parameters.AddWithValue("contact_home_phone", sampleContact.HomePhone);
                cmd.Parameters.AddWithValue("contact_notes", sampleContact.Notes);
                sampleContact.ContactKey = (int)(await cmd.ExecuteScalarAsync() ?? 0);
                sampleClient.PrimaryContact = sampleContact;
            }

            await using (var cmd = new NpgsqlCommand("INSERT INTO clients (client_name, primary_contact_key, inserted_datetime, inserted_by_user_key) "
                        + "VALUES (@client_name, @primary_contact_key, CURRENT_TIMESTAMP, 100) "
                        + "RETURNING client_key;", connection))
            {
                cmd.Parameters.AddWithValue("client_name", sampleClient.ClientName);
                cmd.Parameters.AddWithValue("primary_contact_key", sampleContact.ContactKey);
                sampleClient.ClientKey = (int)(await cmd.ExecuteScalarAsync() ?? 0);
            }

            await using (var cmd = new NpgsqlCommand("INSERT INTO cases (case_number, date_of_referral, case_type_code_key, synopsis, subject_key, client_key, inserted_datetime, inserted_by_user_key) "
                        + "VALUES (@case_number, @date_of_referral, @type_code_key, @synopsis, @subject_key, @client_key, CURRENT_TIMESTAMP, 100) "
                        + "RETURNING case_key;", connection))
            {
                cmd.Parameters.AddWithValue("case_number", sampleCase.CaseNumber);
                cmd.Parameters.AddWithValue("date_of_referral", sampleCase.DateOfReferral.GetValueOrDefault());
                cmd.Parameters.AddWithValue("type_code_key", (int)sampleCase.Type);
                cmd.Parameters.AddWithValue("synopsis", sampleCase.Synopsis);
                cmd.Parameters.AddWithValue("subject_key", sampleSubject.SubjectKey);
                cmd.Parameters.AddWithValue("client_key", sampleClient.ClientKey);
                sampleCase.CaseKey = (int)(await cmd.ExecuteScalarAsync() ?? 0);
            }

            return sampleCase;
        }
    }

    public async static Task CleanUp(TestFixture fixture)
    {
        await using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            connection.Open();

            await using (var cmd = new NpgsqlCommand("TRUNCATE TABLE clients;" +
                        "TRUNCATE TABLE contacts;" +
                        "TRUNCATE TABLE subjects;" +
                        "TRUNCATE TABLE cases;", connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
