CREATE OR REPLACE FUNCTION delete_case_by_key(
    p_case_key INT
)
RETURNS VOID
LANGUAGE sql
VOLATILE
AS $$
    DELETE FROM cases
    WHERE case_key = p_case_key;
$$;

GRANT EXECUTE ON FUNCTION delete_case_by_key(INT) TO app_user;
