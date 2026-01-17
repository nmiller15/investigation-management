CREATE TABLE IF NOT EXISTS clients (
    client_id UUID PRIMARY KEY DEFAULT get_random_uuid(),
    client_name VARCHAR(100) NOT NULL,
    primary_contact_id UUID,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_id UUID,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_id UUID
);
