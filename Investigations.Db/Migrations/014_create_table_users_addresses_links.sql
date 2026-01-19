CREATE TABLE IF NOT EXISTS users_addresses_links (
    user_address_link_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    user_key INT NOT NULL,
    address_key INT NOT NULL,
    is_primary_address BOOLEAN DEFAULT FALSE
);
