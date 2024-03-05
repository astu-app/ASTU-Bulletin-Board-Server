begin;

create schema if not exists public;
set search_path to public;



------ create auxiliary functions
---- common
create function string_not_empty(str text)
    returns boolean 
as 
$$
begin
    return not (str is null or trim(str) = '');
end;
$$ language plpgsql;

---- announcements
create function can_set_auto_publishing_moment(auto_publishing_at timestamp, published_at timestamp)
    returns boolean
as
$$
begin
    return published_at is null or auto_publishing_at is null or auto_publishing_at < published_at;
end;
$$ language plpgsql;

create function can_set_auto_hiding_moment(auto_hiding_at timestamp, hidden_at timestamp)
    returns boolean
as
$$
begin
    return hidden_at is null or auto_hiding_at is null or auto_hiding_at < hidden_at;
end;
$$ language plpgsql;

create function can_set_published_and_hidden_moments(published_at timestamp, hidden_at timestamp)
    returns boolean
as
$$
begin
    return published_at is null or hidden_at is null or published_at < hidden_at;
end
$$ language plpgsql;

----
create function is_color_format_correct(color_hex text)
    returns boolean
as
$$
begin
    return color_hex ~* '#[0-9a-fA-F]{6}';
end
$$ language plpgsql;


------ create tables
create table announcements
(
    id                         uuid primary key,
    
    author_id                  uuid not null,
    audience_size              int  not null,
    
    content                    text,
    
    delayed_publishing_at      timestamp,
    expects_delayed_publishing boolean generated always as ( delayed_publishing_at is not null ) stored,
    delayed_hiding_at          timestamp,
    expects_delayed_hiding     boolean generated always as ( delayed_hiding_at is not null ) stored,
    
    published_at               timestamp,
    is_published               bool generated always as ( published_at is not null ) stored,
    hidden_at                  timestamp,
    is_hidden                  bool generated always as ( hidden_at is not null ) stored
    
    constraint non_negative_audience_size
        check (audience_size >= 0),
    
    constraint non_empty_content
        check (string_not_empty(content)),
    
    constraint set_auto_publishing_moment_to_already_published_announcement
        check (can_set_auto_publishing_moment(delayed_publishing_at, published_at)),
    
    constraint set_auto_hiding_moment_to_already_hidden_announcement
        check (can_set_auto_hiding_moment(delayed_hiding_at, hidden_at)),
    
    constraint set_published_and_hidden_moments
        check (can_set_published_and_hidden_moments(published_at, hidden_at))
);

create table users
(
    id          uuid primary key,

    first_name  text,
    second_name text,
    patronymic  text,
    
    constraint non_empty_first_name
        check (string_not_empty(first_name)),
    
    constraint non_empty_second_name
        check (string_not_empty(second_name))
);

create table usergroups
(
    id       uuid primary key,

    admin_id uuid,

    name     text,
    
    constraint non_empty_name
        check (string_not_empty(name))
);

create table announcement_audience
(
    announcement_id uuid,
    user_id         uuid,
    viewed          boolean,
    
    primary key (announcement_id, user_id)
);

create table member_rights
(
    user_id      uuid,
    usergroup_id uuid,

    primary key (user_id, usergroup_id)
);

create table child_usergroups
(
    usergroup_id       uuid,
    child_usergroup_id uuid,

    primary key (usergroup_id, child_usergroup_id)
);

create table files
(
    id          uuid primary key,
    
    uploader_id uuid not null,

    name        text,
    hash        text,

    links_count integer default 0,
    
    constraint non_empty_name
        check (string_not_empty(name)),
    
    constraint non_empty_hash
        check (string_not_empty(hash))
);

create table attachments
(
    id   uuid primary key,
    type text,
    
    constraint non_empty_type
        check (string_not_empty(type))
);

create table announcements_attachments
(
    attachment_id   uuid,
    announcement_id uuid,

    primary key (attachment_id, announcement_id)
);

create table announcement_categories
(
    id        uuid primary key,
    name      text,
    color_hex text,
    
    constraint non_empty_name
        check (string_not_empty(name)),
    constraint non_empty_color
        check (is_color_format_correct(color_hex))
);

create table announcement_categories_subscribers
(
    announcement_category_id uuid,
    subscriber_id            uuid,

    primary key (announcement_category_id, subscriber_id)
);

create table announcements_announcement_categories
(
    announcement_id          uuid,
    announcement_category_id uuid,

    primary key (announcement_id, announcement_category_id)
);

