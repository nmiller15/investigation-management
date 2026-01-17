CREATE TABLE IF NOT EXISTS subjects_addresses_links (
    subject_id UUID NOT NULL,
    address_id UUID NOT NULL,
    is_primary_address BOOLEAN DEFAULT FALSE
);
