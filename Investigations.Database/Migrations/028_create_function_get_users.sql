CREATE OR REPLACE FUNCTION get_users()
RETURNS SETOF v_users
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vu.user_key,
        vu.first_name,
        vu.last_name,
        vu.email,
        vu.birthdate,
        vu.role_code_key,
        vu.role_description,
        vu.inserted_by_user_key,
        vu.inserted_by_user_first_name,
        vu.inserted_by_user_last_name,
        vu.inserted_datetime,
        vu.updated_by_user_key,
        vu.updated_by_user_first_name,
        vu.updated_by_user_last_name,
        vu.updated_datetime
    FROM v_users AS vu;
$$;
