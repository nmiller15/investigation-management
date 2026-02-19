CREATE OR REPLACE FUNCTION delete_notification(
    p_notification_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    DELETE FROM notifications 
    WHERE notification_key = p_notification_key
    RETURNING notification_key;
$$;

GRANT EXECUTE ON FUNCTION delete_notification(INT) TO app_user;
