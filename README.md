# Room Booking Application

A web-based room booking system built with ASP.NET Core MVC that allows users to reserve meeting rooms and administrators to manage room inventory.

## Features

- **User Authentication & Authorization**: Secure login system with role-based access control (Admin/User roles)
- **Room Management**: Administrators can add, view, and delete meeting rooms
- **Booking System**: Users can book available rooms for specific time slots
- **In-Memory Repository**: Fast data storage for development and testing

## Technologies Used

- **Framework**: ASP.NET Core 9.0 MVC
- **Authentication**: Cookie-based authentication
- **Validation**: FluentResults for functional error handling
- **UI**: Razor Views with Bootstrap
- **Repository Pattern**: In-memory data storage

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- A code editor (Visual Studio, Visual Studio Code, or JetBrains Rider)

## Getting Started

### Installation

1. Clone the repository:
```bash
git clone https://github.com/peterprospl12/room-booking-app.git
cd room-booking-app
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

5. Open your browser and navigate to:
```
https://localhost:5001
```

### Initial Data Setup

To populate the application with sample users and rooms, you need to:

1. Log in as an administrator
2. Navigate to `/Home/Init?confirm=yes-i-am-sure`

This will create sample users and rooms:

**Sample Users** (all with password: `password123`):
- jan.kowalski@example.com
- anna.nowak@example.com
- zbyszek.papka@example.com
- patryk.gdynia@example.com

**Sample Rooms**:
- Sala Konferencyjna A (Capacity: 20)
- Sala Szkoleniowa B (Capacity: 15)
- Sala Warsztatowa C (Capacity: 10)
- Sala Executive D (Capacity: 8)

## Project Structure

```
room-booking-app/
├── Controllers/        # MVC Controllers
│   ├── AccountController.cs
│   ├── BookingController.cs
│   ├── HomeController.cs
│   └── RoomController.cs
├── Models/            # Data models
│   ├── Booking.cs
│   ├── Room.cs
│   ├── User.cs
│   └── DTOs/          # Data Transfer Objects
├── Services/          # Business logic layer
│   ├── BookingService.cs
│   ├── RoomService.cs
│   └── UserService.cs
├── Repositories/      # Data access layer
│   ├── IRepository.cs
│   └── InMemoryRepository.cs
├── ViewModels/        # View-specific models
├── Views/             # Razor view templates
├── wwwroot/           # Static files (CSS, JS, images)
└── Program.cs         # Application entry point
```

## Usage

### For Users

1. **Login**: Access the application and log in with your credentials
2. **View Rooms**: Browse available meeting rooms
3. **Book a Room**: Select a room and choose your desired time slot
4. **Manage Bookings**: View and manage your reservations

### For Administrators

1. **Login**: Log in with administrator credentials
2. **Manage Rooms**: Navigate to Room Management to add or delete rooms
3. **View All Bookings**: Access the complete booking overview

## Configuration

Application settings can be modified in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Security

- Passwords are hashed using SHA-256
- Cookie-based authentication with configurable expiration (24 hours default)
- Role-based authorization for administrative functions
- HTTPS redirection in production environments

## Development

### Running in Development Mode

```bash
dotnet run --environment Development
```

### Building for Production

```bash
dotnet publish -c Release -o ./publish
```

## License

This project is available for educational purposes.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For issues, questions, or contributions, please open an issue on the GitHub repository.
