CREATE OR REPLACE FUNCTION update_client(
    p_client_key INT,
    p_client_name VARCHAR,
    p_primary_contact_key INT,
    p_updated_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
     UPDATE clients 
     SET
        client_name = p_client_name,
        primary_contact_key = p_primary_contact_key,
        updated_by_user_key = p_updated_by_user_key,
        updated_datetime = now()
    WHERE client_key =  p_client_key
    RETURNING client_key;
$$;

GRANT EXECUTE ON FUNCTION update_client(INT, VARCHAR, INT, INT) TO app_user;
