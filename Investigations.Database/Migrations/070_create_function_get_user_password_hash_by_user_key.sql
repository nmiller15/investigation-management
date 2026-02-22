
CREATE OR REPLACE FUNCTION get_user_password_hash_by_user_key(
    p_user_key INT
)
RETURNS VARCHAR(256)
LANGUAGE sql
STABLE
AS $$
    SELECT 
        password_hash
    FROM users AS u
    WHERE 
        u.user_key = p_user_key;
$$;

GRANT EXECUTE ON FUNCTION get_user_password_hash_by_user_key(INT) TO app_user;
