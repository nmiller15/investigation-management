CREATE OR REPLACE FUNCTION update_task(
    p_task_key INT,
    p_task_name VARCHAR(100),
    p_task_description TEXT,
    p_case_key INT,
    p_assigned_to_user_key INT,
    p_reminder_date TIMESTAMPTZ,
    p_due_date TIMESTAMPTZ,
    p_updated_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    UPDATE tasks 
    SET
        task_name = p_task_name,
        task_description = p_task_description,
        case_key = p_case_key,
        assigned_to_user_key = p_assigned_to_user_key,
        reminder_date = p_reminder_date,
        due_date = p_due_date,
        updated_datetime = now(),
        updated_by_user_key = p_updated_by_user_key
    WHERE task_key = p_task_key
    RETURNING task_key;
$$;

GRANT EXECUTE ON FUNCTION update_task(INT, VARCHAR, TEXT, INT, INT, TIMESTAMPTZ, TIMESTAMPTZ, INT) TO app_user;
