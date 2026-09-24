# ⚡ FitFlow - Gym Management System

A Modern, Luxury Dark-Themed Gym Management System built with **C# .NET 8 WinForms**, **Guna UI 2**, and **SQLite**.

---

## ✨ Features

- 💎 **Modern Dark SaaS UI**: Inspired by premier SaaS dashboards, custom gradient cards, metrics, and vector icons.
- 🏋️‍♂️ **Comprehensive Member Management**:
  - Auto-generated member ID (`MEM-0001`, `MEM-0002`, etc.) for instant lookup and search.
  - Privacy-preserving phone number masking (`0100****89`).
  - Full CRUD operations with instant confirmation and cascade deletion.
- 🔄 **Subscriptions & Renewals**:
  - Package plans management with dynamic durations and prices.
  - Active, Expired, and Suspended badge indicators.
- 💳 **Payments & Thermal Receipt Printing**:
  - Instant 80mm thermal receipt generator (`Receipt Print`).
  - Barcode representation, amount paid, payment method, and remaining balance.
  - Fallback printable document with zero external printer dependency.
- 📊 **One-Click Table Export (Excel / PDF)**:
  - Instant CSV export with **UTF-8 BOM** for 100% Arabic text compatibility in Microsoft Excel.
  - High-resolution Landscape PDF/Printable Reports for revenue breakdown, memberships, and expenses.
- ⏱️ **Attendance & Check-In Tracking**:
  - Fast member search & single-click check-in/check-out.
- 📈 **Financial & Operational Analytics**:
  - Live revenue, expenses, net profit, and membership package market share.
- 🔔 **Renewal Reminders & Alerts**:
  - Automatic detection of expiring subscriptions within 14 days and WhatsApp messaging integration.
- 🛡️ **Embedded Assets**:
  - High-res embedded application icon (`dumbbell.ico`), hero branding, and background images directly inside the executable without requiring external file dependencies.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11

### Build & Run
```bash
# Clone the repository
git clone https://github.com/XREFS0/gym-management-system.git
cd gym-management-system

# Build the project
dotnet build GymManagement/GymManagement.csproj

# Run the application
dotnet run --project GymManagement/GymManagement.csproj
```

---

## 🛠️ Tech Stack
- **Framework**: C# .NET 8 (Windows Forms)
- **UI Toolkit**: Guna.UI2.WinForms
- **Database**: SQLite (Microsoft.Data.Sqlite)
- **Icons & Graphics**: Pure Vector Drawing2D + Embedded High-Resolution Resources
