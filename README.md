# OBSD in C# — Interactive Examples

**Purpose**  
This repository contains **interactive sample codes** for the **Object-Based Software Development (OBSD)** methodology implemented in **C#**.  
OBSD is related to but **distinct from classic OOP**; the examples highlight OBSD’s modeling style and coding patterns.

## Repository Layout
- **Solution**: one `.sln` at the root
- **Projects**:  
  - **Console app** — a *sandbox* to type and run code quickly  
  - **Multiple class libraries** — reusable OBSD components used by the console app

> You can write quick experiments in the console app’s `Program.cs` and reference the libraries directly.

---

## Prerequisites
- **.NET SDK** 8.0 or later (`dotnet --info`)
- Git
- One of: **Visual Studio**, **VS Code**, or **JetBrains Rider**

---

## Quick Start (CLI)
```bash
# 1) Clone
git clone <YOUR-REPO-URL>
cd <repo-folder>

# 2) Restore & build
dotnet restore
dotnet build -c Debug

# 3) Run the console sandbox (adjust the path if different)
dotnet run --project ./apps/Sandbox   # or ./src/ConsoleApp or similar
