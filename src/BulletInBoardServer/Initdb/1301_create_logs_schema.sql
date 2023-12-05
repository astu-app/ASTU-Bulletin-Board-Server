begin;

create schema if not exists logs;
set search_path to logs;



-- create tables
create table announcements_logs
(
    operation_time     timestamp not null,
    operation_type     text      not null,
    
    announcement_id    uuid      not null,
    user_id            uuid      not null,
    
    updated_attributes jsonb
);

create table files_logs
(
    operation_time timestamp not null,
    operation_type text      not null,
    
    file_id        uuid      not null,
    user_id        uuid      not null
);

create table usergroups_logs
(
    operation_time     timestamp not null,
    operation_type     text      not null,
    
    usergroup_id       uuid      not null,
    user_id            uuid      not null,
    
    updated_attributes jsonb
);

create table announcement_categories_logs
(
    operation_time     timestamp not null,
    operation_type     text      not null,
    
    category_id        uuid      not null,
    user_id            uuid      not null,
    
    updated_attributes jsonb
);



commit;