create table surveys
(
    id                   uuid primary key,
    
    voters_count         integer          default 0,
    
    is_open              boolean not null default true,
    is_anonymous         boolean not null default true,
    
    auto_closing_at      timestamp,
    expects_auto_closing boolean generated always as ( auto_closing_at is not null ) stored
);

create table questions
(
    id                         uuid primary key,
    
    survey_id                  uuid not null,

    content                    text,
    is_multiple_choice_allowed boolean not null default true,
    
    constraint non_empty_content
        check (string_not_empty(content))
);

create table answers
(
    id           uuid primary key,

    question_id  uuid not null,

    content      text,
    voters_count integer default 0,
    
    constraint non_empty_content
        check (string_not_empty(content))
);

create table participation
(
    id        uuid primary key,
    
    user_id   uuid not null,
    survey_id uuid  not null,
    
    constraint user_can_vote_only_once
        unique (user_id, survey_id)
);

create table user_selections
(
    participation_id uuid,
    answer_id        uuid,
    
    primary key (participation_id, answer_id)
);


-- create foreign keys
alter table announcements
    add constraint announcements_author_id_fkey
        foreign key (author_id) references users (id)
            on delete cascade on update cascade;

alter table usergroups
    add constraint usergroups_admin_id_fkey
        foreign key (admin_id) references users (id)
            on update cascade on delete set null;

alter table member_rights
    add constraint member_rights_usergroup_id_fkey
        foreign key (usergroup_id) references usergroups (id)
            on update cascade on delete cascade,
    add constraint member_rights_user_id_fkey
        foreign key (user_id) references users (id)
            on update cascade on delete cascade;

alter table child_usergroups
    add constraint child_usergroups_usergroup_id_fkey
        foreign key (usergroup_id) references usergroups(id)
            on update cascade on delete cascade,
    add constraint child_usergroup_child_usergroup_id_fkey
        foreign key (child_usergroup_id) references usergroups(id)
            on update cascade on delete cascade;

alter table announcement_audience
    add constraint announcement_audience_announcement_id_fkey
        foreign key (announcement_id) references announcements (id)
            on update cascade on delete cascade,
    add constraint announcement_audience_user_id_fkey
        foreign key (user_id) references users (id)
            on update cascade on delete cascade;

alter table files
    add constraint files_uploader_id_fkey
        foreign key (uploader_id) references users (id)
            on update cascade on delete cascade,
    add constraint files_attachments_id_fkey
        foreign key (id) references attachments
            on update cascade on delete cascade;

alter table announcements_attachments
    add constraint announcements_attachments_attachment_id_fkey
        foreign key (attachment_id) references attachments (id)
            on update cascade on delete cascade,
    add constraint announcements_attachments_announcement_id_fkey
        foreign key (announcement_id) references announcements (id)
            on update cascade on delete cascade;

alter table announcement_categories_subscribers
    add constraint categories_subscribers_category_id_fkey
        foreign key (announcement_category_id) references announcement_categories (id)
            on update cascade on delete cascade,
    add constraint categories_subscribers_subscriber_id_fkey
        foreign key (subscriber_id) references users (id)
            on update cascade on delete cascade; 

alter table announcements_announcement_categories
    add constraint announcements_categories_announcement_id_fkey
        foreign key (announcement_id) references announcements (id)
            on update cascade on delete cascade,
    add constraint announcements_categories_category_id_fkey
        foreign key (announcement_category_id) references announcement_categories (id)
            on update cascade on delete cascade;

alter table surveys
    add constraint surveys_id_fkey
        foreign key (id) references attachments (id)
            on update cascade on delete cascade;

alter table questions
    add constraint questions_survey_id_fkey
        foreign key (survey_id) references surveys (id)
            on update cascade on delete cascade;

alter table answers
    add constraint answers_question_id_fkey
        foreign key (question_id) references questions (id)
            on update cascade on delete cascade;

alter table participation
    add constraint participation_survey_id_fkey
        foreign key (survey_id) references surveys (id)
            on update cascade on delete cascade,
    add constraint participation_user_id_fkey
        foreign key (user_id) references users (id)
            on update cascade on delete cascade;

alter table user_selections
    add constraint user_selections_participation_id_fkey
        foreign key (participation_id) references participation (id)
            on update cascade on delete cascade,
    add constraint participation_answer_id_fkey
        foreign key (answer_id) references answers (id)
            on update cascade on delete cascade;



commit;