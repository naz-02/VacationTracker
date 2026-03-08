# Vacation Tracker - Province de Berkane 🇲🇦🌴

A modern, offline-first native Windows application built with **.NET 8 MAUI** and **Blazor Hybrid** designed to streamline and automate the management of employee vacations for the Provincial Council of Berkane.

## 🚀 Features

* **Admin Dashboard:** Centralized view of pending vacation requests, total employee count, and 2026 official Moroccan transport/religious holidays.
* **Intelligent Holiday Calculation:** Automatically calculates the exact number of deduction days by skipping weekends and recognizing official Moroccan holidays (e.g., Aïd el-Fitr, Marche Verte) within the selected date ranges.
* **Dynamic UI Alerts:** Instantly notifies employees when they select dates that intersect with official non-deductible public holidays.
* **Responsive Blazor UI:** Clean, modern interface fully stripped of unnecessary web baggage (no popups, no redundant navigation elements). Built natively for Windows.
* **Offline Database Setup:** Powered by an embedded SQLite database (`VacationTracker.db`) utilizing Entity Framework Core. Automatically seeds essential generic staff data on first run. 
* **Native Windows Iconography:** Compiles natively into a standalone `.exe` utilizing standard Windows `.ico` protocols for desktop shortcuts.

## 🛠️ Tech Stack

* **Framework:** .NET 8 MAUI (Multi-platform App UI)
* **UI Architecture:** Blazor WebAssembly / Razor Components
* **Database:** SQLite (local embedded storage)
* **Styling:** Bootstrap + Custom Native CSS
* **Build Targets:** strictly `net8.0-windows10.0.19041.0` 

## ⚙️ Getting Started

### Prerequisites
* Windows 10/11
* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Visual Studio 2022 (with MAUI workload) or VS Code with C# Dev Kit.

### Installation & Run

1. Clone the repository:
   ```bash
   git clone https://github.com/naz-02/VacationTracker.git
   cd VacationTracker
   ```
2. Build the Windows executable:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run --project VacationTracker.csproj -f net8.0-windows10.0.19041.0
   ```

## 🔒 Privacy & Data Anonymization
> **Note:** This repository is intended for portfolio demonstration purposes. All real employee names and PII (Personally Identifiable Information) originally present in the database seeding logic have been thoroughly scrubbed and replaced with generic dummy data (e.g., `Employé 01`, `Ingénieur en Chef`). The database files themselves are also rigorously `.gitignore`'d.

## 📄 License
This project is proprietary and built specifically for the internal use of the Provincial Council of Berkane. Public distribution is for demonstration only.
