CREATE TABLE IF NOT EXISTS cases (
    case_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    case_number VARCHAR(15) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    subject_key INT,
    client_key INT,
    date_of_referral TIMESTAMPTZ NOT NULL,
    case_type_code_key INT,
    synopsis TEXT NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
