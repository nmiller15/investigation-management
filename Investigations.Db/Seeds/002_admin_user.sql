-- System Administrator User
-- Creates the initial admin user for the system with user_key = 100
-- Run after 001_codes.sql and after you've added your ROLE codes

-- First, get admin role code key and create admin user
-- NOTE: Replace ROLE code values with your actual codes from the codes table

INSERT INTO users (
    first_name,
    last_name,
    email,
    birthdate,
    password_hash,
    role_code_key,
    inserted_by_user_key
) VALUES (
    'System',
    'Administrator',
    'admin@nolanmiller.me',
    '1997-09-02',
    -- ASP.NET Identity password hash placeholder
    -- Replace this with actual ASP.NET generated hash for "admin" password
    '\x_asp_net_hash_placeholder',
    (SELECT code_key FROM codes WHERE code_type = 'ROLE' AND code = 'ADMIN'),
    100 -- Self-referenced for first admin user
);

-- After inserting admin user, update codes to reference this user:
-- UPDATE codes SET inserted_by_user_key = 100 WHERE inserted_by_user_key = 100;

-- INSTRUCTIONS:
-- 1. Add your ROLE codes to 001_codes.sql first
-- 2. Update role code reference above ('ADMIN' → your admin code)
-- 3. Generate ASP.NET Identity hash/salt for "admin" password
-- 4. Uncomment and run this script
-- 5. The codes are already updated to reference admin user key 100

-- ASP.NET Identity Hash/Salt Generation:
-- Use your C# application to generate hash/salt for "admin" password:
-- var hasher = new PasswordHasher<ApplicationUser>();
-- var hash = hasher.HashPassword(user, "admin");
-- The hash contains both salt and hash, store the full value in password_hash
-- Set password_salt to NULL or empty for ASP.NET Identity
