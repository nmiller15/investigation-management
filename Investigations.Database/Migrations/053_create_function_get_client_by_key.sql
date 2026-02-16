CREATE OR REPLACE FUNCTION get_client_by_key(
    p_client_key INT
)
RETURNS SETOF v_clients
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vc.client_key,
        vc.client_name,
        vc.primary_contact_key,
        vc.primary_contact_first_name,
        vc.primary_contact_last_name,
        vc.inserted_datetime,
        vc.inserted_by_user_key,
        vc.inserted_by_first_name,
        vc.inserted_by_last_name,
        vc.updated_datetime,
        vc.updated_by_user_key,
        vc.updated_by_first_name,
        vc.updated_by_last_name
    FROM v_clients AS vc
    WHERE vc.client_key = p_client_key
    LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION get_client_by_key(INT) TO app_user;
