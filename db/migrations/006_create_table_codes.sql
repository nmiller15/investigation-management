CREATE TABLE IF NOT EXISTS codes (
    code_key INT PRIMARY KEY DEFAULT INCREMENT, 
    code_description VARCHAR(50) NOT NULL,
    code_short_description VARCHAR(20),
    code_type VARCHAR(6) NOT NULL,
    code VARCHAR(10) NOT NULL,
    inserted_by_user_id UUID NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_by_user_id UUID,
    updated_datetime TIMESTAMPTZ
);
