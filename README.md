# Object Oriented Programming 2 [C]

AIUB coursework, Section C. C# (.NET, Windows Forms) + SQL Server.

## Setup (MacBook, Apple Silicon)

- **Visual Studio 2022 + SSMS** run inside a Windows 11 ARM VM (VMware Fusion).
  This repo is mapped in the VM as drive **Z:** (macOS File Sharing over SMB,
  `\\172.16.37.1\minhazulabedin\Developer\OOP2`). Fusion's own shared folders
  don't work for Windows on Apple silicon.
- **SQL Server 2022** runs on the Mac in a container (OrbStack):

  ```
  docker compose up -d
  ```

  Password is in `.env` (gitignored, never commit it).

### Connecting from the VM

| Field | Value |
|-------|-------|
| Server | `172.16.37.1,1433` (the Mac on VMware's NAT network; stable, unlike the Wi-Fi IP) |
| Auth | SQL Server Authentication |
| Login | `sa` |
| Password | from `.env` |
| Options | Trust server certificate |

Connection string for ADO.NET:

```
Server=172.16.37.1,1433;Database=OOP2;User Id=sa;Password=<from .env>;TrustServerCertificate=True;
```

## Layout

One Visual Studio project per lab, all in `OOP2.sln`:

```
Lab-01/      Windows Forms App (.NET 8)
SmokeTest/   checks WinForms + SQL Server end to end
SenseAssist/ accessible companion app (see below)
SimpleLoginUI/ simple sign-in screen built with the WinForms designer
AnimalInheritance/ console demo of inheritance (Animal -> Dog, Cat, Bird)
course-materials/  instructor's lecture slides (weeks 1-3) and example code
```

## Labs

| Lab | Topic | Status |
|-----|-------|--------|
| Lab-01 | — | project created (empty Form1) |
| SmokeTest | toolchain check: WinForms + SQL Server | done |
| SenseAssist | accessible app for blind / hard-of-hearing users | built |
| SimpleLoginUI | simple UI/UX sign-in form (designer-based) | built |
| AnimalInheritance | inheritance: base class, override, base(), polymorphism | built |

## SenseAssist

A Windows Forms app for people who are blind or have low vision and/or use hearing aids.

- **Accounts:** register, sign in, log out (PBKDF2-hashed passwords).
- **Reminders and Contacts:** add, edit, save, delete, search, mark done.
- **Blind / low vision:** every screen, field, row and alert is spoken (System.Speech);
  fully keyboard-driven (F1 help, Ctrl+1..4 pages, Ctrl+N/E, Delete, Ctrl+R read, Esc stop);
  high-contrast and large-text modes; works with screen readers (accessible names everywhere).
- **Hard of hearing:** live caption bar shows everything that is spoken; due reminders
  flash the caption bar and the taskbar button.
- **Logging:** activity log in the database (viewable/clearable in the app) plus a daily
  log file in `%LOCALAPPDATA%\SenseAssist\logs`.
- **Data:** SQL Server database `SenseAssist` (created automatically on first run).

Structure: `Models/` (Entity base class → Reminder, Contact), `Data/` (`IRepository<T>`,
repositories, ActivityLogger), `Services/` (Auth, Speech, Settings, AppLog), `UI/` (theme,
custom controls, forms, pages; `CrudPage<T>` is the shared add/edit/delete screen).

Run in the VM: `cd /d Z:\SenseAssist && dotnet run`, or set it as the startup project in Visual Studio.
