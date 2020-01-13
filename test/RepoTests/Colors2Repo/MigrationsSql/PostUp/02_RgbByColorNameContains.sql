SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RgbByColorNameContains] 
	@ColorNameContains varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select * 
		from Rgb
		where Name Like '%' + @ColorNameContains + '%'
END
