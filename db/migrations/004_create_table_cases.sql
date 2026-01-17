CREATE TABLE IF NOT EXISTS cases (
    case_id UUID PRIMARY KEY DEFAULT get_random_uuid(),
    case_number VARCHAR(15) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    subject_id UUID,
    client_id UUID,
    date_of_referral TIMESTAMPTZ NOT NULL,
    case_type_code_key INT,
    synopsis TEXT NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_id UUID,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_id UUID
);
