CREATE TABLE IF NOT EXISTS cases_subjects_links (
    case_id UUID,
    subject_id UUID,
    is_primary_subject BOOLEAN DEFAULT FALSE
);
