CREATE OR REPLACE FUNCTION get_tasks_by_case_key(
    p_case_key INT
)
RETURNS SETOF v_tasks
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vt.task_key,
        vt.task_name,
        vt.task_description,
        vt.case_key,
        vt.case_number,
        vt.case_type_short_description,
        vt.case_type_description,
        vt.subject_first_name,
        vt.subject_last_name,
        vt.assigned_to_user_key,
        vt.assigned_to_first_name,
        vt.assigned_to_last_name,
        vt.reminder_date,
        vt.due_date,
        vt.inserted_by_user_key,
        vt.inserted_by_first_name,
        vt.inserted_by_last_name,
        vt.inserted_datetime,
        vt.updated_by_user_key,
        vt.updated_by_first_name,
        vt.updated_by_last_name,
        vt.updated_datetime
    FROM v_tasks AS vt
    WHERE case_key = p_case_key ;
$$;

GRANT EXECUTE ON FUNCTION get_tasks_by_case_key(INT) TO app_user;
