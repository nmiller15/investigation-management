CREATE VIEW v_contacts AS
    SELECT
        c.contact_key,
        c.first_name,
        c.last_name,
        c.email,
        c.mobile_phone,
        c.work_phone,
        c.home_phone,
        c.preferred_contact_method_code_key,
        pcm.code AS preferred_contact_method_code,
        pcm.code_short_description AS preferred_contact_method_description,
        c.notes,
        c.inserted_datetime,
        c.inserted_by_user_key,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        c.updated_datetime,
        c.updated_by_user_key,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name
    FROM contacts AS c
    LEFT JOIN codes AS pcm ON c.preferred_contact_method_code_key = pcm.code_key
    LEFT JOIN users AS iu ON c.inserted_by_user_key = iu.user_key
    LEFT JOIN users AS uu ON c.updated_by_user_key = iu.user_key;
