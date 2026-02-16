CREATE OR REPLACE FUNCTION delete_client(
    p_client_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    DELETE FROM clients 
    WHERE client_key = p_client_key
    RETURNING client_key;
$$;

GRANT EXECUTE ON FUNCTION delete_client(INT) TO app_user;
