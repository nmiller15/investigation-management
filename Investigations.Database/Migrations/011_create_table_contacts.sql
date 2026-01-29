CREATE TABLE IF NOT EXISTS contacts (
    contact_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    first_name VARCHAR(30) NOT NULL,
    last_name VARCHAR(30) NOT NULL,
    email VARCHAR(50) NOT NULL,
    mobile_phone VARCHAR(15),
    work_phone VARCHAR(15),
    home_phone VARCHAR(15),
    preferred_contact_method_code_key INT,
    notes TEXT,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
