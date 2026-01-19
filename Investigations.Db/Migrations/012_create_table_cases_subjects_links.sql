CREATE TABLE IF NOT EXISTS cases_subjects_links (
    case_subject_link_key INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 100),
    case_key INT NOT NULL,
    subject_key INT NOT NULL,
    is_primary_subject BOOLEAN DEFAULT FALSE
);
