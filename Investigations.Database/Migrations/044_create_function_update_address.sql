CREATE OR REPLACE FUNCTION update_address(
    p_address_key INT,
    p_line_one VARCHAR,
    p_line_two VARCHAR,
    p_city VARCHAR,
    p_state_code_key INT,
    p_country_code_key INT,
    p_zip VARCHAR,
    p_updated_by_user_key INT
)

RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    UPDATE addresses 
    SET
        line_one = p_line_one,
        line_two = p_line_two,
        city = p_city,
        state_code_key = p_state_code_key,
        country_code_key = p_country_code_key,
        zip = p_zip,
        updated_datetime = now(),
        updated_by_user_key = p_updated_by_user_key
    WHERE address_key = p_address_key
    RETURNING address_key;
$$;

GRANT EXECUTE ON FUNCTION update_address(INT, VARCHAR, VARCHAR, VARCHAR, INT, INT, VARCHAR, INT) TO app_user;
