CREATE OR REPLACE FUNCTION add_notification(
    p_subject VARCHAR(100),
    p_description TEXT,
    p_assigned_to_user_key INT,
    p_is_delayed BOOLEAN,
    p_delayed_until_datetime TIMESTAMPTZ,
    p_inserted_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    INSERT INTO notifications (
        subject,
        description,
        assigned_to_user_key,
        is_delayed,
        delayed_until_datetime,
        inserted_by_user_key,
        inserted_datetime
    ) VALUES (
        p_subject,
        p_description,
        p_assigned_to_user_key,
        p_is_delayed,
        p_delayed_until_datetime,
        p_inserted_by_user_key,
        now()
    )
    RETURNING notification_key;
$$;

GRANT EXECUTE ON FUNCTION add_notification(VARCHAR, TEXT, INT, BOOLEAN, TIMESTAMPTZ, INT) TO app_user;
