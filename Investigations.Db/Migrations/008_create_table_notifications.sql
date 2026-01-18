CREATE TABLE IF NOT EXISTS notifications (
    notification_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    subject VARCHAR(100) NOT NULL,
    description TEXT,
    assigned_to_user_id UUID,
    is_delayed BOOLEAN NOT NULL DEFAULT FALSE,
    delayed_until_datetime TIMESTAMPTZ,
    inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now(),
    inserted_by_user_id UUID,
    updated_datetime TIMESTAMPTZ,
    updated_by_user_id UUID
);
