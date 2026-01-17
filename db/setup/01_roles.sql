CREATE ROLE admin WITH LOGIN;

CREATE ROLE app_user WITH LOGIN;

ALTER DATABASE core
    OWNER TO admin;

-- Passwords set out of band:
-- ALTER ROLE admin PASSWORD 'your-admin-password';
-- ALTER ROLE app_user PASSWORD 'your-app-password';
