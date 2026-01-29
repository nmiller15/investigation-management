CREATE TABLE IF NOT EXISTS contacts_addresses_links (
    contact_address_link_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    contact_key INT NOT NULL,
    address_key INT NOT NULL,
    is_primary_address BOOLEAN NOT NULL DEFAULT FALSE
);
