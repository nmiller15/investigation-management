-- Sample Clients for Development
-- Creates 3 sample clients for testing the system
-- Run after 002_admin_user.sql

-- Acme Corporation - Corporate Client
INSERT INTO clients (
    client_name,
    primary_contact_key,
    inserted_by_user_key
) VALUES (
    'Acme Corporation',
    NULL, -- Will be updated after contacts are created in 005_sample_contacts.sql
    100
);

-- Metro Insurance Group - Insurance Client  
INSERT INTO clients (
    client_name,
    primary_contact_key,
    inserted_by_user_key
) VALUES (
    'Metro Insurance Group',
    NULL, -- Will be updated after contacts are created in 005_sample_contacts.sql
    100
);

-- Jane Doe Law Firm - Individual Professional Client
INSERT INTO clients (
    client_name,
    primary_contact_key,
    inserted_by_user_key
) VALUES (
    'Jane Doe Law Firm',
    NULL, -- Will be updated after contacts are created in 005_sample_contacts.sql
    100
);

-- NOTE: After creating contacts in 005_sample_contacts.sql, update primary_contact_key:
-- UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'jsmith@acmecorp.com') WHERE client_name = 'Acme Corporation';
-- UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'sjohnson@metroinsurance.com') WHERE client_name = 'Metro Insurance Group';
-- UPDATE clients SET primary_contact_key = (SELECT contact_key FROM contacts WHERE email = 'jane@janedoelaw.com') WHERE client_name = 'Jane Doe Law Firm';
