SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RgbHex] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	declare @i table (RgbInt int)
	insert into @i exec RgbInt @ColorName
	declare @RgbInt int = (select * from @i)

	select 
		substring(
			upper(sys.fn_varbintohexsubstring(0, @RgbInt,1,0))
			,3,6) RgbHex
	from rgb
		where Name = @ColorName
END
