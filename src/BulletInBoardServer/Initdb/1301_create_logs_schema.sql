begin;

create schema if not exists logs;
set search_path to logs;



-- create tables
create table announcements_logs
(
    operation_time  timestamp not null,
    operation_type  text      not null,
    
    announcement_id uuid      not null,
    user_id         uuid      not null,
    
    details         jsonb
);



commit;