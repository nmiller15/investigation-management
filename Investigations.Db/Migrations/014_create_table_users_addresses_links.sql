CREATE TABLE IF NOT EXISTS users_addresses_links (
    user_id UUID NOT NULL,
    address_id UUID NOT NULL,
    is_primary_address BOOLEAN DEFAULT FALSE
);
