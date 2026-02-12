CREATE OR REPLACE FUNCTION add_contact(
    p_first_name VARCHAR(30),
    p_last_name VARCHAR(30),
    p_email VARCHAR(50),
    p_mobile_phone VARCHAR(15),
    p_work_phone VARCHAR(15),
    p_home_phone VARCHAR(15),
    p_preferred_contact_method_code_key INT,
    p_notes TEXT,
    p_inserted_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    INSERT INTO contacts (
        first_name,
        last_name,
        email,
        mobile_phone,
        work_phone,
        home_phone,
        preferred_contact_method_code_key,
        notes,
        inserted_datetime,
        inserted_by_user_key
    ) VALUES (
        p_first_name,
        p_last_name,
        p_email,
        p_mobile_phone,
        p_work_phone,
        p_home_phone,
        p_preferred_contact_method_code_key,
        p_notes,
        now(),
        p_inserted_by_user_key
    ) 
    RETURNING contact_key;
$$;

GRANT EXECUTE ON FUNCTION add_contact(VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, INT, TEXT, INT) TO app_user;
