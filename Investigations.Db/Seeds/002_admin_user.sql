-- System Administrator User
-- Creates the initial admin user for the system
-- Run after 001_codes.sql and after you've added your ROLE codes

-- First, get the admin role code key and create the admin user
-- NOTE: Replace ROLE code values with your actual codes from the codes table

-- INSERT INTO users (
--     first_name,
--     last_name,
--     email,
--     birthdate,
--     password_hash,
--     role_code_key,
--     inserted_by_user_id
-- ) VALUES (
--     'System',
--     'Administrator',
--     'admin@nolanmiller.me',
--     '1980-01-01',
--     -- ASP.NET Identity password hash placeholder
--     -- Replace this with actual ASP.NET generated hash for "admin" password
--     '\x_asp_net_hash_placeholder',
--     (SELECT code_key FROM codes WHERE code_type = 'ROLE' AND code = 'ADMIN'),
--     '00000000-0000-0000-0000-000000000000'::uuid -- First record has no inserted_by_user_id
-- );

-- After inserting admin user, update the codes to reference this user:
-- UPDATE codes SET inserted_by_user_id = (SELECT user_id FROM users WHERE email = 'admin@nolanmiller.me') 
-- WHERE inserted_by_user_id IS '00000000-0000-0000-0000-000000000000'::uuid;

-- INSTRUCTIONS:
-- 1. Add your ROLE codes to 001_codes.sql first
-- 2. Update the role code reference above ('ADMIN' → your admin code)
-- 3. Generate ASP.NET Identity hash/salt for "admin" password
-- 4. Uncomment and run this script
-- 5. Run the update statement to set inserted_by_user_id in codes table

-- ASP.NET Identity Hash/Salt Generation:
-- Use your C# application to generate hash/salt for "admin" password:
-- var hasher = new PasswordHasher<ApplicationUser>();
-- var hash = hasher.HashPassword(user, "admin");
-- The hash contains both salt and hash, store the full value in password_hash
-- Set password_salt to NULL or empty for ASP.NET Identity
