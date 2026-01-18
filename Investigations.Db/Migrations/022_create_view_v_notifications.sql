CREATE VIEW v_notifications AS
    SELECT
        n.notification_id,
        n.subject,
        n.description,
        n.assigned_to_user_id,
        au.first_name AS assigned_to_user_first_name,
        au.last_name AS assigned_to_user_last_name,
        n.is_delayed,
        n.delayed_until_datetime,
        n.inserted_by_user_id,
        iu.first_name AS inserted_by_user_first_name,
        iu.last_name AS inserted_by_user_last_name,
        n.inserted_datetime,
        n.updated_by_user_id,
        uu.first_name AS updated_by_user_first_name,
        uu.last_name AS updated_by_user_last_name,
        n.updated_datetime
    FROM notifications AS n
    LEFT JOIN users AS au ON n.assigned_to_user_id = au.user_id
    LEFT JOIN users AS iu ON n.inserted_by_user_id = iu.user_id
    LEFT JOIN users AS uu ON n.updated_by_user_id = uu.user_id;
