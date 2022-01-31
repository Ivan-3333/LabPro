select *
from AspNetUsers

select *
from company

select *
from Location

select *
from ContactPerson

declare @i int=0

while @i<10000
begin
	insert into company(Name, Address, City, Phone, Email, Created, CreatedBy, LastModified, LastModifiedBy, IsClient, IsImporter, IsTransporter, Status)
	select 'Preduzece '+convert(varchar,@i), Address, City, Phone, Email, Created, CreatedBy, LastModified, LastModifiedBy,IsClient, IsImporter, IsTransporter, 0
	from company
	where id=1

	select @i=@i+1
end

update company set Name='Pretraga zxz' where id =49489


IsClient	IsImporter	IsTransporter
1	1	1



--drop table AspNetUserTokens
--drop table AspNetUserRoles
--drop table AspNetUserClaims
--drop table AspNetUserLogins
--drop table AspNetRoleClaims
--drop table AspNetRoles
--drop table AspNetUsers
--drop table company

select *
from AspNetUsers