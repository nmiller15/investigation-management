CREATE TABLE IF NOT EXISTS addresses (
    address_id UUID PRIMARY KEY DEFAULT get_random_uuid(),
    line_one VARCHAR(100) NOT NULL,
    line_two VARCHAR(100),
    city VARCHAR(50) NOT NULL,
    state_code_key INT,
    country_code_key INT,
    zip varchar(10) NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_id UUID,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_id UUID
);
