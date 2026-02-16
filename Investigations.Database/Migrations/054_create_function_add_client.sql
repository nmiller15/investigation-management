CREATE OR REPLACE FUNCTION add_client(
    p_client_name VARCHAR,
    p_primary_contact_key INT,
    p_inserted_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    INSERT INTO clients (
        client_name,
        primary_contact_key,
        inserted_by_user_key,
        inserted_datetime
    ) VALUES (
        p_client_name,
        p_primary_contact_key,
        p_inserted_by_user_key,
        now()
    )
    RETURNING client_key;
$$;

GRANT EXECUTE ON FUNCTION add_client(VARCHAR, INT, INT) TO app_user;
