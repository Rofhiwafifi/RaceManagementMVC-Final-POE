# RaceManagementMVC
RaceManagementMVC
A web-based race management system built with ASP.NET Core MVC and
.NET 8. The application provides a web interface for managing race
events, categories, participant enrolments, user profiles and race
results. It communicates with a separate backend API for data
operations.RaceManagementMVC

The project uses the Model-View-Controller (MVC) pattern:
- Models describe the data used by the application.
- Views display pages and forms to users.
- Controllers receive requests, call the API service and return
  the appropriate views.
- apiService centralises HTTP communication with the backend API.
The MVC application and backend API are separate applications. The MVC
project depends on the API being available at the configured base URL.
