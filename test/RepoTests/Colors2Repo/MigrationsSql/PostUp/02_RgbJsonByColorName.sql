SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RgbJsonByColorName] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	declare @json varchar(max) = 
	(
		select Red, Green, Blue
			from Rgb
			where Name = @ColorName
			for json path, without_array_wrapper
	)
	select @json [json]
END
GO


