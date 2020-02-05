SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RgbJsonByColorNameContains] 
	@ColorNameContains varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	declare @json varchar(max) = 
	(
		select Id, Name, Red, Green, Blue
			from Rgb
			where Name like '%' +  @ColorNameContains + '%'
			for json path
	)
	select @json [json]
END
GO


