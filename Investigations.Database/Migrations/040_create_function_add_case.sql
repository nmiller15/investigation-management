CREATE OR REPLACE FUNCTION add_case(
    p_case_number VARCHAR(15),
    p_is_active BOOLEAN, 
    p_subject_key INT,
    p_client_key INT,
    p_date_of_referral TIMESTAMPTZ,
    p_case_type_code_key INT,
    p_synopsis TEXT,
    p_inserted_by_user_key INT
)

RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    INSERT INTO cases (
        case_number,
        is_active,
        subject_key,
        client_key,
        date_of_referral,
        case_type_code_key,
        synopsis,
        inserted_datetime,
        inserted_by_user_key
    ) VALUES (
        p_case_number,
        p_is_active,
        p_subject_key,
        p_client_key,
        p_date_of_referral,
        p_case_type_code_key,
        p_synopsis,
        now(),
        p_inserted_by_user_key
    )
    RETURNING case_key;
$$;

GRANT EXECUTE ON FUNCTION add_case(VARCHAR, BOOLEAN, INT, INT, TIMESTAMPTZ, INT, TEXT, INT) TO app_user;
