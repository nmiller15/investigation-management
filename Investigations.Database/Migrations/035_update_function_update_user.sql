CREATE OR REPLACE FUNCTION update_user(
    p_user_key INT,
    p_first_name VARCHAR(30),
    p_last_name VARCHAR(30),
    p_email VARCHAR(50),
    p_birthdate TIMESTAMP,
    p_role_code_key INT,
    p_updated_by_user_key INT
)
RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    UPDATE users 
    SET
        first_name = p_first_name,
        last_name = p_last_name,
        email = p_email,
        birthdate = p_birthdate,
        role_code_key = p_role_code_key,
        updated_by_user_key = p_updated_by_user_key,
        updated_datetime = CURRENT_TIMESTAMP
    RETURNING user_key;
$$;

GRANT EXECUTE ON FUNCTION update_user(INT, VARCHAR, VARCHAR, VARCHAR, DATE, INT, INT) TO app_user;
