SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HslJsonByColorNameContains] 
	@ColorNameContains varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	declare @json varchar(max) = 
	(
		select Id, Name, Hue, Saturation, Luminance 
		from vwHsl
		where Name like '%' + @ColorNameContains + '%'
			for json path
	)
	select @json [json]
END
GO


