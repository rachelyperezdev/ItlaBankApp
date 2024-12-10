# Internet Banking Application - ITLA Bank App

## Overview
This project is an **Internet Banking Application** built with **ASP.NET Core MVC 7**. It includes role-based access for **Administrators** and **Clients**, allowing seamless management of accounts and transactions.

---

## Features

### General Features
- **Role-based Login:**
  - Redirects logged-in users to their respective home pages.
- **User Roles:**
  - **Administrator:** Manages clients, accounts, and views dashboard statistics.
  - **Client:** Manages personal accounts, performs transfers, and views transaction history.

---

### Administrator Features
- **Dashboard:**
  - View total clients, accounts, and transactions.
  - Search clients and export data.
- **Client Management:**
  - Add, edit, or deactivate client accounts.

### Client Features
- **Account Management:**
  - View account balances and details.
  - Perform internal and external transfers.
- **Transaction History:**
  - Access detailed transaction logs with filtering options.

---

## Technologies Used
- **Framework:** ASP.NET Core MVC 7
- **Database:** SQL Server
- **Authentication:** ASP.NET Identity
- **UI Design:** Razor Pages with Bootstrap for responsiveness

---

## Getting Started

Follow these instructions to set up the project locally.

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/) or any preferred IDE for [ASP.NET](http://asp.net/) Core development

### Installation

1. **Clone the repository**:

```bash
git clone https://github.com/rachelyperezdev/ItlaBankApp.git
cd ItlaBankApp
```

1. **Set up the database**:
- Update the `appsettings.json` file with your SQL Server connection string.
- Run the following commands to apply migrations and update the database:

```bash
dotnet ef database update
```

1. **Run the application**:

```bash
dotnet run
```
