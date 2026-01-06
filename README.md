# FKNISystem

## 📖 Description
**FKNISystem** is a full-featured web-based e-commerce system developed in **C#** with **SQL Server** as the database engine.  
The system was created to support the online sales operations of the clothing brand **Fucking Industry**, specifically focused on t-shirt sales.

It provides both customer-facing and administrative functionalities, covering the complete purchase flow from product browsing to order management.

---

## 🛠 Technologies
- **Backend:** C# – ASP.NET (Web Application)
- **Database:** Microsoft SQL Server
- **Architecture:** Clean Architecture
  - Application
  - Infrastructure
  - Web
- **Authentication:** User registration and login system
- **Version Control:** Git & GitHub

---

## 🧱 Project Architecture
The project follows a layered architecture to ensure scalability, maintainability, and separation of concerns:

FKNISystem
│
-
├── Application
-
│ └── Business logic, use cases, and services
│
├── Infrastructure
│ └── Database access, repositories, and external services
│
└── Web
└── User interface, controllers, and presentation layer


---

## ✨ Main Features

### 🛍 Product Catalog
- Product listing with detailed information
- Product detail view including:
  - Description
  - Price
  - Promotions
  - User ratings and reviews

### 🔥 Promotions
- List of active promotions
- Administrative management of promotions (create, update, delete)

### 🛒 Shopping Cart
- Add and remove products
- Update quantities
- Cart execution generates an order

### 📦 Orders
- Order creation from shopping cart
- Order listing for logged-in users
- Order detail view

### ⭐ Reviews & Ratings
- Logged-in users can leave reviews and ratings on products
- Product rating displayed on the product detail page

### 👤 User Management & Authentication
- User registration
- Login and authentication
- Session-based access to protected features

### 🧑‍💼 Administration
- Product creation and maintenance
- Promotion creation and maintenance
- Secure access to management features

---

## 🎯 Purpose of the Project
This project was developed to:
- Apply full-stack web development concepts using **C#**
- Implement a real-world e-commerce workflow
- Practice clean architecture principles
- Integrate authentication, database management, and business logic in a single system

---

## 🚀 Future Improvements
- Payment gateway integration
- Advanced user roles and permissions
- Order status tracking
- Improved UI/UX design
- API exposure for mobile or external integrations

---

## 👨‍💻 Author
**Moisés Castro**  
Software Development Engineering Student  
Passionate about building scalable and well-structured web systems

---
