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

Application Architecture
The general request flow is:
1. A user opens a page or submits a form in the browser.
2. ASP.NET Core routes the request to the relevant controller action.
3. The controller validates the input and uses apiService when
   backend data is required.
4. apiService sends an HTTP request to the configured backend API.
5. The API returns a response, normally containing JSON data.
6. The controller processes the response and returns a Razor view.
7. The view displays the result to the user.
For example, when a participant opens the event list, EventsController
requests the event data through apiService and passes the returned
events to the Events/Index.cshtml view.

Configuration and secrets
Do not commit real passwords, private API keys or production credentials
to source control. Use development user secrets or environment variables
for sensitive configuration. The API base URL is configuration, and
should be changed to match the environment where the API is running.

Running the Application
Option 1: Visual Studio
1. Extract or open the RaceManagementMVC solution.
2. Open RaceManagementMVC.sln in Visual Studio.
3. Make sure the backend API is running.
4. Confirm that ApiSettings:BaseUrl in appsettings.json points to
   the API.
5. Set RaceManagementMVC as the startup project.
6. Run the project using the HTTPS profile.
7. Open the local URL shown by Visual Studio.

Typical User Workflows
Register and log in
1. Open the registration page.
2. Enter the required account details and select a role.
3. Submit the registration form.
4. Open the login page and enter the account credentials.
5. After a successful login, the application stores the returned user
   information and token in the session.

Organiser manages an event
1. Log in as an organiser.
2. Open the organiser dashboard.
3. Open the events section.
4. View existing events or create a new event.
5. Edit or delete an event when required.
6. Manage the categories and results associated with race events.


