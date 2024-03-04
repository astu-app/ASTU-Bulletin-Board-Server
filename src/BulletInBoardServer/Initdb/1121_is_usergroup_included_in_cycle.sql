-- Функция проверяет, можно ли добавить группу пользователей в таблицу child_usergroups, или кидает исключение
create or replace function can_add_usergroup_or_throw() 
    returns trigger
as $$
    declare 
        usergroup_included_in_cycle boolean;
        usergroup_id uuid;
begin
    usergroup_id = new.usergroup_id;
    raise notice 'usergroup_id %', usergroup_id;
    
    usergroup_included_in_cycle = is_usergroup_included_in_cycle(usergroup_id);
    if (usergroup_included_in_cycle) then
        raise exception 'Группа пользователей % создает цикл', usergroup_id;
    end if;
    
    return new;
end 
$$ language plpgsql;