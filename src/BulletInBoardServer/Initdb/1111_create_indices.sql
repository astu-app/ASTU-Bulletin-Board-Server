begin;

set search_path to public;

create extension pg_trgm;

--  users table
-- create index users_second_name_idx on users using gin (second_name gin_trgm_ops); 

-- announcements_index
create index announcements_published_at on announcements (published_at); -- todo тип индекса?

-- usergroups table
create index usergroups_name_idx on usergroups using gin (name gin_trgm_ops);

-- child_usergroups table
create index child_usergroups_child_usergroup_id on child_usergroups using hash (child_usergroup_id);

-- announcement_categories table
create index announcement_categories_name_idx on announcement_categories using gin (name gin_trgm_ops);

-- questions table
create index questions_survey_id on questions using hash (survey_id);

-- answers table
create index answers_survey_id on answers using hash (question_id);




commit;