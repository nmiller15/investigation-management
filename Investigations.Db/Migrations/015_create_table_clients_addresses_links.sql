CREATE TABLE IF NOT EXISTS clients_addresses_links (
    client_id UUID NOT NULL,
    address_id UUID NOT NULL,
    is_primary_address BOOLEAN NOT NULL DEFAULT FALSE
);
