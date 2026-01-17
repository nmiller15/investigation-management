CREATE VIEW v_users AS 
    SELECT
        u.user_id,
        u.user_no,
        u.first_name,
        u.last_name,
        u.email,
        u.birthdate,
        u.role_code_key,
        r.description AS role_description,
        u.inserted_by_user_id,
        iuc.first_name AS inserted_by_user_first_name,
        iuc.last_name AS inserted_by_user_last_name,
        u.inserted_datetime,
        u.updated_by_user_id,
        uuc.first_name AS updated_by_user_first_name,
        uuc.last_name AS updated_by_user_last_name,
        u.updated_datetime
    FROM users AS u
    LEFT JOIN codes AS r ON codes.code_key = u.role_code_key
    LEFT JOIN users AS iuc ON iuc.user_id = u.inserted_by_user_id
    LEFT JOIN users AS uuc ON uuc.user_id = u.updated_by_user_id;
