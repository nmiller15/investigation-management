CREATE TABLE IF NOT EXISTS subjects_addresses_links (
    subject_address_link_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    subject_key INT NOT NULL,
    address_key INT NOT NULL,
    is_primary_address BOOLEAN DEFAULT FALSE
);
