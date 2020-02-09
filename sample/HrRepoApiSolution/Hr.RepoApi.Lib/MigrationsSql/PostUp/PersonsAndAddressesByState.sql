SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PersonsAndAddressesByState] 
	@State varchar(2)
AS
BEGIN
	SET NOCOUNT ON;

declare @json varchar(max) = 
(
	select 
		p.Id, 
		p.FirstName, 
		p.LastName,
		(select 
			a.Id, a.StreetAddress, a.City, a.State, a.PostalCode
			from Address a
			where p.Id = a.Id
			for json path) Addresses
		from Person p
		where exists (
			select 0 from Address a
			where State = 'CA'
				and a.Id = p.Id )
		for json path
)
select @json [json]

END
GO

