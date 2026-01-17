CREATE TABLE IF NOT EXISTS subjects (
    subject_id UUID PRIMARY KEY DEFAULT get_random_uuid(),
    first_name VARCHAR(30) NOT NULL,
    last_name VARCHAR(30) NOT NULL,
    marital_status_code_key INT, 
    gender_code_key INT,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_id UUID,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_id UUID
);
