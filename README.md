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

## 🏗️ Architecture

Angular (Frontend)
    ↓
.NET API
    ↓
Service Layer
    ↓
Repository Layer
    ↓
Database

## 🏢 Business Use Case

This project simulates an internal system used by companies to manage customer orders, track order status, and enforce role-based access for operations teams.