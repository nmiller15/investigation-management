CREATE OR REPLACE FUNCTION get_notifications_by_user_key(
    p_assigned_to_user_key INT 
)
RETURNS SETOF v_notifications
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vn.notification_key,
        vn.subject,
        vn.description,
        vn.assigned_to_user_key,
        vn.assigned_to_user_first_name,
        vn.assigned_to_user_last_name,
        vn.is_delayed,
        vn.delayed_until_datetime,
        vn.inserted_by_user_key,
        vn.inserted_by_user_first_name,
        vn.inserted_by_user_last_name,
        vn.inserted_datetime,
        vn.updated_by_user_key,
        vn.updated_by_user_first_name,
        vn.updated_by_user_last_name,
        vn.updated_datetime
    FROM v_notifications AS vn
    WHERE assigned_to_user_key = p_assigned_to_user_key;
$$;

GRANT EXECUTE ON FUNCTION get_notifications_by_user_key(INT) TO app_user;
