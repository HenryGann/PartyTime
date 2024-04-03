# PartyTime

## [DEMONSTRATION](https://youtu.be/GU5oql4W0ow)

## Overview

PartyTime is an ASP.NET REST API. It was built using .NET 8.0 with psql as the SQL server. The main purpose of this project, was to provide a simple backend for an anonymous event hosting forum. Anyone could sign up and create events to be posted. Admins would also be able to monitor the board and prevent abuse of the system.

I focussed on backend because I have done considerable work on frontend development before and implementing an interface for this project would be fairly trivial. Shortly into the development of this project, I realised that this was also largely trivial to implement. This lead to me instead focussing more on the security aspects and how to develop an ASP.NET application rather than learning about backend development. This means that not all of my routes are 'bulletproofed' in terms of implementation, for the simple fact that it would consume time to prove that I can do something I have done many times before.

## Security

PartyTime authentication occurs exclusively over HTTPS, which conceals the payload (containing the plain-text username/password) from network snooping. The `Users` table stores a salt, unique to each user. This is generated when the user is first created by getting a random UUID. This salt is added to the users password and then hashed using SHA-256.

This is because SHA-256 is collision resistant (prevent false positives) and has high cryptographic strength preventing brute force attacks. The reason that we use a salt, is to prevent rainbow table attacks. If we add a random salt to common user passwords, e.g. 'Password123', the pre-computed outputs for common passwords in the rainbow table will not be applicable. It also means that if many users have the same passsword, they will not all be broken at the same time, further increasing computation time.

PartyTime uses a custom implementation of JSON Web Tokens (JWT) as a method for authorisation. While this is somewhat wasted on a monolithic architecture, due to the self-signing properties of JWT, it was done as a learning experience as opposed to the typical session-based authentication. The monolithic architecture was used primarily because this project only has two components: `Users` and `Events`. To break it into micro-services would have been severely overkill.

Additionally, I created a new user in the psql with permissions exclusively over the partytime database. This restricts access to the database from other users and decreases the attack surface.

## API Routes

### UsersController

#### GET /api/Users

- **Description**: Retrieves a list of all users.
- **Method**: `GET`
- **Endpoint**: `/api/Users`
- **Authorization**: Requires admin role.
- **Response**:
  - **Status Code**: 200 OK
  - **Body**: List of user objects.

#### GET /api/Users/id

- **Description**: Retrieves user data by ID.
- **Method**: `GET`
- **Endpoint**: `/api/Users/{id}`
- **Authorization**: Requires admin role.
- **Request Parameters**:
  - `id` (integer): ID of the user to retrieve.
- **Response**:
  - **Status Code**: 200 OK
  - **Body**: User object.
  - **Status Code**: 404 Not Found
  - **Body**: "User not found"

#### POST /api/Users/Login

- **Description**: Logs in a user.
- **Method**: `POST`
- **Endpoint**: `/api/Users/Login`
- **Request Body**:
  - `Username` (string): Username of the user.
  - `Password` (string): Password of the user.
- **Response**:
  - **Status Code**: 200 OK
  - **Body**: Access token.
  - **Status Code**: 404 Not Found
  - **Body**: "User not found"
  - **Status Code**: 401 Unauthorized
  - **Body**: "Incorrect password"

#### POST /api/Users

- **Description**: Creates a new user.
- **Method**: `POST`
- **Endpoint**: `/api/Users`
- **Request Body**:
  - `NewUser` (object): Details of the new user.
- **Response**:
  - **Status Code**: 200 OK

#### DELETE /api/Users/id

- **Description**: Deletes a user by ID.
- **Method**: `DELETE`
- **Endpoint**: `/api/Users/{id}`
- **Request Parameters**:
  - `id` (integer): ID of the user to delete.
- **Response**:
  - **Status Code**: 204 No Content
  - **Status Code**: 404 Not Found

### EventsController

#### GET /api/Events

- **Description**: Retrieves a list of all events.
- **Method**: `GET`
- **Endpoint**: `/api/Events`
- **Response**:
  - **Status Code**: 200 OK
  - **Body**: List of event objects.

#### GET /api/Events/id

- **Description**: Retrieves event data by ID.
- **Method**: `GET`
- **Endpoint**: `/api/Events/{id}`
- **Request Parameters**:
  - `id` (integer): ID of the event to retrieve.
- **Response**:
  - **Status Code**: 200 OK
  - **Body**: Event object.

#### PUT /api/Events/id

- **Description**: Updates an event by ID.
- **Method**: `PUT`
- **Endpoint**: `/api/Events/{id}`
- **Request Body**:
  - `EventDTO` (object): Updated event details.
- **Response**:
  - **Status Code**: 204 No Content
  - **Status Code**: 400 Bad Request
  - **Body**: "Both event ID and event creator cannot be changed"
- **Authorization**: Requires authentication.

#### POST /api/Events

- **Description**: Creates a new event.
- **Method**: `POST`
- **Endpoint**: `/api/Events`
- **Request Body**:
  - `NewEventDTO` (object): Details of the new event.
- **Response**:
  - **Status Code**: 200 OK
  - **Status Code**: 401 Unauthorized
  - **Body**: "Your ID does not match the given username"
- **Authorization**: Requires authentication.

#### DELETE /api/Events/id

- **Description**: Deletes an event by ID.
- **Method**: `DELETE`
- **Endpoint**: `/api/Events/{id}`
- **Request Parameters**:
  - `id` (integer): ID of the event to delete.
- **Response**:
  - **Status Code**: 204 No Content
  - **Status Code**: 404 Not Found
- **Authorization**: Requires authentication.

### Database Schemas

```
partytime=> \d party_time.events
                                      Table "party_time.events"
  Column   |            Type             | Collation | Nullable |              Default
-----------+-----------------------------+-----------+----------+------------------------------------
 id        | integer                     |           | not null | nextval('events_id_seq'::regclass)
 owner     | integer                     |           |          |
 title     | character varying(255)      |           |          |
 summary   | text                        |           |          |
 location  | character varying(255)      |           |          |
 date_time | timestamp without time zone |           |          |
Indexes:
    "events_pkey" PRIMARY KEY, btree (id)
Foreign-key constraints:
    "events_owner_fkey" FOREIGN KEY (owner) REFERENCES users(id)


partytime=> \d party_time.users
                                   Table "party_time.users"
  Column  |          Type          | Collation | Nullable |              Default
----------+------------------------+-----------+----------+-----------------------------------
 id       | integer                |           | not null | nextval('users_id_seq'::regclass)
 username | character varying(50)  |           |          |
 password | character varying(255) |           |          |
 role     | character varying(50)  |           |          |
 salt     | character varying(255) |           |          |
Indexes:
    "users_pkey" PRIMARY KEY, btree (id)
    "users_username_key" UNIQUE CONSTRAINT, btree (username)
Referenced by:
    TABLE "events" CONSTRAINT "events_owner_fkey" FOREIGN KEY (owner) REFERENCES users(id)
```
