CREATE VIEW v_addresses AS
    SELECT
        a.address_id,
        a.line_one,
        a.line_two,
        a.city,
        a.state_code_key,
        sc.code_description AS state,
        sc.code_short_description AS state_abbreviation,
        a.country_code_key,
        cc.code_description AS country,
        cc.code_short_description  AS country_abbreviation,
        a.zip,
        a.inserted_by_user_id,
        iu.first_name AS inserted_by_first_name,
        iu.last_name AS inserted_by_last_name,
        a.inserted_datetime,
        a.updated_by_user_id,
        uu.first_name AS updated_by_first_name,
        uu.last_name AS updated_by_last_name,
        a.updated_datetime
    FROM addresses as a
    LEFT JOIN codes AS sc ON a.state_code_key = sc.code_key
    LEFT JOIN codes AS cc ON a.country_code_key = cc.code_key
    LEFT JOIN users AS iu ON a.inserted_by_user_id = iu.user_id
    LEFT JOIN users AS uu ON a.updated_by_user_id = uu.user_id;
