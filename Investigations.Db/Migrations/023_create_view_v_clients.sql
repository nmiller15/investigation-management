CREATE VIEW v_clients AS 
    SELECT 
        c.client_id,
        c.client_name,
        c.primary_contact_id,
        pc.first_name AS primary_contact_first_name,
        pc.last_name AS primary_contact_last_name,
        c.inserted_datetime,
        c.inserted_by_user_id,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        c.updated_datetime,
        c.updated_by_user_id,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name
    FROM clients AS c
    LEFT JOIN contacts AS pc ON c.primary_contact_id = pc.contact_id
    LEFT JOIN users AS iu ON c.inserted_by_user_id = iu.user_id
    LEFT JOIN users AS uu ON c.updated_by_user_id = uu.user_id;
