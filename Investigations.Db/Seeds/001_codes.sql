-- Geographic Reference Data
-- ISO-compliant codes for US states, territories, and North American countries
-- Run this first as other seed files depend on these codes

-- Insert statement will reference admin_user_id
-- This value should be updated after admin user is created in 002_admin_user.sql

-- ==== US STATES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('STATE', 'AL', 'Alabama', 'AL', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'AK', 'Alaska', 'AK', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'AZ', 'Arizona', 'AZ', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'AR', 'Arkansas', 'AR', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'CA', 'California', 'CA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'CO', 'Colorado', 'CO', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'CT', 'Connecticut', 'CT', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'DE', 'Delaware', 'DE', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'FL', 'Florida', 'FL', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'GA', 'Georgia', 'GA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'HI', 'Hawaii', 'HI', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'ID', 'Idaho', 'ID', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'IL', 'Illinois', 'IL', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'IN', 'Indiana', 'IN', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'IA', 'Iowa', 'IA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'KS', 'Kansas', 'KS', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'KY', 'Kentucky', 'KY', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'LA', 'Louisiana', 'LA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'ME', 'Maine', 'ME', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MD', 'Maryland', 'MD', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MA', 'Massachusetts', 'MA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MI', 'Michigan', 'MI', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MN', 'Minnesota', 'MN', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MS', 'Mississippi', 'MS', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MO', 'Missouri', 'MO', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MT', 'Montana', 'MT', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NE', 'Nebraska', 'NE', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NV', 'Nevada', 'NV', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NH', 'New Hampshire', 'NH', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NJ', 'New Jersey', 'NJ', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NM', 'New Mexico', 'NM', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NY', 'New York', 'NY', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'NC', 'North Carolina', 'NC', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'ND', 'North Dakota', 'ND', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'OH', 'Ohio', 'OH', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'OK', 'Oklahoma', 'OK', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'OR', 'Oregon', 'OR', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'PA', 'Pennsylvania', 'PA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'RI', 'Rhode Island', 'RI', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'SC', 'South Carolina', 'SC', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'SD', 'South Dakota', 'SD', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'TN', 'Tennessee', 'TN', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'TX', 'Texas', 'TX', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'UT', 'Utah', 'UT', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'VT', 'Vermont', 'VT', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'VA', 'Virginia', 'VA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'WA', 'Washington', 'WA', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'WV', 'West Virginia', 'WV', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'WI', 'Wisconsin', 'WI', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'WY', 'Wyoming', 'WY', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'DC', 'District of Columbia', 'DC', '00000000-0000-0000-0000-000000000000'::uuid);

-- ==== US TERRITORIES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('STATE', 'PR', 'Puerto Rico', 'PR', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'GU', 'Guam', 'GU', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'VI', 'Virgin Islands', 'VI', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'AS', 'American Samoa', 'AS', '00000000-0000-0000-0000-000000000000'::uuid),
('STATE', 'MP', 'Northern Mariana Islands', 'MP', '00000000-0000-0000-0000-000000000000'::uuid);

-- ==== NORTH AMERICAN COUNTRIES (ISO 3166-1 alpha-2) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('COUNTR', 'US', 'United States', 'US', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'CA', 'Canada', 'CA', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'MX', 'Mexico', 'MX', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'GT', 'Guatemala', 'GT', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'BN', 'Belize', 'BN', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'SV', 'El Salvador', 'SV', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'HN', 'Honduras', 'HN', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'NI', 'Nicaragua', 'NI', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'CR', 'Costa Rica', 'CR', '00000000-0000-0000-0000-000000000000'::uuid),
('COUNTR', 'PA', 'Panama', 'PA', '00000000-0000-0000-0000-000000000000'::uuid);

-- User Roles
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('ROLE', 'ADMIN', 'System Administrator', 'Admin', '00000000-0000-0000-0000-000000000000'::uuid),
('ROLE', 'OWNER', 'Accout Owner', 'Owner', '00000000-0000-0000-0000-000000000000'::uuid),
('ROLE', 'INVESTIGAT', 'Investigator', 'Inv', '00000000-0000-0000-0000-000000000000'::uuid);

-- Case Types
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('CASTYP', 'BCK', 'Background', 'Bg Check', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'CVL', 'Civil', 'Civil', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'COLL', 'Collection', 'Collection', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'CRI', 'Criminal', 'Criminal', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'CUST', 'Custody', 'Custody', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'DTH', 'Death', 'Death', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'DIV', 'Divorce', 'Divorce', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'DOM', 'Domestic', 'Domestic', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'EMP', 'Employer', 'Employer', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'FRA', 'Fraud', 'Fraud', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'HCD', 'Homicide', 'Homicide', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'IND', 'Industrial', 'Industrial', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'FID', 'Infidelity', 'Infidelity', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'INS', 'Insurance', 'Insurance', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'LCAT', 'Locate', 'Locate', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'MSPR', 'Missing Person', 'Missing Person', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'OIS', 'Officer Involved Shooting', 'Offr Inv Shooting', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'POL', 'Political', 'Political', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'PRO', 'Process Service', 'Process Service', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'SEC', 'Protection', 'Protection', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'STLK', 'Stalking', 'Stalking', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'SWP', 'Sweep', 'Sweep', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'TRA', 'Traffic Crash', 'Traffic Crash', '00000000-0000-0000-0000-000000000000'::uuid),
('CASTYP', 'UOF', 'Use Of Force', 'Use Of Force', '00000000-0000-0000-0000-000000000000'::uuid);

-- Marital Status
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('MARITL', 'SINGLE', 'Never Married', 'Single', '00000000-0000-0000-0000-000000000000'::uuid),
('MARITL', 'MARRIED', 'Currently Married', 'Married', '00000000-0000-0000-0000-000000000000'::uuid),
('MARITL', 'DIVORCED', 'Divorced', 'Divorced', '00000000-0000-0000-0000-000000000000'::uuid),
('MARITL', 'WIDOWED', 'Widowed', 'Widowed', '00000000-0000-0000-0000-000000000000'::uuid),
('MARITL', 'SEPARATED', 'Legally Separated', 'Separated', '00000000-0000-0000-0000-000000000000'::uuid);

-- Gender
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('GENDER', 'M', 'Male', 'M', '00000000-0000-0000-0000-000000000000'::uuid),
('GENDER', 'F', 'Female', 'F', '00000000-0000-0000-0000-000000000000'::uuid),
('GENDER', 'O', 'Other/Non-binary', 'Other', '00000000-0000-0000-0000-000000000000'::uuid),
('GENDER', 'U', 'Undisclosed', 'Unknown', '00000000-0000-0000-0000-000000000000'::uuid);

-- Contact Methods
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('CONTAC', 'EMAIL', 'Email', 'Email', '00000000-0000-0000-0000-000000000000'::uuid),
('CONTAC', 'MOBILE', 'Mobile Phone', 'Mobile', '00000000-0000-0000-0000-000000000000'::uuid),
('CONTAC', 'WORK', 'Work Phone', 'Work', '00000000-0000-0000-0000-000000000000'::uuid),
('CONTAC', 'HOME', 'Home Phone', 'Home', '00000000-0000-0000-0000-000000000000'::uuid);

-- NOTE: After creating admin user, update inserted_by_user_id fields:
-- UPDATE codes SET inserted_by_user_id = [admin_user_uuid] WHERE inserted_by_user_id = '00000000-0000-0000-0000-000000000000'::uuid;
