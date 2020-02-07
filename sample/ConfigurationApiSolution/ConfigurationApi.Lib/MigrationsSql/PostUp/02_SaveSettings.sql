SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveSettings] 
	@projectName varchar(255),
	@tvpSettings dbo.SettingTableType readonly
AS
BEGIN
	SET NOCOUNT ON;

	declare @projectId int

	select top 1 @projectId = Id from Project where ProjectName = @projectName;

	if @projectId is null
	begin
		insert into Project (ProjectName) values (@projectName)
		select top 1 @projectId = Id from Project where ProjectName = @projectName;
	end

	--add new setting keys
	insert into Setting(SettingKey) 
		select distinct SettingKey 
			from @tvpSettings t
			where not exists (
				select 0 
					from Setting s
					where s.SettingKey = t.SettingKey
			)


	--delete all existing config settings for the app
	delete from ProjectSetting
		where ProjectId = @projectId;


	--insert the new config settings
	insert into ProjectConfigSettings(ProjectId, SettingId, SettingValue)
		select @projectId, s.Id, ps.SettingValue
			from @tvpSettings ps
			inner join Setting s
				on s.SettingKey = ps.SettingKey

END