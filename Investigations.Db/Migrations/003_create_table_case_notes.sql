CREATE TABLE IF NOT EXISTS case_notes (
    case_note_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    body TEXT NOT NULL,
    case_key INT NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
)
