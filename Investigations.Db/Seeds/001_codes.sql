-- Geographic Reference Data
-- ISO-compliant codes for US states, territories, and North American countries
-- Run this first as other seed files depend on these codes

-- Insert statement will reference admin_user_id
-- This value should be updated after admin user is created in 002_admin_user.sql

-- ==== US STATES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('STATE', 'AL', 'Alabama', 'AL', NULL),
('STATE', 'AK', 'Alaska', 'AK', NULL),
('STATE', 'AZ', 'Arizona', 'AZ', NULL),
('STATE', 'AR', 'Arkansas', 'AR', NULL),
('STATE', 'CA', 'California', 'CA', NULL),
('STATE', 'CO', 'Colorado', 'CO', NULL),
('STATE', 'CT', 'Connecticut', 'CT', NULL),
('STATE', 'DE', 'Delaware', 'DE', NULL),
('STATE', 'FL', 'Florida', 'FL', NULL),
('STATE', 'GA', 'Georgia', 'GA', NULL),
('STATE', 'HI', 'Hawaii', 'HI', NULL),
('STATE', 'ID', 'Idaho', 'ID', NULL),
('STATE', 'IL', 'Illinois', 'IL', NULL),
('STATE', 'IN', 'Indiana', 'IN', NULL),
('STATE', 'IA', 'Iowa', 'IA', NULL),
('STATE', 'KS', 'Kansas', 'KS', NULL),
('STATE', 'KY', 'Kentucky', 'KY', NULL),
('STATE', 'LA', 'Louisiana', 'LA', NULL),
('STATE', 'ME', 'Maine', 'ME', NULL),
('STATE', 'MD', 'Maryland', 'MD', NULL),
('STATE', 'MA', 'Massachusetts', 'MA', NULL),
('STATE', 'MI', 'Michigan', 'MI', NULL),
('STATE', 'MN', 'Minnesota', 'MN', NULL),
('STATE', 'MS', 'Mississippi', 'MS', NULL),
('STATE', 'MO', 'Missouri', 'MO', NULL),
('STATE', 'MT', 'Montana', 'MT', NULL),
('STATE', 'NE', 'Nebraska', 'NE', NULL),
('STATE', 'NV', 'Nevada', 'NV', NULL),
('STATE', 'NH', 'New Hampshire', 'NH', NULL),
('STATE', 'NJ', 'New Jersey', 'NJ', NULL),
('STATE', 'NM', 'New Mexico', 'NM', NULL),
('STATE', 'NY', 'New York', 'NY', NULL),
('STATE', 'NC', 'North Carolina', 'NC', NULL),
('STATE', 'ND', 'North Dakota', 'ND', NULL),
('STATE', 'OH', 'Ohio', 'OH', NULL),
('STATE', 'OK', 'Oklahoma', 'OK', NULL),
('STATE', 'OR', 'Oregon', 'OR', NULL),
('STATE', 'PA', 'Pennsylvania', 'PA', NULL),
('STATE', 'RI', 'Rhode Island', 'RI', NULL),
('STATE', 'SC', 'South Carolina', 'SC', NULL),
('STATE', 'SD', 'South Dakota', 'SD', NULL),
('STATE', 'TN', 'Tennessee', 'TN', NULL),
('STATE', 'TX', 'Texas', 'TX', NULL),
('STATE', 'UT', 'Utah', 'UT', NULL),
('STATE', 'VT', 'Vermont', 'VT', NULL),
('STATE', 'VA', 'Virginia', 'VA', NULL),
('STATE', 'WA', 'Washington', 'WA', NULL),
('STATE', 'WV', 'West Virginia', 'WV', NULL),
('STATE', 'WI', 'Wisconsin', 'WI', NULL),
('STATE', 'WY', 'Wyoming', 'WY', NULL),
('STATE', 'DC', 'District of Columbia', 'DC', NULL);

-- ==== US TERRITORIES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('STATE', 'PR', 'Puerto Rico', 'PR', NULL),
('STATE', 'GU', 'Guam', 'GU', NULL),
('STATE', 'VI', 'Virgin Islands', 'VI', NULL),
('STATE', 'AS', 'American Samoa', 'AS', NULL),
('STATE', 'MP', 'Northern Mariana Islands', 'MP', NULL);

