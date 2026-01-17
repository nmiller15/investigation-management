CREATE VIEW v_tasks AS
    SELECT
        t.task_id,
        t.task_name,
        t.task_description,
        t.case_id,
        vc.case_number,
        vc.case_type_short_description,
        vc.case_type_description,
        vc.subject_first_name,
        vc.subject_last_name,
        t.assigned_to_user_id,
        u.first_name AS assigned_to_first_name,
        u.last_name AS assigned_to_last_name,
        t.reminder_date,
        t.due_date,
        t.inserted_by_user_id,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        t.inserted_datetime,
        t.updated_by_user_id,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name,
        t.updated_datetime
    FROM tasks AS t
    LEFT JOIN v_cases AS vc ON t.case_id = vc.case_id
    LEFT JOIN users AS u ON u.user_id = t.assigned_to_user_id
    LEFT JOIN users AS iu ON iu.user_id = t.inserted_by_user_id
    LEFT JOIN users AS uu ON uu.user_id = t.updated_by_user_id;
