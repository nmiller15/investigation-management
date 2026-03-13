DROP FUNCTION IF EXISTS get_contact_by_contact_key(INT);
DROP FUNCTION IF EXISTS get_contacts(INT);
DROP FUNCTION IF EXISTS add_contact(VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, INT, TEXT, INT);
DROP FUNCTION IF EXISTS update_contact(INT, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, INT, TEXT, INT);

DROP VIEW v_contacts;

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

ALTER TABLE contacts
DROP COLUMN notes;
