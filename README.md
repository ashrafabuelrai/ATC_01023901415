# EventBookingSystem
# Admin Acount
# username => alaagad
# password => Alaa123##
# 🎫 Event Booking System – ASP.NET MVC & Web API

### Developed by: **El-Mohandes Ashraf**

This is a fully-featured **Event Booking System** built using ASP.NET Web API and ASP.NET MVC, structured with Clean Architecture. It includes support for **Arabic & English**, **dark mode**, **role-based access**, and **automatic translation** from Arabic to English.

---

## ✅ Features Implemented

- ✔️ Clean Architecture structure across Domain, Application, Infrastructure, API, and MVC.
- ✔️ RESTful Web API for managing events, categories, and bookings.
- ✔️ Frontend built using ASP.NET MVC that consumes the Web API using `HttpClient`.
- ✔️ Full support for Arabic & English using `.resx` resource files.
- ✔️ RTL support for Arabic layouts automatically.
- ✔️ Dark mode toggle with preference stored in `localStorage`.
- ✔️ Responsive and modern UI with Bootstrap 5.
- ✔️ Admin interface to add events in Arabic.
- ✔️ Automatic translation to English using **LibreTranslate** (free API).
- ✔️ Stored bilingual content in the database (Title/Description in both languages).
- ✔️ Event details, booking form, and booking history implemented.
- ✔️ Language and theme preferences persisted across sessions.
- ✔️ Role-based authorization using ASP.NET Identity.
- ✔️ Consistent API responses using `ApiResponse<T>` wrapper.
- ✔️ Event/category tagging and image upload supported.
- ✔️ Pagination implemented for event lists.
- ✔️ Fully seeded sample data (categories and events).
- ✔️ Login & Register pages styled with custom blue theme.

---

## 🧱 Project Structure (Clean Architecture)


---

## 🌐 Localization

- Language switch (Arabic / English) available in the navbar.
- Culture is stored in a cookie and applied globally.
- All UI content is localized via `.resx` resource files.

---

## 🌙 Dark Mode

- Toggle switch in the top navbar.
- Theme preference saved in `localStorage`.
- Applies instantly across pages.

---
## Technologies Used
ASP.NET Core 8.0

ASP.NET MVC

Entity Framework Core

Bootstrap 5

LibreTranslate (Open-source translation API)

SQL Server

Identity (Roles / Login)

Tailwind (optionally used in some frontend mockups)
