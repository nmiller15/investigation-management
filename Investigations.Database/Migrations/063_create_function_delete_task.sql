CREATE OR REPLACE FUNCTION delete_task(
    p_task_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    DELETE FROM tasks 
    WHERE task_key = p_task_key
    RETURNING task_key;
$$;

GRANT EXECUTE ON FUNCTION delete_task(INT) TO app_user;
