-- =========================================================
-- Author:			LinuxChata
-- Created date:	08.12.2013
-- Modified date:	08.12.2013
-- Description:		Perform update 1.0.16.0 of the database.
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
	BEGIN TRANSACTION UpdateTransaction10161
		-- Perform script version 1.0.16.1.
		SET @update_old_version = '1.0.15.0';
		SET @update_old_version_int = 10150;
		SET @update_version = '1.0.16.1';
		SET @update_version_int = 10161;
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

		IF (NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'IsEmailChecked' AND OBJECT_ID = OBJECT_ID(N'Patient')))
		BEGIN
			ALTER TABLE [Patient] ADD [IsEmailChecked] BIT NULL;
			ALTER TABLE [Patient] ADD CONSTRAINT DF_IsEmailChecked_Patient DEFAULT 0 FOR [IsEmailChecked];
			PRINT 'Column IsEmailChecked has been added to the table Patient.'
		END

		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
									([UpdateInformation],
									[UpdateVersion],
									[UpdateVersionInt],
									[UpdateDate])
							 VALUES
									('Improve e-mail delivery.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

	COMMIT TRANSACTION UpdateTransaction10161
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION UpdateTransaction10161
END CATCH

GO

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
	BEGIN TRANSACTION UpdateTransaction10162
		-- Perform script version 1.0.16.2.
		SET @update_old_version = '1.0.16.1';
		SET @update_old_version_int = 10161;
		SET @update_version = '1.0.16.2';
		SET @update_version_int = 10162;
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

		IF (EXISTS (SELECT * FROM sys.columns WHERE name = N'IsEmailChecked' AND OBJECT_ID = OBJECT_ID(N'Patient')))
		BEGIN
			UPDATE [Patient] SET [IsEmailChecked] = 0 WHERE ([Email] IS NULL OR [Email] = '');
			UPDATE [Patient] SET [IsEmailChecked] = 1 WHERE ([Email] IS NOT NULL AND [Email] != '');
			ALTER TABLE [Patient] ALTER COLUMN [IsEmailChecked] BIT NOT NULL;
			PRINT 'Column IsEmailChecked has been updated.'
		END

		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
									([UpdateInformation],
									[UpdateVersion],
									[UpdateVersionInt],
									[UpdateDate])
							 VALUES
									('Improve e-mail delivery.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

	COMMIT TRANSACTION UpdateTransaction10162
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION UpdateTransaction10162
END CATCH