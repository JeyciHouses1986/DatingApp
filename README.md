Dating App Documentation
Overview
The Dating App is a full-stack application designed to facilitate interactions and connections between users. It is built using Angular for the frontend and ASP.NET Core for the backend, with a focus on providing a seamless and responsive user experience. The application features user authentication, profile management, messaging, and real-time presence tracking.

Project Structure
The project is divided into two main parts: the client-side (Angular application) and the server-side (ASP.NET Core API).

Client-Side (Angular)
Located under the client directory, the Angular application handles the user interface and client-side logic. Key files and directories include:

src/index.html: The entry point of the Angular application.
src/app/app.component.ts: The root component of the Angular application, responsible for initializing the app.
src/app/app.component.html: The HTML template associated with the root component.
src/app/app.component.css: The styles for the root component.
src/app/_services/account.service.ts: A service for managing account-related operations such as login and registration.


Server-Side (ASP.NET Core API)
Located under the API directory, the ASP.NET Core application provides RESTful APIs for the Angular application. Key files and directories include:

Program.cs: The entry point of the ASP.NET Core application, configuring services and the request pipeline.
Controllers: Contains API controllers that handle HTTP requests for different functionalities like accounts, messages, and user profiles.
Data: Includes the DataContext class for database access and repositories for handling business logic.
Entities: Contains classes that represent the database entities.
Services: Includes services like TokenService for JWT token generation and PhotoService for handling photo uploads.


Configuration
Deployment Configuration
The application is configured for deployment using Fly.io, as indicated by the fly.toml and fly1.toml files. These files specify the application name, region, environment variables, and VM settings.

Environment Variables
Key environment variables include:

ASPNETCORE_URLS: Configures the URLs the application listens on.
CloudinarySettings__ApiKey and CloudinarySettings__CloudName: Used for integrating with Cloudinary for photo storage.


CORS Policy
The application is configured to allow cross-origin requests from https://localhost:4200, facilitating communication between the Angular frontend and ASP.NET Core backend during development.

Features
User Authentication
The application supports user authentication, including registration, login, and JWT token generation for securing API endpoints.

Profile Management
Users can create and update their profiles, including uploading photos and managing personal information.

Messaging
The application supports real-time messaging between users, leveraging SignalR for real-time web functionality.

Real-Time Presence Tracking
Using SignalR, the application tracks and displays user presence in real-time, enhancing the interactive experience.

Development and Deployment
For local development, the Angular application runs on localhost:4200, and the ASP.NET Core API runs as configured in the Program.cs file. For deployment, the application uses Fly.io, with configuration specified in the fly.toml files.

Testing
The Angular application includes a basic test setup in app.component.spec.ts, verifying the title of the root component.

Conclusion
The Dating App is a comprehensive solution for online dating, featuring a robust set of functionalities ranging from user authentication to real-time interactions. Its architecture, utilizing Angular and ASP.NET Core, ensures a scalable and maintainable codebase, suitable for further development and enhancements.
