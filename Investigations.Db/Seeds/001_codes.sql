-- Geographic Reference Data
-- ISO-compliant codes for US states, territories, and North American countries
-- Run this first as other seed files depend on these codes

-- Insert statement will reference admin user with key 100
-- Admin user will be created in 002_admin_user.sql with key 100

-- ==== US STATES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('STATE', 'AL', 'Alabama', 'AL', 100),
('STATE', 'AK', 'Alaska', 'AK', 100),
('STATE', 'AZ', 'Arizona', 'AZ', 100),
('STATE', 'AR', 'Arkansas', 'AR', 100),
('STATE', 'CA', 'California', 'CA', 100),
('STATE', 'CO', 'Colorado', 'CO', 100),
('STATE', 'CT', 'Connecticut', 'CT', 100),
('STATE', 'DE', 'Delaware', 'DE', 100),
('STATE', 'FL', 'Florida', 'FL', 100),
('STATE', 'GA', 'Georgia', 'GA', 100),
('STATE', 'HI', 'Hawaii', 'HI', 100),
('STATE', 'ID', 'Idaho', 'ID', 100),
('STATE', 'IL', 'Illinois', 'IL', 100),
('STATE', 'IN', 'Indiana', 'IN', 100),
('STATE', 'IA', 'Iowa', 'IA', 100),
('STATE', 'KS', 'Kansas', 'KS', 100),
('STATE', 'KY', 'Kentucky', 'KY', 100),
('STATE', 'LA', 'Louisiana', 'LA', 100),
('STATE', 'ME', 'Maine', 'ME', 100),
('STATE', 'MD', 'Maryland', 'MD', 100),
('STATE', 'MA', 'Massachusetts', 'MA', 100),
('STATE', 'MI', 'Michigan', 'MI', 100),
('STATE', 'MN', 'Minnesota', 'MN', 100),
('STATE', 'MS', 'Mississippi', 'MS', 100),
('STATE', 'MO', 'Missouri', 'MO', 100),
('STATE', 'MT', 'Montana', 'MT', 100),
('STATE', 'NE', 'Nebraska', 'NE', 100),
('STATE', 'NV', 'Nevada', 'NV', 100),
('STATE', 'NH', 'New Hampshire', 'NH', 100),
('STATE', 'NJ', 'New Jersey', 'NJ', 100),
('STATE', 'NM', 'New Mexico', 'NM', 100),
('STATE', 'NY', 'New York', 'NY', 100),
('STATE', 'NC', 'North Carolina', 'NC', 100),
('STATE', 'ND', 'North Dakota', 'ND', 100),
('STATE', 'OH', 'Ohio', 'OH', 100),
('STATE', 'OK', 'Oklahoma', 'OK', 100),
('STATE', 'OR', 'Oregon', 'OR', 100),
('STATE', 'PA', 'Pennsylvania', 'PA', 100),
('STATE', 'RI', 'Rhode Island', 'RI', 100),
('STATE', 'SC', 'South Carolina', 'SC', 100),
('STATE', 'SD', 'South Dakota', 'SD', 100),
('STATE', 'TN', 'Tennessee', 'TN', 100),
('STATE', 'TX', 'Texas', 'TX', 100),
('STATE', 'UT', 'Utah', 'UT', 100),
('STATE', 'VT', 'Vermont', 'VT', 100),
('STATE', 'VA', 'Virginia', 'VA', 100),
('STATE', 'WA', 'Washington', 'WA', 100),
('STATE', 'WV', 'West Virginia', 'WV', 100),
('STATE', 'WI', 'Wisconsin', 'WI', 100),
('STATE', 'WY', 'Wyoming', 'WY', 100),
('STATE', 'DC', 'District of Columbia', 'DC', 100);

-- ==== US TERRITORIES (ISO 3166-2:US) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('STATE', 'PR', 'Puerto Rico', 'PR', 100),
('STATE', 'GU', 'Guam', 'GU', 100),
('STATE', 'VI', 'Virgin Islands', 'VI', 100),
('STATE', 'AS', 'American Samoa', 'AS', 100),
('STATE', 'MP', 'Northern Mariana Islands', 'MP', 100);

