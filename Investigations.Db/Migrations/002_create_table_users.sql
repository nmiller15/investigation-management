CREATE TABLE IF NOT EXISTS users (
    user_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    first_name VARCHAR(30) NOT NULL,
    last_name VARCHAR(30) NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    birthdate DATE NOT NULL,
    password_hash BYTEA NOT NULL,
    role_code_key INT NOT NULL,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_key INT,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_key INT
);
