CREATE OR REPLACE FUNCTION update_notification(
    p_notification_key INT,
    p_subject VARCHAR(100),
    p_description TEXT,
    p_assigned_to_user_key INT,
    p_is_delayed BOOLEAN,
    p_delayed_until_datetime TIMESTAMPTZ,
    p_updated_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    UPDATE notifications 
    SET
        subject = p_subject,
        description = p_description,
        assigned_to_user_key = p_assigned_to_user_key,
        is_delayed = p_is_delayed,
        delayed_until_datetime = p_delayed_until_datetime,
        updated_by_user_key = p_updated_by_user_key,
        updated_datetime = now()
    WHERE notification_key = p_notification_key
    RETURNING notification_key;
$$;

GRANT EXECUTE ON FUNCTION update_notification(INT, VARCHAR, TEXT, INT, BOOLEAN, TIMESTAMPTZ, INT) TO app_user;
