using Investigations.Configuration;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Repositories;

public class NotificationRepository(IConnectionStrings connectionStrings)
    : BaseSqlRepository(connectionStrings)
{
    private readonly NotificationParser _notificationParser = new();
    private readonly IntParser _intParser = new();

    public async Task<Notification> GetNotification(int notificationKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_notification_by_key");
        AddNotificationKeyParameters(notificationKey, dcs);

        var notification = await NpgsqlDataProvider.ExecuteFunction(dcs, _notificationParser);
        return notification.FirstOrDefault() ?? new();
    }

    public async Task<List<Notification>> GetNotifications()
    {
        var dcs = GetFunctionCallDcsInstance("get_notifications");

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _notificationParser);
        return records ?? [];
    }

    public async Task<List<Notification>> GetNotificationsByUser(int assignedToUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_notifications_by_user_key");
        dcs.AddParameter("p_assigned_to_user_key", assignedToUserKey);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _notificationParser);
        return records ?? [];
    }

    public async Task<int> AddNotification(Notification notification, int insertedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("add_notification");
        AddNotificationParameters(notification, dcs);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);

        var notificationKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return notificationKey;
    }

    public async Task<int> UpdateNotification(Notification notification, int updatedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("update_notification");
        AddNotificationKeyParameters(notification.NotificationKey, dcs);
        AddNotificationParameters(notification, dcs);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var notificationKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return notificationKey;
    }

    public async Task<int> DeleteNotification(Notification notification)
    {
        var dcs = GetFunctionCallDcsInstance("delete_notification");
        AddNotificationKeyParameters(notification.NotificationKey, dcs);

        var notificationKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return notificationKey;
    }

    private static void AddNotificationKeyParameters(int notificationKey, DataCallSettings dcs)
    {
        dcs.AddParameter("p_notification_key", notificationKey);
    }

    private static void AddNotificationParameters(Notification notification, DataCallSettings dcs)
    {
        dcs.AddParameter("p_subject", notification.Subject);
        dcs.AddParameter("p_description", notification.Description);
        dcs.AddParameter("p_assigned_to_user", notification.AssignedToUserKey);
        dcs.AddParameter("p_is_delayed", notification.IsDelayed);
        dcs.AddParameter("p_delayed_until_datetime", notification.DelayedUntilDatetime);
    }
}
