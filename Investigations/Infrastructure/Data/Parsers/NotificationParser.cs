using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class NotificationParser : BaseAuditModelParser<Notification>
{
    public override Notification Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.NotificationKey = reader.ParseInt32("notification_key");
        Model.Subject = reader.ParseString("subject");
        Model.Description = reader.ParseString("description");
        Model.AssignedToUserKey = reader.ParseInt32("assigned_to_user_key");
        Model.AssignedToUserFirstName = reader.ParseString("assigned_to_user_first_name");
        Model.AssignedToUserLastName = reader.ParseString("assigned_to_user_last_name");
        Model.IsDelayed = reader.ParseBool("is_delayed");
        Model.DelayedUntilDatetime = reader.ParseDateTime("delayed_until_datetime");

        return Model;
    }
}
