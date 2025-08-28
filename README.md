# OBSD in C# — Interactive Examples

**Purpose**  
This repository contains interactive sample codes for the **Object-Based Software Development (OBSD)** methodology implemented in **C#**.  
OBSD is related to but*distinct from classic OOP; the examples highlight OBSD’s modeling style and coding patterns.

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

---

### Try it: write code in `Program.cs`

Open the console app project and edit `Program.cs`:

```csharp
// Program.cs (top-level statements)
using System;
// using Obsd.Core; // example library

Console.WriteLine("OBSD sandbox ready.");

// Write temporary experiments here:
// var result = MyObsdService.DoSomething();
// Console.WriteLine(result);
```

Run again:

```bash
dotnet run --project ./apps/Sandbox
```

---

## Using Visual Studio (Windows)

1. Open the `.sln` file.
2. In **Solution Explorer**, right-click the console app → **Set as Startup Project**.
3. Open `Program.cs`, type your test code.
4. Press **F5** (Debug) or **Ctrl+F5** (Run).

**Terminal inside VS (optional):**

```powershell
dotnet run --project .\apps\Sandbox
```

---

## Using VS Code

1. Open the repo folder in VS Code.
2. Install extensions: **C#** (and optionally **C# Dev Kit**).
3. VS Code will prompt to restore/build; accept.
4. Edit `Program.cs` in the console app.
5. Use **Run and Debug** (F5) or the terminal:

   ```bash
   dotnet run --project ./apps/Sandbox
   ```

---

## Using JetBrains Rider

1. **Open** the solution folder.
2. Set the console app as **Startup Project**.
3. Edit `Program.cs`.
4. Click **Run** ▶ or press **Shift+F10**.

---

## Common Tasks

* **Add a new library**

  ```bash
  dotnet new classlib -n Obsd.Extras -o ./libs/Obsd.Extras
  dotnet sln add ./libs/Obsd.Extras/Obsd.Extras.csproj
  dotnet add ./apps/Sandbox reference ./libs/Obsd.Extras/Obsd.Extras.csproj
  ```
* **Add a NuGet package to the console app**

  ```bash
  dotnet add ./apps/Sandbox package <PackageId>
  ```

---

## Troubleshooting

* **SDK mismatch**: run `dotnet --info` and install the version in `global.json` (if present).
* **Restore issues**: `dotnet nuget locals all --clear` then `dotnet restore`.
* **Startup project path**: ensure the `dotnet run --project` path matches your console app’s actual folder.

---

## License

Apache License 2.0 (see `LICENSE`).
