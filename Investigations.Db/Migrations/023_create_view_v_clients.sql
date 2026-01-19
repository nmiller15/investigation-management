CREATE VIEW v_clients AS 
    SELECT 
        c.client_key,
        c.client_name,
        c.primary_contact_key,
        pc.first_name AS primary_contact_first_name,
        pc.last_name AS primary_contact_last_name,
        c.inserted_datetime,
        c.inserted_by_user_key,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        c.updated_datetime,
        c.updated_by_user_key,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name
    FROM clients AS c
    LEFT JOIN contacts AS pc ON c.primary_contact_key = pc.contact_key
    LEFT JOIN users AS iu ON c.inserted_by_user_key = iu.user_key
    LEFT JOIN users AS uu ON c.updated_by_user_key = iu.user_key;
