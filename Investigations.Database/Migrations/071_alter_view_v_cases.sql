ALTER VIEW v_cases RENAME COLUMN inserted_by_first_name TO inserted_by_user_first_name;
ALTER VIEW v_cases RENAME COLUMN inserted_by_last_name TO inserted_by_user_last_name;

ALTER VIEW v_cases RENAME COLUMN updated_by_first_name TO updated_by_user_first_name;
ALTER VIEW v_cases RENAME COLUMN updated_by_last_name TO updated_by_user_last_name;
