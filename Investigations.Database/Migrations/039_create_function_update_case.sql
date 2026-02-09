CREATE OR REPLACE FUNCTION update_case(
    p_case_key INT,
    p_case_number VARCHAR(15),
    p_is_active BOOLEAN, 
    p_subject_key INT,
    p_client_key INT,
    p_date_of_referral TIMESTAMPTZ,
    p_case_type_code_key INT,
    p_synopsis TEXT,
    p_updated_by_user_key INT
)

RETURNS INT
LANGUAGE sql
VOLATILE
AS $$
    UPDATE cases 
    SET
        case_number = p_case_number,
        is_active = p_is_active,
        subject_key = p_subject_key,
        client_key = p_client_key,
        date_of_referral = p_date_of_referral,
        case_type_code_key = p_case_type_code_key,
        synopsis = p_synopsis,
        updated_datetime = now(),
        updated_by_user_key = p_updated_by_user_key
    WHERE case_key = p_case_key
    RETURNING case_key;
$$;

GRANT EXECUTE ON FUNCTION update_case(INT, VARCHAR, BOOLEAN, INT, INT, TIMESTAMPTZ, INT, TEXT, INT) TO app_user;