-- ==== NORTH AMERICAN COUNTRIES (ISO 3166-1 alpha-2) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('COUNTR', 'US', 'United States', 'US', NULL),
('COUNTR', 'CA', 'Canada', 'CA', NULL),
('COUNTR', 'MX', 'Mexico', 'MX', NULL),
('COUNTR', 'GT', 'Guatemala', 'GT', NULL),
('COUNTR', 'BN', 'Belize', 'BN', NULL),
('COUNTR', 'SV', 'El Salvador', 'SV', NULL),
('COUNTR', 'HN', 'Honduras', 'HN', NULL),
('COUNTR', 'NI', 'Nicaragua', 'NI', NULL),
('COUNTR', 'CR', 'Costa Rica', 'CR', NULL),
('COUNTR', 'PA', 'Panama', 'PA', NULL);

-- User Roles
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('ROLE', 'ADMIN', 'System Administrator', 'Admin', NULL),
('ROLE', 'OWNER', 'Accout Owner', 'Owner', NULL),
('ROLE', 'INVESTIGAT', 'Investigator', 'Inv', NULL);

-- Case Types
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('CASTYP', 'BCK', 'Background', 'Bg Check', NULL),
('CASTYP', 'CVL', 'Civil', 'Civil', NULL),
('CASTYP', 'COLL', 'Collection', 'Collection', NULL),
('CASTYP', 'CRI', 'Criminal', 'Criminal', NULL),
('CASTYP', 'CUST', 'Custody', 'Custody', NULL),
('CASTYP', 'DTH', 'Death', 'Death', NULL),
('CASTYP', 'DIV', 'Divorce', 'Divorce', NULL),
('CASTYP', 'DOM', 'Domestic', 'Domestic', NULL),
('CASTYP', 'EMP', 'Employer', 'Employer', NULL),
('CASTYP', 'FRA', 'Fraud', 'Fraud', NULL),
('CASTYP', 'HCD', 'Homicide', 'Homicide', NULL),
('CASTYP', 'IND', 'Industrial', 'Industrial', NULL),
('CASTYP', 'FID', 'Infidelity', 'Infidelity', NULL),
('CASTYP', 'INS', 'Insurance', 'Insurance', NULL),
('CASTYP', 'LCAT', 'Locate', 'Locate', NULL),
('CASTYP', 'MSPR', 'Missing Person', 'Missing Person', NULL),
('CASTYP', 'OIS', 'Officer Involved Shooting', 'Offr Inv Shooting', NULL),
('CASTYP', 'POL', 'Political', 'Political', NULL),
('CASTYP', 'PRO', 'Process Service', 'Process Service', NULL),
('CASTYP', 'SEC', 'Protection', 'Protection', NULL),
('CASTYP', 'STLK', 'Stalking', 'Stalking', NULL),
('CASTYP', 'SWP', 'Sweep', 'Sweep', NULL),
('CASTYP', 'TRA', 'Traffic Crash', 'Traffic Crash', NULL),
('CASTYP', 'UOF', 'Use Of Force', 'Use Of Force', NULL);

-- Marital Status
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('MARITL', 'SINGLE', 'Never Married', 'Single', NULL),
('MARITL', 'MARRIED', 'Currently Married', 'Married', NULL),
('MARITL', 'DIVORCED', 'Divorced', 'Divorced', NULL),
('MARITL', 'WIDOWED', 'Widowed', 'Widowed', NULL),
('MARITL', 'SEPARATED', 'Legally Separated', 'Separated', NULL);

-- Gender
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('GENDER', 'M', 'Male', 'M', NULL),
('GENDER', 'F', 'Female', 'F', NULL),
('GENDER', 'O', 'Other/Non-binary', 'Other', NULL),
('GENDER', 'U', 'Undisclosed', 'Unknown', NULL);

-- Contact Methods
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_id) VALUES
('CONTAC', 'EMAIL', 'Email', 'Email', NULL),
('CONTAC', 'MOBILE', 'Mobile Phone', 'Mobile', NULL),
('CONTAC', 'WORK', 'Work Phone', 'Work', NULL),
('CONTAC', 'HOME', 'Home Phone', 'Home', NULL);

-- NOTE: After creating admin user, update inserted_by_user_id fields:
-- UPDATE codes SET inserted_by_user_id = [admin_user_uuid] WHERE inserted_by_user_id IS NULL;
