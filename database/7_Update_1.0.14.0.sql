-- =========================================================
-- Author:			LinuxChata
-- Created date:	02.11.2013
-- Modified date:	02.11.2013
-- Description:		Perform update 1.0.14.0 of the database.
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
DECLARE @int_param int;

BEGIN TRY
	BEGIN TRANSACTION [Update10140Transaction]
		-- Perform script version 1.0.14.0.
		SET @update_old_version = '1.0.13.2';
		SET @update_old_version_int = 10132;
		SET @update_version = '1.0.14.0';
		SET @update_version_int = 10140;
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
									('Update e-mail notification configuration.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		SET @setting_key = N'EmailNotificationSendingDelaySec'
		SET @int_param = 20;

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[intValue])
					VALUES
						(@setting_key
						,@int_param)
			END

	COMMIT TRANSACTION [Update10140Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
    PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10140Transaction]
END CATCH