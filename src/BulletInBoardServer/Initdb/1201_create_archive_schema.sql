begin;

create schema if not exists archive;
set search_path to archive;



------ create tables
create table announcements
(
    id                 uuid primary key,
    
    author_id          uuid not null,
    
    content            text,
    auto_publishing_at timestamp,
    auto_hiding_at     timestamp,
    
    published_at       timestamp,
    hidden_at          timestamp
);

create table usergroups
(
    id       uuid primary key,
    
    admin_id uuid,
    
    name     text
);

create table announcements_usergroups
(
    announcement_id uuid,
    usergroup_id    uuid,
    
    primary key (announcement_id, usergroup_id)
);

create table child_usergroups
(
    usergroup_id       uuid,
    child_usergroup_id uuid,
    
    primary key (usergroup_id, child_usergroup_id)
);

create table member_rights
(
    user_id      uuid,
    usergroup_id uuid,
    
    primary key (user_id, usergroup_id)
);

create table files
(
    id          uuid primary key,
    
    uploader_id uuid not null,
    
    name        text,
    hash        text,
    
    links_count integer default 0
);

create table announcements_files
(
    announcement_id uuid,
    file_id         uuid,
    
    primary key (announcement_id, file_id)
);

create table announcement_categories
(
    id   uuid primary key,
    name text
);

create table announcements_announcement_categories
(
    announcement_id          uuid,
    announcement_category_id uuid,
    
    primary key (announcement_id, announcement_category_id)
);

create table surveys
(
    id                         uuid primary key,
    
    announcement_id            uuid    not null,
    
    is_open                    boolean not null default true,
    is_anonymous               boolean not null default false,
    is_multiple_choice_allowed boolean not null default false,
    
    auto_closing_at            timestamp
);

create table questions
(
    id        uuid primary key,
    
    survey_id uuid not null,
    
    content   text
);

create table answers
(
    id           uuid primary key,
    
    question_id  uuid not null,
    
    content      text,
    voters_count integer default 0
);



commit;