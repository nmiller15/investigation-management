-- Sample Contacts for Development
-- Creates contact persons for sample clients
-- Run after 004_sample_addresses.sql

-- Primary contact for Acme Corporation
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'John',
    'Smith',
    'jsmith@acmecorp.com',
    '(555) 234-5678',
    '(555) 123-4567',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'WORK'),
    'Primary contact for corporate investigations',
    100
);

-- Link John Smith to Acme Corporation as primary contact
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Acme Corporation'),
    TRUE,
    (SELECT contact_key FROM contacts WHERE email = 'jsmith@acmecorp.com'),
    TRUE
);

-- Primary contact for Metro Insurance Group
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'Sarah',
    'Johnson',
    'sjohnson@metroinsurance.com',
    '(555) 876-5432',
    '(555) 987-6543',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'WORK'),
    'Primary contact for insurance claims investigations',
    100
);

-- Link Sarah Johnson to Metro Insurance Group as primary contact
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Metro Insurance Group'),
    TRUE,
    (SELECT contact_key FROM contacts WHERE email = 'sjohnson@metroinsurance.com'),
    TRUE
);

-- Primary contact for Jane Doe Law Firm
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'Jane',
    'Doe',
    'jane@janedoelaw.com',
    '(555) 135-7924',
    '(555) 246-8135',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'MOBILE'),
    'Primary attorney contact for legal investigations',
    100
);

-- Link Jane Doe to Jane Doe Law Firm as primary contact
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Jane Doe Law Firm'),
    TRUE,
    (SELECT contact_key FROM contacts WHERE email = 'jane@janedoelaw.com'),
    TRUE
);

-- Additional contact for Acme Corporation
INSERT INTO contacts (
    first_name,
    last_name,
    email,
    mobile_phone,
    work_phone,
    home_phone,
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'Emily',
    'Chen',
    'echen@acmecorp.com',
    '(555) 234-5678',
    '(555) 123-4567',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'EMAIL'),
    'Secondary contact for corporate investigations',
    100
);

-- Link Emily Chen to Acme Corporation
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Acme Corporation'),
    FALSE,
    (SELECT contact_key FROM contacts WHERE email = 'echen@acmecorp.com'),
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
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'Michael',
    'Williams',
    'mwilliams@metroinsurance.com',
    '(555) 876-5432',
    '(555) 987-6543',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'WORK'),
    'Claims department lead investigator contact',
    100
);

-- Link Michael Williams to Metro Insurance Group
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Metro Insurance Group'),
    FALSE,
    (SELECT contact_key FROM contacts WHERE email = 'mwilliams@metroinsurance.com'),
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
    preferred_contact_method_code_key,
    notes,
    inserted_by_user_key
) VALUES (
    'Robert',
    'Garcia',
    'rgarcia@janedoelaw.com',
    '(555) 135-7924',
    '(555) 246-8135',
    NULL,
    (SELECT code_key FROM codes WHERE code_type = 'CONTAC' AND code = 'MOBILE'),
    'Paralegal and case coordinator for Jane Doe',
    100
);

-- Link Robert Garcia to Jane Doe Law Firm
INSERT INTO clients_contacts_links (
    client_key,
    is_primary_client,
    contact_key,
    is_primary_contact
) VALUES (
    (SELECT client_key FROM clients WHERE client_name = 'Jane Doe Law Firm'),
    FALSE,
    (SELECT contact_key FROM contacts WHERE email = 'rgarcia@janedoelaw.com'),
    FALSE
);

-- Now update clients with their primary contact keys
UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'jsmith@acmecorp.com') WHERE client_name = 'Acme Corporation';
UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'sjohnson@metroinsurance.com') WHERE client_name = 'Metro Insurance Group';
UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'jane@janedoelaw.com') WHERE client_name = 'Jane Doe Law Firm';
