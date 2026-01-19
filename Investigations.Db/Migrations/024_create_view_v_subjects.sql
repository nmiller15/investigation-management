CREATE VIEW v_subjects AS
    SELECT
        s.subject_key,
        s.first_name,
        s.last_name,
        s.marital_status_code_key,
        ms.code AS marital_status_code,
        ms.code_short_description AS marital_status_description,
        g.code_short_description AS gender,
        s.inserted_by_user_key,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        s.inserted_datetime,
        s.updated_by_user_key,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name,
        s.updated_datetime
    FROM subjects AS s
    LEFT JOIN codes AS ms ON s.marital_status_code_key = ms.code_key
    LEFT JOIN codes AS g ON s.gender_code_key = g.code_key
    LEFT JOIN users AS iu ON s.inserted_by_user_key = iu.user_key
    LEFT JOIN users AS uu ON s.updated_by_user_key = iu.user_key;
