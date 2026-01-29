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
    inserted_by_user_key
) VALUES (
    '1234 Broadway Avenue',
    'Suite 1500',
    'New York',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'NY'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTR' AND code = 'US'),
    '10001',
    100
);

-- Metro Insurance Group Headquarters - Illinois
INSERT INTO addresses (
    line_one,
    line_two,
    city,
    state_code_key,
    country_code_key,
    zip,
    inserted_by_user_key
) VALUES (
    '500 N Michigan Avenue',
    'Floor 25',
    'Chicago',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'IL'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTR' AND code = 'US'),
    '60611',
    100
);

-- Jane Doe Law Firm - California
INSERT INTO addresses (
    line_one,
    line_two,
    city,
    state_code_key,
    country_code_key,
    zip,
    inserted_by_user_key
) VALUES (
    '789 Market Street',
    'Suite 200',
    'San Francisco',
    (SELECT code_key FROM codes WHERE code_type = 'STATE' AND code = 'CA'),
    (SELECT code_key FROM codes WHERE code_type = 'COUNTR' AND code = 'US'),
    '94103',
    100
);

-- Link clients to their addresses using clients_addresses_links table
INSERT INTO clients_addresses_links (
    client_key,
    address_key,
    is_primary_address
) 
SELECT 
    (SELECT client_key FROM clients WHERE client_name = 'Acme Corporation'),
    address_key,
    TRUE
FROM addresses 
WHERE line_one = '1234 Broadway Avenue' AND line_two = 'Suite 1500';

INSERT INTO clients_addresses_links (
    client_key,
    address_key,
    is_primary_address
) 
SELECT 
    (SELECT client_key FROM clients WHERE client_name = 'Metro Insurance Group'),
    address_key,
    TRUE
FROM addresses 
WHERE line_one = '500 N Michigan Avenue' AND line_two = 'Floor 25';

INSERT INTO clients_addresses_links (
    client_key,
    address_key,
    is_primary_address
) 
SELECT 
    (SELECT client_key FROM clients WHERE client_name = 'Jane Doe Law Firm'),
    address_key,
    TRUE
FROM addresses 
WHERE line_one = '789 Market Street' AND line_two = 'Suite 200';
