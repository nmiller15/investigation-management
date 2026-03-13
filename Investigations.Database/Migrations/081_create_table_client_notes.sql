CREATE TABLE IF NOT EXISTS client_notes (
    client_note_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    body TEXT NOT NULL,
    client_key INT NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
