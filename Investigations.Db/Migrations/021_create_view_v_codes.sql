CREATE VIEW v_codes AS 
    SELECT 
        c.code_key,
        c.code,
        c.code_type,
        c.code_description,
        c.code_short_description,
        c.inserted_by_user_key,
        iu.first_name AS inserted_by_user_first_name,
        iu.last_name AS inserted_by_user_last_name,
        c.inserted_datetime,
        c.updated_by_user_key,
        uu.first_name AS updated_by_user_first_name,
        uu.last_name AS updated_by_user_last_name,
        c.updated_datetime
    FROM codes AS c
    LEFT JOIN users AS iu on c.inserted_by_user_key = iu.user_key
    LEFT JOIN users AS uu on c.updated_by_user_key = iu.user_key;
