CREATE VIEW v_contact_notes AS
    SELECT
        cn.contact_note_key,
        cn.body,
        cn.contact_key,
        cn.inserted_datetime,
        cn.inserted_by_user_key,
        ic.first_name AS inserted_by_user_first_name,
        ic.last_name AS inserted_by_user_last_name,
        cn.updated_datetime,
        cn.updated_by_user_key,
        uc.first_name AS updated_by_user_first_name,
        uc.last_name AS updated_by_user_last_name
    FROM contact_notes AS cn
    LEFT JOIN contacts AS c ON cn.contact_key = c.contact_key
    LEFT JOIN users AS ic ON ic.user_key = cn.inserted_by_user_key
    LEFT JOIN users AS uc ON uc.user_key = cn.updated_by_user_key;
