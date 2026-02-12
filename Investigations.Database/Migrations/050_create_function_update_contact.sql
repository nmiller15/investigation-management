CREATE OR REPLACE FUNCTION update_contact(
    p_contact_key INT,
    p_first_name VARCHAR(30),
    p_last_name VARCHAR(30),
    p_email VARCHAR(50),
    p_mobile_phone VARCHAR(15),
    p_work_phone VARCHAR(15),
    p_home_phone VARCHAR(15),
    p_preferred_contact_method_code_key INT,
    p_notes TEXT,
    p_updated_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    UPDATE contacts SET
        first_name = p_first_name,
        last_name = p_last_name,
        email = p_email,
        mobile_phone = p_mobile_phone,
        work_phone = p_work_phone,
        home_phone = p_home_phone,
        preferred_contact_method_code_key = p_preferred_contact_method_code_key,
        notes = p_notes,
        updated_datetime = now(),
        updated_by_user_key = p_updated_by_user_key
    WHERE contact_key = p_contact_key
    RETURNING contact_key;
$$;

GRANT EXECUTE ON FUNCTION update_contact(INT, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, INT, TEXT, INT) TO app_user;
