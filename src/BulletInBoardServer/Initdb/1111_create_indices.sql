begin;

set search_path to public;

create extension pg_trgm;

--  users table
-- create index users_second_name_idx on users using gin (second_name gin_trgm_ops); 

-- announcements_index
create index announcements_published_at_idx on announcements (published_at); -- todo тип индекса?
create index announcements_is_published_idx on announcements (is_published);

-- child_usergroups table
create index child_usergroups_child_usergroup_id on child_usergroups using hash (child_usergroup_id);

-- questions table
create index questions_survey_id on questions using hash (survey_id);

-- answers table
create index answers_survey_id on answers using hash (question_id);



commit;