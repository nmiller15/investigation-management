CREATE VIEW v_client_notes AS
    SELECT
        cn.client_note_key,
        cn.body,
        cn.client_key,
        cn.inserted_datetime,
        cn.inserted_by_user_key,
        ic.first_name AS inserted_by_user_first_name,
        ic.last_name AS inserted_by_user_last_name,
        cn.updated_datetime,
        cn.updated_by_user_key,
        uc.first_name AS updated_by_user_first_name,
        uc.last_name AS updated_by_user_last_name
    FROM client_notes AS cn
    LEFT JOIN clients AS c ON cn.client_key = c.client_key
    LEFT JOIN users AS ic ON ic.user_key = cn.inserted_by_user_key
    LEFT JOIN users AS uc ON uc.user_key = cn.updated_by_user_key;
