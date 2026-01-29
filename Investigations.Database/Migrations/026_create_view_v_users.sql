CREATE VIEW v_users AS 
    SELECT
        u.user_key,
        u.first_name,
        u.last_name,
        u.email,
        u.birthdate,
        u.role_code_key,
        r.code_description AS role_description,
        u.inserted_by_user_key,
        iuc.first_name AS inserted_by_user_first_name,
        iuc.last_name AS inserted_by_user_last_name,
        u.inserted_datetime,
        u.updated_by_user_key,
        uuc.first_name AS updated_by_user_first_name,
        uuc.last_name AS updated_by_user_last_name,
        u.updated_datetime
    FROM users AS u
    LEFT JOIN codes AS r ON r.code_key = u.role_code_key
    LEFT JOIN users AS iuc ON iuc.user_key = u.inserted_by_user_key
    LEFT JOIN users AS uuc ON uuc.user_key = u.updated_by_user_key;
