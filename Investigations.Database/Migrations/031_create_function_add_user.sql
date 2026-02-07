CREATE OR REPLACE FUNCTION add_user(
    p_email VARCHAR(100),
    p_password_hash VARCHAR(256),
    p_inserted_by_user_key INT
)
RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    INSERT INTO users (
        first_name,
        last_name,
        email,
        password_hash,
        role_code_key,
        inserted_by_user_key,
        inserted_datetime
    ) VALUES (
        '',
        '',
        p_email,
        p_password_hash,
        168, -- default to 'Investigator' role
        p_inserted_by_user_key,
        CURRENT_TIMESTAMP
    )
    RETURNING user_key;
$$;

GRANT EXECUTE ON FUNCTION add_user(VARCHAR, VARCHAR, INT) TO app_user;
