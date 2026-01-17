CREATE TABLE IF NOT EXISTS clients_contacts_links (
    clients_id UUID NOT NULL,
    is_primary_client BOOLEAN NOT NULL DEFAULT FALSE,
    contact_id UUID NOT NULL,
    is_primary_contact BOOLEAN NOT NULL DEFAULT FALSE
);
