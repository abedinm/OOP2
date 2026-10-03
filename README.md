# Object Oriented Programming 2 [C]

AIUB coursework, Section C. C# (.NET, Windows Forms) + SQL Server.

## Setup (MacBook, Apple Silicon)

- **Visual Studio 2022 + SSMS** run inside a Windows 11 ARM VM (VMware Fusion).
  This repo is shared into the VM, so projects open straight from here.
- **SQL Server 2022** runs on the Mac in a container (OrbStack):

  ```
  docker compose up -d
  ```

  Password is in `.env` (gitignored, never commit it).

### Connecting from the VM

| Field | Value |
|-------|-------|
| Server | `<Mac IP>,1433`: the Mac's address on VMware's NAT network, i.e. the VM's IPv4 with the last number set to `.1` (VM `192.168.x.128` → `192.168.x.1`). Stable, unlike the Wi-Fi IP. |
| Auth | SQL Server Authentication |
| Login | `sa` |
| Password | from `.env` |
| Options | Trust server certificate |

Connection string for ADO.NET:

```
Server=<Mac IP>,1433;Database=OOP2;User Id=sa;Password=<from .env>;TrustServerCertificate=True;
```

## Layout

One Visual Studio project per lab, all in `OOP2.sln`:

```
Lab-01/   (created from Visual Studio in the VM)
Lab-02/
```

## Labs

| Lab | Topic | Status |
|-----|-------|--------|
| Lab-01 | — | not started |
