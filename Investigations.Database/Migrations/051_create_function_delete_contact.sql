CREATE OR REPLACE FUNCTION delete_contact_by_key(
    p_contact_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    DELETE FROM contacts
    WHERE contact_key = p_contact_key
    RETURNING contact_key;
$$;

GRANT EXECUTE ON FUNCTION delete_contact_by_key(INT) TO app_user;
