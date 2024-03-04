create or replace trigger prevent_cycles_in_usergroups
    after insert or update on child_usergroups
    for each row
    execute procedure can_add_usergroup_or_throw();