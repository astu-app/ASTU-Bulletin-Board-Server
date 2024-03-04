-- Функция проверяет, включается ли группа пользователей в цикл
-- checking_usergroup_id - id проверяемой группы пользователей
create or replace function is_usergroup_included_in_cycle(checking_usergroup_id uuid)
    returns boolean
as $$
begin
    return (
        select exists(
            with recursive c as (
                select
                    null::uuid as usergroup_id,
                    checking_usergroup_id as child_usergroup_id,
                    array[ checking_usergroup_id ] as track
                union all
                select
                    n.usergroup_id,
                    n.child_usergroup_id,
                    c.track || n.usergroup_id
                from child_usergroups as n
                         join c on n.usergroup_id = c.child_usergroup_id
            ) cycle child_usergroup_id set is_cycle to true default false using cycle_path
            select * from c where is_cycle = true
        )
    );
end;
$$
    language plpgsql;