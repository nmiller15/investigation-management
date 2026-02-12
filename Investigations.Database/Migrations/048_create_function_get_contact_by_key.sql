CREATE OR REPLACE FUNCTION get_contact_by_key(
    p_contact_key INT
)
RETURNS SETOF v_contacts
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vc.contact_key,
        vc.first_name,
        vc.last_name,
        vc.email,
        vc.mobile_phone,
        vc.work_phone,
        vc.home_phone,
        vc.preferred_contact_method_code_key,
        vc.preferred_contact_method_code,
        vc.preferred_contact_method_description,
        vc.notes,
        vc.inserted_datetime,
        vc.inserted_by_user_key,
        vc.inserted_by_first_name,
        vc.inserted_by_last_name,
        vc.updated_datetime,
        vc.updated_by_user_key,
        vc.updated_by_first_name,
        vc.updated_by_last_name
    FROM v_contacts AS vc
    WHERE vc.contact_key = p_contact_key
    LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION get_contact_by_key(INT) TO app_user;