-- ==== NORTH AMERICAN COUNTRIES (ISO 3166-1 alpha-2) ====
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('COUNTR', 'US', 'United States', 'US', 100),
('COUNTR', 'CA', 'Canada', 'CA', 100),
('COUNTR', 'MX', 'Mexico', 'MX', 100),
('COUNTR', 'GT', 'Guatemala', 'GT', 100),
('COUNTR', 'BN', 'Belize', 'BN', 100),
('COUNTR', 'SV', 'El Salvador', 'SV', 100),
('COUNTR', 'HN', 'Honduras', 'HN', 100),
('COUNTR', 'NI', 'Nicaragua', 'NI', 100),
('COUNTR', 'CR', 'Costa Rica', 'CR', 100),
('COUNTR', 'PA', 'Panama', 'PA', 100);

-- User Roles
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('ROLE', 'ADMIN', 'System Administrator', 'Admin', 100),
('ROLE', 'OWNER', 'Account Owner', 'Owner', 100),
('ROLE', 'INVESTIGAT', 'Investigator', 'Inv', 100);

-- Case Types
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('CASTYP', 'BCK', 'Background', 'Bg Check', 100),
('CASTYP', 'CVL', 'Civil', 'Civil', 100),
('CASTYP', 'COLL', 'Collection', 'Collection', 100),
('CASTYP', 'CRI', 'Criminal', 'Criminal', 100),
('CASTYP', 'CUST', 'Custody', 'Custody', 100),
('CASTYP', 'DTH', 'Death', 'Death', 100),
('CASTYP', 'DIV', 'Divorce', 'Divorce', 100),
('CASTYP', 'DOM', 'Domestic', 'Domestic', 100),
('CASTYP', 'EMP', 'Employer', 'Employer', 100),
('CASTYP', 'FRA', 'Fraud', 'Fraud', 100),
('CASTYP', 'HCD', 'Homicide', 'Homicide', 100),
('CASTYP', 'IND', 'Industrial', 'Industrial', 100),
('CASTYP', 'FID', 'Infidelity', 'Infidelity', 100),
('CASTYP', 'INS', 'Insurance', 'Insurance', 100),
('CASTYP', 'LCAT', 'Locate', 'Locate', 100),
('CASTYP', 'MSPR', 'Missing Person', 'Missing Person', 100),
('CASTYP', 'OIS', 'Officer Involved Shooting', 'Offr Inv Shooting', 100),
('CASTYP', 'POL', 'Political', 'Political', 100),
('CASTYP', 'PRO', 'Process Service', 'Process Service', 100),
('CASTYP', 'SEC', 'Protection', 'Protection', 100),
('CASTYP', 'STLK', 'Stalking', 'Stalking', 100),
('CASTYP', 'SWP', 'Sweep', 'Sweep', 100),
('CASTYP', 'TRA', 'Traffic Crash', 'Traffic Crash', 100),
('CASTYP', 'UOF', 'Use Of Force', 'Use Of Force', 100);

-- Marital Status
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('MARITL', 'SINGLE', 'Never Married', 'Single', 100),
('MARITL', 'MARRIED', 'Currently Married', 'Married', 100),
('MARITL', 'DIVORCED', 'Divorced', 'Divorced', 100),
('MARITL', 'WIDOWED', 'Widowed', 'Widowed', 100),
('MARITL', 'SEPARATED', 'Legally Separated', 'Separated', 100);

-- Gender
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('GENDER', 'M', 'Male', 'M', 100),
('GENDER', 'F', 'Female', 'F', 100),
('GENDER', 'O', 'Other/Non-binary', 'Other', 100),
('GENDER', 'U', 'Undisclosed', 'Unknown', 100);

-- Contact Methods
INSERT INTO codes (code_type, code, code_description, code_short_description, inserted_by_user_key) VALUES
('CONTAC', 'EMAIL', 'Email', 'Email', 100),
('CONTAC', 'MOBILE', 'Mobile Phone', 'Mobile', 100),
('CONTAC', 'WORK', 'Work Phone', 'Work', 100),
('CONTAC', 'HOME', 'Home Phone', 'Home', 100);

-- NOTE: Admin user will be created with user_key = 100 in 002_admin_user.sql
