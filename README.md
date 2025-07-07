<<<<<<< HEAD
# Race-Server
Race App Server
=======

# C# Race Manager System


---

## 📁 Project Structure

```
csharp/
├── MPP-Server/         # Main C# server application (business logic, DB, networking, REST)
│   ├── model/          # Data models (Participant, Race, User, etc.)
│   ├── network/        # Networking logic (TCP, Sockets, Protocol)
│   ├── repo/           # Repository layer (database access)
│   ├── server/         # Server main logic
│   ├── service/        # Business services
│   ├── database.db     # Database (SQLite sample/template)
│   ├── App.config      # Database/configuration settings
│   ├── MPP-Server.csproj
│   └── ...             # bin/, obj/, etc.
├── Rest-Test/          # C# REST API test utility
│   ├── Program.cs      # Main entry point
│   └── Rest-Test.csproj
```

---

## 🚦 Overview

**MPP-Server** is a modular C# application for managing races and participants, featuring:
- Layered architecture: Model, Repository, Service, Network
- Relational DB persistence (e.g., SQLite via ADO.NET)
- Multi-client networking with sockets
- REST API support for CRUD operations
- Easily extensible for new requirements

**Rest-Test** is a simple C# project to test the REST API endpoints exposed by the backend.

---

## 🏗️ Architecture

- **Model Layer:**  
  Core entities like `Participant`, `Race`, `User`, typically inheriting from a base entity.

- **Repository Layer:**  
  Handles data persistence, database access, CRUD for all major entities.

- **Service Layer:**  
  Business logic and orchestration between the repository and other layers.

- **Network Layer:**  
  Implements server sockets, communication protocol, and multi-client handling.

- **REST API:**  
  Exposes endpoints for CRUD on key entities, tested via the Rest-Test project.

---


## 🌱 Branching & Workflow Suggestions

- `main` – stable, tested code for both server and test client.
- `feature/server-extensions` – for new server features or assignments.
- `feature/rest-test` – for REST API test improvements.

---

## 🚀 Assignment Coverage

- **Assignment 1–5:**  
  - C# server with DB, services, networking
- **Assignment 6:**  
  - REST endpoints and test utility
- **Assignment 7:**  
  - Ready for web client (React, etc.)

---

## 📝 Authors

- Mihalcea Andrei Cristian — AI, UBB FMI 2025

---

## 📜 License

For academic use only.
>>>>>>> bfd4db1 (Full Project Server)
