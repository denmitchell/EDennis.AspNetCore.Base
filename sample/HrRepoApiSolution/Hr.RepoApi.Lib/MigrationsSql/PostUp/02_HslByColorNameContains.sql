SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HslByColorNameContains] 
	@ColorNameContains varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select * 
		from vwHsl
		where Name Like '%' + @ColorNameContains + '%'
END
