# Medical Soft
A simple system for clients / patients registration built on .NET

## Build Status
| Build server| Platform       | Status      |
|-------------|----------------|-------------|
| AppVeyor    | Windows        |[![Build status](https://ci.appveyor.com/api/projects/status/wfcu51b8g9s40avo?svg=true)](https://ci.appveyor.com/project/linuxchata/medical-soft/branch/master) |

## Features
- Clients / patients registration
- Staff / personal registration
- Reminder for staff / personal on clients / patients appointments
- Email notifications
- Scheduler for clients / patients appointments
- Supports of English, Ukrainian and Russian languages of user interface
- Automatic database backup

### Prerequisites
- SQL Server
- Visual Studio 2015

### Steps to run
- Create a database in SQL Server (installation scripts are located in database folder)
- Update the connection string in app.config in src/Client folder
- Build and run the solution (login and password - a)

## Technologies and frameworks used
- WPF
- CommonServiceLocator
- Unity
- SQL Server / EntityFramework
- log4net
- Microsoft Expression Encoder
