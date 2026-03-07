using Investigations.Features.Tasks;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Tests.Integration.Utilities;
using Npgsql;
using Xunit.Abstractions;

namespace Investigations.Tests.Integration.Tasks;

public class CreateTaskTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly CreateTask.Handler _handler;
    private readonly ITestOutputHelper _output;

    public CreateTaskTests(TestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _handler = _fixture.Services.GetRequiredService<CreateTask.Handler>();
        _output = output;
    }

    [Fact]
    public async Task CreateTask_ReturnsAssignableUsersAndCases()
    {
        await UserSeeder.SeedUsers(_fixture, 3);
        await CaseSeeder.SeedCases(_fixture, 3);

        var query = new CreateTask.Query
        {
            CanAssignToOtherUsers = true,
            CanAssignToAnyCase = true,
            CurrentUserKey = 1,
            SourcedCaseKey = null
        };

        var response = await _handler.Handle(query);

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Assignable Users Count: {0}", response.Payload?.AssignableUsers.Count);
        _output.WriteLine("Assignable Cases Count: {0}", response.Payload?.AssignableCases.Count);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.NotEmpty(response.Payload.AssignableUsers);
        Assert.NotEmpty(response.Payload.AssignableCases);

        await UserSeeder.CleanUp(_fixture);
        await CaseSeeder.CleanUp(_fixture);
    }

    [Fact]
    public async Task CreateTask_OnlyReturnsCurrentUser_IfCannotAssignToOtherUsers()
    {
        var users = await UserSeeder.SeedUsers(_fixture, 3);
        await CaseSeeder.SeedCases(_fixture, 3);

        var currentUser = users.First();

        var response = await _handler.Handle(new CreateTask.Query
        {
            CanAssignToOtherUsers = false,
            CanAssignToAnyCase = true,
            CurrentUserKey = currentUser.UserKey,
            SourcedCaseKey = null,
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Assignable Users Count: {0}", response.Payload?.AssignableUsers.Count);

        Assert.True(response.WasSuccessful);
        Assert.NotNull(response.Payload);
        Assert.Single(response.Payload.AssignableUsers);

        await UserSeeder.CleanUp(_fixture);
        await CaseSeeder.CleanUp(_fixture);
    }

    [Fact]
    public async Task CreateTask_FailsIfCannotAssignToAnyCaseAndNoSourcedCaseProvided()
    {
        var users = await UserSeeder.SeedUsers(_fixture, 3);
        await CaseSeeder.SeedCases(_fixture, 3);

        var currentUser = users.First();

        var response = await _handler.Handle(new CreateTask.Query
        {
            CanAssignToOtherUsers = false,
            CanAssignToAnyCase = false,
            CurrentUserKey = currentUser.UserKey,
            SourcedCaseKey = null,
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);

        Assert.False(response.WasSuccessful);
        Assert.NotEmpty(response.Message);

        await UserSeeder.CleanUp(_fixture);
        await CaseSeeder.CleanUp(_fixture);
    }

    [Fact]
    public async Task CreateTask_ReturnsSourcedCaseIfCanAssignToAnyCaseFalse()
    {
        var users = await UserSeeder.SeedUsers(_fixture, 3);
        var cases = await CaseSeeder.SeedCases(_fixture, 3);

        var currentUser = users.First();
        var sourcedCase = cases.First();

        var response = await _handler.Handle(new CreateTask.Query
        {
            CanAssignToOtherUsers = false,
            CanAssignToAnyCase = false,
            CurrentUserKey = currentUser.UserKey,
            SourcedCaseKey = sourcedCase.CaseKey,
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);
        _output.WriteLine("Assignable Cases Count: {0}", response.Payload?.AssignableCases.Count);
        _output.WriteLine("Assignable Case Key: {0}", response.Payload?.AssignableCases.FirstOrDefault()?.CaseKey);

        Assert.True(response.WasSuccessful);
        Assert.NotEmpty(response.Payload.AssignableCases);
        Assert.Equal(sourcedCase.CaseKey, response.Payload.AssignableCases.First().CaseKey);

        await UserSeeder.CleanUp(_fixture);
        await CaseSeeder.CleanUp(_fixture);
    }

    [Fact]
    public async Task CreateTask_AssignsTaskToCaseAndUserSpecified()
    {
        var users = await UserSeeder.SeedUsers(_fixture, 3);
        var cases = await CaseSeeder.SeedCases(_fixture, 3);

        var currentUser = users.First();
        var assignedUser = users.Last();
        var sourcedCase = cases.First();

        var task = new AppTask
        {
            TaskName = "Test Task",
            TaskDescription = "This is a test task.",
            AssignedToUser = assignedUser,
            AssignedByUser = currentUser
        };

        var response = await _handler.Handle(new CreateTask.Command
        {
            CaseKey = sourcedCase.CaseKey,
            TaskName = task.TaskName,
            TaskDescription = task.TaskDescription,
            AssignedToUserKey = task.AssignedToUser.UserKey,
            InsertedByUserKey = task.AssignedByUser.UserKey
        });

        _output.WriteLine("Response.WasSuccessful: {0}", response.WasSuccessful);
        _output.WriteLine("Response.Message: {0}", response.Message);

        Assert.True(response.WasSuccessful);

        await using (var connection = new NpgsqlConnection(_fixture.ConnectionString))
        {
            await connection.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT task_name, assigned_to_user_key, case_key FROM tasks WHERE assigned_to_user_key = @AssignedToUserKey", connection))
            {
                cmd.Parameters.AddWithValue("AssignedToUserKey", task.AssignedToUser.UserKey);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Assert.True(await reader.ReadAsync());
                    var dbAssignedUserKey = reader.ParseInt32("assigned_to_user_key");
                    var dbCaseKey = reader.ParseInt32("case_key");
                    Assert.Equal(assignedUser.UserKey, dbAssignedUserKey);
                    Assert.Equal(sourcedCase.CaseKey, dbCaseKey);
                }
            }
        }

        await UserSeeder.CleanUp(_fixture);
        await CaseSeeder.CleanUp(_fixture);
    }
}
