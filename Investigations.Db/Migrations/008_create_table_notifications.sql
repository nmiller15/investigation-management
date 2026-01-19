CREATE TABLE IF NOT EXISTS notifications (
    notification_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    subject VARCHAR(100) NOT NULL,
    description TEXT,
    assigned_to_user_key INT,
    is_delayed BOOLEAN NOT NULL DEFAULT FALSE,
    delayed_until_datetime TIMESTAMPTZ,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
