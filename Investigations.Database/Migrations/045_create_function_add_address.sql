CREATE OR REPLACE FUNCTION add_address(
    p_line_one VARCHAR,
    p_line_two VARCHAR,
    p_city VARCHAR,
    p_state_code_key INT,
    p_country_code_key INT,
    p_zip VARCHAR,
    p_inserted_by_user_key INT
)

RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    INSERT INTO addresses (
        line_one,
        line_two,
        city,
        state_code_key,
        country_code_key,
        zip,
        inserted_datetime,
        inserted_by_user_key
    ) VALUES (
        p_line_one,
        p_line_two,
        p_city,
        p_state_code_key,
        p_country_code_key,
        p_zip,
        now(),
        p_inserted_by_user_key
    )
    RETURNING address_key;
$$;

GRANT EXECUTE ON FUNCTION add_address(VARCHAR, VARCHAR, VARCHAR, INT, INT, VARCHAR, INT) TO app_user;
