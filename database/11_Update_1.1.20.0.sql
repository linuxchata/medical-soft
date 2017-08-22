-- =========================================================
-- Author:			LinuxChata
-- Created date:	02.11.2014
-- Modified date:	30.11.2014
-- Description:		Perform update 1.1.20.0 of the database.
-- =========================================================
USE [dentist]

SET NOCOUNT ON
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

DECLARE @update_old_version nvarchar(255);
DECLARE @update_version nvarchar(255);
DECLARE @update_old_version_int int;
DECLARE @current_version_int int;
DECLARE @update_version_int int;
DECLARE @update_date datetime;

DECLARE @startHtml nvarchar(512);
DECLARE @endHtml nvarchar(512);

BEGIN TRY
	BEGIN TRANSACTION [Update11200Transaction]
		-- Perform script version 1.1.20.0.
		SET @update_old_version = '1.0.17.2';
		SET @update_old_version_int = 10172;
		SET @update_version = '1.1.20.0';
		SET @update_version_int = 11200;
		SET @update_date = GETDATE();

		IF EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_version + ' already exist in the database.'
				RaisError('ERROR: Script version %s already exist in the database. Update was aborted.', 15, 1, @update_version);
			END

		SET @current_version_int = (SELECT TOP 1 [UpdateVersionInt] FROM [SystemUpdates] ORDER BY [ID] DESC);
		IF (@current_version_int != @update_old_version_int)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_old_version + ' has to be applied first.';
				RaisError(N'ERROR: Script version %s has to be applied first. Update was aborted.', 15, 1, @update_old_version);
			END

		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
							([UpdateInformation],
							[UpdateVersion],
							[UpdateVersionInt],
							[UpdateDate])
						VALUES
							('Update template string to the HTML.',
							@update_version,
							@update_version_int,
							@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END
			
		IF(NOT EXISTS(SELECT * FROM sys.columns WHERE name = N'Id' AND OBJECT_ID = OBJECT_ID(N'Setting')))
		BEGIN
			PRINT 'Add column Id to the Setting table.';
			ALTER TABLE [Setting] ADD [Id] INT NULL;
		END

		SET @startHtml = '&lt;html&gt;&lt;body&gt;&lt;div style=&quot;text-align:Left;font-family:Segoe UI;font-style:normal;font-weight:normal;font-size:12;color:#000000;&quot;&gt;&lt;p&gt;';

		SET @endHtml = '&lt;/p&gt;&lt;/div&gt;&lt;/body&gt;&lt;/html&gt;';

		UPDATE NotificationTemplate SET Body = @startHtml + Body + @endHtml;

	COMMIT TRANSACTION [Update11200Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update11200Transaction]
END CATCH

GO