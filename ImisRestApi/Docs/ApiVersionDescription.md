## REST services openIMIS ecosystem

### Authentication 

* The REST API uses a JWT authentication.

Every request needs the Authorization HTTP header field:

```
Authorization: Bearer <Token>
```

The Token is obtained by making a POST request to the /api/login route.

If no valid headers are provided in the request, a 401 Unauthorized status code will throw with no response HTTP body.



