-- Sample Contacts for Development
-- Creates contact persons for sample clients
-- Run after 004_sample_addresses.sql

-- Additional contact for Acme Corporation
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    notes,
    inserted_by_user_id
) VALUES (
    'Emily',
    'Chen',
    'echen@acmecorp.com',
    '(555) 234-5678',
    '(555) 123-4567',
    NULL,
    'Secondary contact for corporate investigations',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Link Emily Chen to Acme Corporation
INSERT INTO clients_contacts_links (
    clients_id,
    is_primary_client,
    contact_id,
    is_primary_contact
) VALUES (
    (SELECT client_id FROM clients WHERE client_name = 'Acme Corporation'),
    TRUE,
    (SELECT contact_id FROM contacts WHERE email = 'echen@acmecorp.com'),
    FALSE
);

-- Additional contact for Metro Insurance Group
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    notes,
    inserted_by_user_id
) VALUES (
    'Michael',
    'Williams',
    'mwilliams@metroinsurance.com',
    '(555) 876-5432',
    '(555) 987-6543',
    NULL,
    'Claims department lead investigator contact',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Link Michael Williams to Metro Insurance Group
INSERT INTO clients_contacts_links (
    clients_id,
    is_primary_client,
    contact_id,
    is_primary_contact
) VALUES (
    (SELECT client_id FROM clients WHERE client_name = 'Metro Insurance Group'),
    TRUE,
    (SELECT contact_id FROM contacts WHERE email = 'mwilliams@metroinsurance.com'),
    FALSE
);

-- Personal assistant for Jane Doe
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    notes,
    inserted_by_user_id
) VALUES (
    'Robert',
    'Garcia',
    'rgarcia@janedoelaw.com',
    '(555) 135-7924',
    '(555) 246-8135',
    NULL,
    'Paralegal and case coordinator for Jane Doe',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Link Robert Garcia to Jane Doe Law Firm
INSERT INTO clients_contacts_links (
    clients_id,
    is_primary_client,
    contact_id,
    is_primary_contact
) VALUES (
    (SELECT client_id FROM clients WHERE client_name = 'Jane Doe Law Firm'),
    TRUE,
    (SELECT contact_id FROM contacts WHERE email = 'rgarcia@janedoelaw.com'),
    FALSE
);