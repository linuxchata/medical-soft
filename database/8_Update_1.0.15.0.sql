-- =========================================================
-- Author:			LinuxChata
-- Created date:	13.11.2013
-- Modified date:	13.11.2013
-- Description:		Perform update 1.0.15.0 of the database.
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

DECLARE @setting_key nvarchar(100);
DECLARE @param nvarchar(100);

BEGIN TRY
	BEGIN TRANSACTION [Update10140Transaction]
		-- Perform script version 1.0.15.0.
		SET @update_old_version = '1.0.14.0';
		SET @update_old_version_int = 10140;
		SET @update_version = '1.0.15.0';
		SET @update_version_int = 10150;
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
									('Update smtp e-mail configuration.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		SET @setting_key = N'SmtpUserName'
		SET @param = 'AKIAJOW2RXFSNKSPYFRQ';

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE [nvKey] = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[nvValue])
					VALUES
						(@setting_key
						,@param)
			END

		UPDATE [Setting] SET [intValue] = 1 WHERE [nvKey] = 'EmailNotificationSendingDelaySec';

		UPDATE [Setting] SET [nvValue] = 'company@gmail.com' WHERE [nvKey] = 'SmtpFromAddress';

		UPDATE [Setting] SET [nvValue] = '' WHERE [nvKey] = 'SmtpUserName';

		UPDATE [Setting] SET [nvValue] = '' WHERE [nvKey] = 'SmtpHost';

		UPDATE [Setting] SET [intValue] = 2525 WHERE [nvKey] = 'SmtpPort';

		UPDATE [Setting] SET [nvValue] = '' WHERE [nvKey] = 'SmtpPassword';

	COMMIT TRANSACTION [Update10140Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10140Transaction]
END CATCH