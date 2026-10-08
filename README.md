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
```

## Labs

| Lab | Topic | Status |
|-----|-------|--------|
| Lab-01 | — | project created (empty Form1) |
| SmokeTest | toolchain check: WinForms + SQL Server | done |
