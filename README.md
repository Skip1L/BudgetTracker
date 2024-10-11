# Budget Tracker Application

## Overview

*BudgetTracker* is a personal budget tracking web application built using C#, ASP.NET Core, Entity Framework Core, SignalR for real-time notifications, and SQL Server for data storage. This application allows users to manage their budgets, track expenses and income, and receive real-time updates when certain conditions are met (e.g., budget exceeds a limit).
Features

- **User Authentication**: Manage user authentication and authorization using JWT tokens
- **Budget Management**: Create, update, and delete budgets
- **Expense Tracking**: Add, view, update, and delete expenses associated with a budget
- **Income Tracking**: Track income sources related to a budget
- **Category Management**: Organize expenses and income by categories
- **Real-time Notifications**: Receive notifications for budget updates, new expenses, and incomes using SignalR
- **Entity Framework Core**: Manage and interact with the database using EF Core
- **RESTful API**: Expose API endpoints for managing budgets, expenses, income, and categories

---

## Technology Stack

- **Backend**: ASP.NET Core Web API
- **Frontend**: IN PROGRESS!!!
- **Real-Time Communication**: SignalR
- **Database**: Entity Framework Core with SQL Server for database management
- **Authentication**: ASP.NET Identity. JWT Authentication for user security
- **Threads**: Multi-threaded architecture with background services (notifications, data cleanup, etc.)
- **Testing**: Postman for API testing. xUnit and Moq for unit testing

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/downloads/)
  
### Running the Project

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Skip1L/BudgetTracker.git
   cd BudgetTracker
   ```

2. **Setup the Database**:

Update the appsettings.json file in the BudgetTracker.Web project with your SQL Server connection string:
  ```json
  {
    "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=BudgetTrackerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
  }
  ```

3. **Run Database Migrations**: To create the necessary tables and seed data:
   ```bash
   cd BudgetTracker.Web
   dotnet ef database update
   ```

4. **Run the Application**:

  ```bash
  dotnet run
  ```
The application should now be running at https://localhost:xxxx

## API Endpoints
Here are some key API endpoints you can use to interact with the BudgetTracker application:

### Budget Endpoints
#### Create a new budget:
  - POST /api/budget
  - Request Body:
   ```json
    {
      "name": "Monthly Budget",
      "totalAmount": 5000
    }
   ```

#### Get all budgets:
   -GET /api/budget

#### Get a budget by ID:
  -GET /api/budget/{id}

#### Update a budget:
  - PUT /api/budget/{id}
  - Request Body:

    ```json
        {
          "id": 1,
          "name": "Updated Budget",
          "totalAmount": 5500
        }
    ```
#### Delete a budget:
   - DELETE /api/budget/{id}
    

### Expense Endpoints

#### Create a new expense:
   - POST /api/expense
   - Request Body:

  ```json
        {
        "description": "Groceries",
        "amount": 150,
        "budgetId": 1,
        "categoryId": 2
        }
  ```

#### Get all expenses for a budget:
  - GET /api/expense/budget/{budgetId}

#### Get a specific expense by ID:
  - GET /api/expense/{id}

#### Update an expense:
  - PUT /api/expense/{id}
  - Request Body:

  ```json
        {
          "id": 1,
          "description": "Updated Groceries",
          "amount": 200,
          "budgetId": 1,
          "categoryId": 2
        }
  ```
#### Delete an expense:
  - DELETE /api/expense/{id}

### Income Endpoints
#### Create a new income:
  - POST /api/income
  - Request Body:

  ```json
    {
      "source": "Freelance Work",
      "amount": 1000,
      "budgetId": 1,
      "categoryId": 2
    }
  ```
#### Get all incomes for a budget:
  - GET /api/income/budget/{budgetId}

#### Get a specific income by ID:
  - GET /api/income/{id}

#### Update an income:
  - PUT /api/income/{id}
  - Request Body:

  ```json
        {
          "id": 1,
          "source": "Updated Freelance",
          "amount": 1200,
          "budgetId": 1,
          "categoryId": 2
        }
  ```
#### Delete an income:
  -DELETE /api/income/{id}

### Category Endpoints
#### Create a new category:
  - POST /api/category
  - Request Body:
```json
    {
      "name": "Groceries",
      "description": "Expenses related to groceries"
    }
```
#### Get all categories:
  - GET /api/category

#### Get a specific category by ID:
  - GET /api/category/{id}

#### Update a category:
  - PUT /api/category/{id}
  - Request Body:

  ```json

        {
          "id": 1,
          "name": "Updated Groceries",
          "description": "Updated description for groceries"
        }
  ```
#### Delete a category:
  - DELETE /api/category/{id}

## Real-time Notifications

This application uses SignalR to send real-time notifications when specific events occur, such as:

  - A new budget is created.
  - An expense is added or updated.
  - An income is added or updated.

Clients can connect to the SignalR hub at /notificationHub and listen for updates.

## Unit Testing

The application includes unit tests for controllers and services using xUnit and Moq. The tests can be found in the BudgetTracker.Web.Test project.
Running Unit Tests

### To run the tests:
  ```bash
  dotnet test
  ```
