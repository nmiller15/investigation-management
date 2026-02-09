CREATE OR REPLACE FUNCTION get_case_by_key(
    p_case_key INT
)
RETURNS SETOF v_cases
LANGUAGE sql
STABLE
AS $$
    SELECT 
        vc.case_key,
        vc.case_number,
        vc.is_active,
        vc.subject_key,
        vc.subject_first_name,
        vc.subject_last_name,
        vc.client_key,
        vc.client_name,
        vc.date_of_referral,
        vc.case_type_code_key,
        vc.case_type_code,
        vc.case_type_short_description,
        vc.case_type_description,
        vc.synopsis,
        vc.inserted_by_user_key,
        vc.inserted_by_first_name,
        vc.inserted_by_last_name,
        vc.inserted_datetime,
        vc.updated_by_user_key,
        vc.updated_by_first_name,
        vc.updated_by_last_name,
        vc.updated_datetime
    FROM v_cases AS vc
    WHERE case_key = p_case_key
    LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION get_case_by_key(INT) TO app_user;
