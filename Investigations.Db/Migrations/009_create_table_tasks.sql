CREATE TABLE IF NOT EXISTS tasks (
    task_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    task_name VARCHAR(100) NOT NULL,
    task_description TEXT, 
    case_key INT,
    assigned_to_user_key INT,
    reminder_date TIMESTAMPTZ,
    due_date TIMESTAMPTZ,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
