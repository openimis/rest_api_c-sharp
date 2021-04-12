## REST services openIMIS ecosystem

### Authentication 

* The REST API uses a JWT authentication.
* Currently, only the /api/login route can be accessed without authentication.

Every request needs the Authorization HTTP header field:

```
Authorization: Bearer <Token>
```

The Token is obtained by making a POST request to the /api/login route.

If no valid headers are provided in the request, a 401 Unauthorized status code will throw with no response HTTP body.

<!---
E.g. for the Admin/Admin user on a GET request:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiRW5yb2xsbWVudE9mZmljZXIiLCJNYW5hZ2VyIiwiQWNjb3VudGFudCIsIkNsZXJrIiwiTWVkaWNhbE9mZmljZXIiLCJTY2hlbWVBZG1pbiIsIklNSVNBZG1pbiIsIlJlY2VwdGlvbmlzdCIsIkNsYWltQWRtaW4iLCJDbGFpbUNvbnRyaWIiXSwiZXhwIjoxNTMzMzk3NzIxLCJpc3MiOiJodHRwOi8vb3BlbmltaXMub3JnIiwiYXVkIjoiaHR0cDovL29wZW5pbWlzLm9yZyJ9.GGyWzZk-SZqU2hL5FfFwT20_IYGb2zvTuRXJS1qOAmU
```
-->

