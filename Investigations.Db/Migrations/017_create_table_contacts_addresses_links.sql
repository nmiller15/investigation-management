CREATE TABLE contacts_addresses_links (
    contact_id UUID NOT NULL,
    address_id UUID NOT NULL,
    is_primary_address BOOLEAN NOT NULL DEFAULT FALSE
);
