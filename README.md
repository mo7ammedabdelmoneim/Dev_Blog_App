# DevBlog App MVC

A modern **ASP.NET MVC Blog Platform** built with a clean, scalable architecture and real-world features.
The project focuses on **separation of concerns, and practical usage of ASP.NET Identity** with a rich admin dashboard and interactive UI.


![PROJECT](Interface/wwwroot/uploads/LandingPage.png)
---

## Overview

**devBlog** is a full-featured blogging platform designed for developers.
It supports role-based access control, post management, real-time interactions, and detailed analytics for admins.

The project is suitable for:

* Learning **ASP.NET MVC** in a real-world scenario
* Understanding **authentication, authorization, and roles**
* Building dashboards and statistics-driven applications

---

## Key Features

### Authentication & Authorization

* ASP.NET Identity
* Google Authentication
* Secure password hashing
* Role-based access control:

  * **Admin**
  * **Manage_Posts**
  * **User**

---

### Post Management

* Create, Edit, Delete posts
* Slug-based post URLs
* Featured posts
* Cover images support
* Post pagination

---

### Tags & Categories

* Assign multiple tags to posts
* Category-based filtering
* View posts by tag or category

---

### Comments System

* Add comments to posts
* Display post discussions

---

### Reactions (Likes)

* Like / Unlike posts using AJAX
* Real-time update of reactions count

---

### Admin Dashboard

* Charts powered by **Chart.js**
* User statistics and monthly growth tracking
* Latest posts and newly registered users
* Most reacted post
* Most active category
* Role-based user count
* Average posts per user

---

### User Management (Admin)

* View all users
* Edit user roles
* Delete users

---

### UI & UX

* Responsive and modern UI
* Built with **TailwindCSS**
* Clean card-based blog design
* TempData alerts and validation messages
* Real-time UI updates with JavaScript

---

## Tech Stack

* ASP.NET MVC (.NET)
* Entity Framework Core
* ASP.NET Identity
* SQL Server
* TailwindCSS
* Chart.js
* JavaScript / AJAX

---

## Setup Instructions

1. Clone the repository:

```bash
git clone https://github.com/your-username/devBlog.git
```

2. Update the connection string in `appsettings.json`

3. Apply migrations:

```bash
dotnet ef database update
```

4. Run the project:

```bash
dotnet run
```

5. Login using seeded admin credentials (check seed data)

---

## Future Improvements

* SignalR for real-time comments
* Full-text search
* Post drafts & scheduling
* API layer for mobile apps
---

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---

## Contact

For any questions or suggestions, please reach out to [mo7ammeda7medabdelmoneim@gmail.com](mailto:your-email@example.com)

---

## PROJECT DEMO

![Click to access project demo video](Interface/wwwroot/uploads/VN20260108_171755.mp4)
<video src="Interface/wwwroot/uploads/VN20260108_171755.mp4" controls></video>
