set sqlserver="LINUXSERVER\SQLEXPRESS"

sqlcmd -S %sqlserver% -i 1_InstallDatabase.sql
sqlcmd -S %sqlserver% -i 2_CreateDatabaseTables.sql
sqlcmd -S %sqlserver% -i 3_DatabaseInitialization.sql
sqlcmd -S %sqlserver% -i 4_StoredProcedures.sql
sqlcmd -S %sqlserver% -i 5_Update_1.0.12.0.sql
sqlcmd -S %sqlserver% -i 6_Update_1.0.13.0.sql
sqlcmd -S %sqlserver% -i 7_Update_1.0.14.0.sql
sqlcmd -S %sqlserver% -i 8_Update_1.0.15.0.sql
sqlcmd -S %sqlserver% -i 9_Update_1.0.16.0.sql
sqlcmd -S %sqlserver% -i 10_Update_1.0.17.0.sql
sqlcmd -S %sqlserver% -i 11_Update_1.1.20.0.sql
sqlcmd -S %sqlserver% -i 12_Update_1.2.25.0.sql

pause