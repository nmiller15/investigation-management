CREATE VIEW v_contacts AS
    SELECT
        c.contact_id,
        c.first_name,
        c.last_name,
        c.email,
        c.mobile_phone,
        c.work_phone,
        c.home_phone,
        c.notes,
        c.inserted_datetime,
        c.inserted_by_user_id,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        c.updated_datetime,
        c.updated_by_user_id,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name
    FROM contacts AS c
    LEFT JOIN users AS iu ON c.inserted_by_user_id = iu.user_id
    LEFT JOIN users AS uu ON c.updated_by_user_id = uu.user_id;
