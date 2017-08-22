-- =========================================================
-- Author:			LinuxChata
-- Created date:	01.09.2017
-- Modified date:	01.09.2017
-- Description:		Perform update X.X.XX.X of the database.
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

BEGIN TRY
	BEGIN TRANSACTION [Update10130Transaction]
		-- Perform script version 1.0.13.0.
		SET @update_old_version = '1.0.12.2';
		SET @update_old_version_int = 10122;
		SET @update_version = '1.0.13.0';
		SET @update_version_int = 10130;
		SET @update_date = GETDATE();

		IF EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_version + ' already exist in the database.';
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
									('Reason of update',
									@update_version,
									@update_version_int,
								 	@update_date);

				PRINT 'SystemUpdates table was updated.';
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.';
			END

	COMMIT TRANSACTION [Update10130Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10130Transaction]
END CATCH