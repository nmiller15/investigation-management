CREATE TABLE IF NOT EXISTS clients_contacts_links (
    client_contact_link_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    client_key INT NOT NULL,
    is_primary_client BOOLEAN NOT NULL DEFAULT FALSE,
    contact_key INT NOT NULL,
    is_primary_contact BOOLEAN NOT NULL DEFAULT FALSE
);
