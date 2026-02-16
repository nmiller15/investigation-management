CREATE OR REPLACE FUNCTION add_task(
    p_task_name VARCHAR(100),
    p_task_description TEXT,
    p_case_key INT,
    p_assigned_to_user_key INT,
    p_reminder_date TIMESTAMPTZ,
    p_due_date TIMESTAMPTZ,
    p_inserted_by_user_key INT
)
RETURNS INT
LANGUAGE sql
STABLE
AS $$
    INSERT INTO tasks (
        task_name,
        task_description,
        case_key,
        assigned_to_user_key,
        reminder_date,
        due_date,
        inserted_datetime,
        inserted_by_user_key
    ) VALUES (
        p_task_name,
        p_task_description,
        p_case_key,
        p_assigned_to_user_key,
        p_reminder_date,
        p_due_date,
        now(),
        p_inserted_by_user_key
    )
    RETURNING task_key;
$$;

GRANT EXECUTE ON FUNCTION add_task(VARCHAR, TEXT, INT, INT, TIMESTAMPTZ, TIMESTAMPTZ, INT) TO app_user;
