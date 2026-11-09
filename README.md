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

Main Features
The project contains pages and controller actions for the following
functionality:
- Account access: user registration, login and logout.
- Event management: list events, view event details, create
  events, edit events and delete events.
- Category management: view, create, edit and delete event
  categories.
- Participant enrolments: display enrolments, enrol a participant
  in an event, view personal enrolments and cancel an enrolment.
- Results: view results, create results, publish results and view
  a participant's own results.
- User profiles: view and update profile information and upload a
  profile picture.
- Dashboards: separate organiser and participant dashboard pages.
- Form validation: model validation attributes and client-side
  validation support.

User Roles
The application uses user role information, including:
- Organiser: intended to manage events, categories, enrolments and
  results.
- Participant: intended to browse events, enrol and view personal
  enrolments and results.
The user's role and authentication information are stored in the session
after a successful login. The backend API remains responsible for
validating protected API requests.
