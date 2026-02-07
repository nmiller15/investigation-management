CREATE OR REPLACE FUNCTION update_user_password(
    p_user_key INT,
    p_password_hash VARCHAR(256),
    p_updated_by_user_key INT
)
RETURNS VOID
LANGUAGE sql
VOLATILE
AS $$
    UPDATE users
    SET 
        password_hash = p_password_hash,
        updated_by_user_key = p_updated_by_user_key,
        updated_datetime = CURRENT_TIMESTAMP
    WHERE user_key = p_user_key;
$$;

GRANT EXECUTE ON FUNCTION update_user_password(INT, VARCHAR, INT) TO app_user;
