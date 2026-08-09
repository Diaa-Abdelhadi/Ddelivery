Ddelivery – Food Delivery Backend API
Ddelivery is a full-featured ASP.NET Core Web API for a multi-role food delivery platform. It provides secure authentication, restaurant and menu management, cart and order handling with a full delivery lifecycle, live order tracking, driver earnings, reviews, localization, and role-based user management.
This project follows N-tire architecture principles (DAL / BLL / PL) and modern backend best practices.

🚀 Features

🔐 Authentication & Authorization
* JWT authentication (access + refresh tokens)
* Refresh tokens
* Email confirmation
* Reset password (code-based)
* Role-based access control (Customer, Restaurant Owner, Driver, Admin)
* Block / unblock users
* Change user roles

🧑‍💻 User Management (Admin)
* List all users with their roles and block status
* Block / unblock accounts
* Change a user's role

🏪 Restaurants
* Owners create and update their restaurants (main image upload, translations)
* Admins list all restaurants and toggle active/inactive status
* Public browsing with pagination, search, and sorting
* Public restaurant details

📋 Menu Categories
* Owners create, update, delete, and toggle category status
* Public browsing of categories per restaurant

🍔 Meals
* Owners create and update meals (main image + multiple sub-images, price, discount, stock)
* Toggle meal availability
* Public browsing with pagination, search, category filter, and sorting
* Meal details including customer reviews
* Meal translations (localization support)

⭐ Reviews
* Customers can review a meal only after it has been ordered and delivered
* Duplicate reviews on the same meal are blocked
* Reviews shown on meal details with the reviewer's name

🛒 Cart
* Add meals to cart
* Get cart with per-item and total pricing
* Update item quantity
* Remove item from cart
* Clear cart

📦 Orders & Checkout
* Checkout converts a cart into an order with atomic, race-condition-safe stock decrement
* Full order state machine: Pending → Accepted → Preparing → On The Way → Delivered (or Cancelled), with invalid transitions rejected
* Restaurant owners accept, prepare, or cancel orders
* Drivers view available orders, claim one for delivery, and mark it delivered
* Customers view their order history and order details
* Live driver location tracking

📡 Real-Time Notifications (SignalR)
* Live order status updates pushed to connected clients
* JWT-authenticated WebSocket connections

⏱️ Background Jobs
* Automatic cancellation of abandoned/stale pending orders
* Scheduled daily earnings calculation

💰 Earnings
* Restaurant owners view daily earnings per restaurant
* Drivers view their daily delivery earnings
* Admins can trigger earnings calculation on demand

🌍 Localization
* Multi-language support (EN / AR)
* Language switching via query string

🧰 Technical Features
* Repository pattern
* Mapster for object mapping, with language-aware translation mapping
* Data annotation validation
* Global exception handling middleware
* CORS policy configuration
* Seed data (roles + test users)
* Audit fields (created/updated by & at)
* Pagination across list endpoints
* Atomic, concurrency-safe stock management

🧱 Tech Stack
* ASP.NET Core 9 Web API
* Entity Framework Core 9
* SQL Server
* ASP.NET Identity
* JWT Authentication
* SignalR
* Mapster
* Scalar (API documentation & testing)

⚙️ Local Setup
1. Copy `Ddelivery.PL/appsettings.Development.json.example` to `Ddelivery.PL/appsettings.Development.json`
2. Fill in your own values: a random JWT signing key, and a Mailtrap sandbox API token / inbox ID (or swap in your own email provider)
3. Run the EF Core migrations against your local SQL Server / LocalDB instance
4. `dotnet run --project Ddelivery.PL`
