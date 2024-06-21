begin;

set search_path to public;

create extension pg_trgm;

-- announcements_index
create index announcements_firstly_published_at_idx on announcements (firstly_published_at);
create index announcements_published_at_idx on announcements (published_at);
create index announcements_is_published_idx on announcements (is_published);
create index announcements_author_id_idx on announcements using hash (author_id);

-- announcement_audience
create index announcement_audience_announcement_id_idx on announcement_audience using hash (announcement_id);

-- child_usergroups table
create index child_usergroups_child_usergroup_id_idx on child_usergroups using hash (child_usergroup_id);

-- participation table
create index participation_survey_id_idx on participation using hash (survey_id);

-- questions table
create index questions_survey_id_idx on questions using hash (survey_id);

-- answers table
create index answers_question_id_idx on answers using hash (question_id);



commit;