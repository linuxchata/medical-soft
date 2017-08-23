-- =============================================
-- Author:		LinuxChata
-- Create date:	02.12.2012
-- Description:	Stored procedure to perform backup of the database
-- =============================================

USE [dentist]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BackUpOfTheDataBase]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_BackUpOfTheDataBase];
END
GO

CREATE PROCEDURE [dbo].[sp_BackUpOfTheDataBase]
	@databasename NVARCHAR(100),		--dentist
	@pathtothedatabase NVARCHAR(300)	--N'D:\dentist_back.bak'
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BACKUP DATABASE @databasename TO DISK = @pathtothedatabase WITH NOFORMAT, INIT, NAME = N'DentistFullDatabaseBackup', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
		SELECT 'Database was backuped successfully.';
	END TRY
	BEGIN CATCH
		SELECT 'Database backup failed.';
	END CATCH; 
END

PRINT 'sp_BackUpOfTheDataBase has been created';
GO