CREATE VIEW v_cases AS
    SELECT
        c.case_key,
        c.case_number,
        c.is_active,
        c.subject_key,
        s.first_name AS subject_first_name,
        s.last_name AS subject_last_name,
        c.client_key,
        cl.client_name,
        c.date_of_referral,
        c.case_type_code_key,
        ct.code AS case_type_code,
        ct.code_short_description AS case_type_short_description,
        ct.code_description AS case_type_description,
        c.synopsis,
        c.inserted_by_user_key,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        c.inserted_datetime,
        c.updated_by_user_key,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name,
        c.updated_datetime
    FROM cases AS c
    LEFT JOIN subjects AS s ON s.subject_key = c.subject_key
    LEFT JOIN clients AS cl ON c.client_key = cl.client_key
    LEFT JOIN codes AS ct ON c.case_type_code_key = ct.code_key
    LEFT JOIN users AS iu ON c.inserted_by_user_key = iu.user_key
    LEFT JOIN users AS uu ON c.updated_by_user_key = uu.user_key;
