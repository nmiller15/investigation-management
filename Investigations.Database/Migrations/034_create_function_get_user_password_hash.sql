CREATE OR REPLACE FUNCTION get_user_password_hash(
    p_email VARCHAR(100)
)
RETURNS VARCHAR(256)
LANGUAGE sql
STABLE
AS $$
    SELECT 
        password_hash
    FROM users AS u
    WHERE 
        u.email = p_email;
$$;

GRANT EXECUTE ON FUNCTION get_user_password_hash(VARCHAR) TO app_user;
