# 🏢 Enterprise Order Management System

A full-stack enterprise-style application built with Angular and .NET, simulating a real-world order processing system with authentication, role-based access, and layered architecture.

## ✨ Features

- User authentication (JWT)
- Role-based access (Admin/User)
- Create and manage orders
- RESTful API design

## ⚙️ Tech Stack

- Frontend: Angular
- Backend: .NET Web API
- Auth: JWT Authentication
- Architecture: Layered (Controller → Service → Repository)

## 📊 Sample API Endpoints

POST /api/auth/login  
POST /api/orders  
GET /api/orders  
GET /api/orders/{id}

## 🧪 Sample Request

POST /api/orders

{
  "customerName": "John Doe",
  "product": "Laptop",
  "quantity": 1,
  "price": 50000
}

## 📦 Sample Response

{
  "id": 1,
  "status": "Pending",
  "createdAt": "2026-05-01T10:00:00Z"
}

## 🏢 Business Use Case

This project simulates an internal system used by companies to manage customer orders, track order status, and enforce role-based access for operations teams.

## 🏗 Architecture

This system follows a simple 3-layer architecture:

Frontend (Angular)
→ Handles UI and user interactions

Backend (ASP.NET Core REST API)
→ Handles business logic and API processing

Database (MySQL)
→ Stores orders, customers, products, and transactions

## 🔄 System Flow

1. User interacts with Angular frontend  
2. Frontend sends HTTP requests to .NET REST API  
3. Backend processes business logic  
4. Data is stored/retrieved from MySQL database  
5. Response is returned to frontend

## 💼 What This Project Demonstrates

- Full-stack application design
- REST API development using .NET
- Angular frontend integration
- Database design using MySQL
- Separation of concerns (frontend / backend / data layer)