CREATE TABLE IF NOT EXISTS clients (
    client_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    client_name VARCHAR(100) NOT NULL,
    primary_contact_key INT,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
