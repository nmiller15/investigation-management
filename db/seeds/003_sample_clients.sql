-- Sample Clients for Development
-- Creates 3 sample clients for testing the system
-- Run after 002_admin_user.sql

-- Acme Corporation - Corporate Client
INSERT INTO clients (
    client_name,
    contact_person,
    contact_email,
    contact_phone,
    address_id,
    inserted_by_user_id
) VALUES (
    'Acme Corporation',
    'John Smith',
    'jsmith@acmecorp.com',
    '(555) 123-4567',
    NULL, -- Will be updated in 004_sample_addresses.sql
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Metro Insurance Group - Insurance Client  
INSERT INTO clients (
    client_name,
    contact_person,
    contact_email,
    contact_phone,
    address_id,
    inserted_by_user_id
) VALUES (
    'Metro Insurance Group',
    'Sarah Johnson',
    'sjohnson@metroinsurance.com',
    '(555) 987-6543',
    NULL, -- Will be updated in 004_sample_addresses.sql
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Jane Doe Law Firm - Individual Professional Client
INSERT INTO clients (
    client_name,
    contact_person,
    contact_email,
    contact_phone,
    address_id,
    inserted_by_user_id
) VALUES (
    'Jane Doe Law Firm',
    'Jane Doe',
    'jane@janedoelaw.com',
    '(555) 246-8135',
    NULL, -- Will be updated in 004_sample_addresses.sql
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);