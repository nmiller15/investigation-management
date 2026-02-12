CREATE OR REPLACE FUNCTION delete_address_by_key(
    p_address_key INT
)
RETURNS VOID
LANGUAGE sql
VOLATILE
AS $$
    DELETE FROM addresses
    WHERE address_key = p_address_key;
$$;

GRANT EXECUTE ON FUNCTION delete_address_by_key(INT) TO app_user;
