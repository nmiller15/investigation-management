-- Sample Addresses for Development
-- Creates addresses for sample clients and general use
-- Run after 003_sample_clients.sql

-- Acme Corporation Headquarters - New York
INSERT INTO addresses (
    line_one,
    line_two,
    city,
    state_code_key,
    country_code_key,
    zip,
    inserted_by_user_id
) VALUES (
    '1234 Broadway Avenue',
    'Suite 1500',
    'New York',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'NY'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTRY' AND code = 'US'),
    '10001',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Metro Insurance Group Headquarters - Illinois
INSERT INTO addresses (
    line_one,
    line_two,
    city,
    state_code_key,
    country_code_key,
    zip,
    inserted_by_user_id
) VALUES (
    '500 N Michigan Avenue',
    'Floor 25',
    'Chicago',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'IL'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTRY' AND code = 'US'),
    '60611',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Jane Doe Law Firm - California
INSERT INTO addresses (
    line_one,
    line_two,
    city,
    state_code_key,
    country_code_key,
    zip,
    inserted_by_user_id
) VALUES (
    '789 Market Street',
    'Suite 200',
    'San Francisco',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'CA'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTRY' AND code = 'US'),
    '94103',
    (SELECT user_id FROM users WHERE email = 'admin@oliverinvestigations.com')
);

-- Update clients with their address IDs
UPDATE clients SET address_id = (
    SELECT address_id FROM addresses 
    WHERE line_one = '1234 Broadway Avenue' AND line_two = 'Suite 1500'
) WHERE client_name = 'Acme Corporation';

UPDATE clients SET address_id = (
    SELECT address_id FROM addresses 
    WHERE line_one = '500 N Michigan Avenue' AND line_two = 'Floor 25'
) WHERE client_name = 'Metro Insurance Group';

UPDATE clients SET address_id = (
    SELECT address_id FROM addresses 
    WHERE line_one = '789 Market Street' AND line_two = 'Suite 200'
) WHERE client_name = 'Jane Doe Law Firm';