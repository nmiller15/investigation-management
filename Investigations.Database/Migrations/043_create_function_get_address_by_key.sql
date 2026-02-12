CREATE OR REPLACE FUNCTION get_address_by_key(
    p_address_key INT
)
RETURNS SETOF v_addresses
LANGUAGE sql
STABLE
AS $$
    SELECT 
        va.address_key,
        va.line_one,
        va.line_two,
        va.city,
        va.state_code_key,
        va.state,
        va.state_abbreviation,
        va.country_code_key,
        va.country,
        va.country_abbreviation,
        va.zip,
        va.inserted_by_user_key,
        va.inserted_by_first_name,
        va.inserted_by_last_name,
        va.inserted_datetime,
        va.updated_by_user_key,
        va.updated_by_first_name,
        va.updated_by_last_name,
        va.updated_datetime
    FROM v_addresses AS va
    WHERE address_key = p_address_key
    LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION get_address_by_key(INT) TO app_user;
