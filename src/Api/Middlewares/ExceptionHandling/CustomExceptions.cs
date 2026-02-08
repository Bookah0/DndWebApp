namespace Api.Middlewares.ExceptionHandling;

public class CustomException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public class NotFoundException(string message) : CustomException(message, 404) { }

public class ValidationException(string message) : CustomException(message, 400) { }    

public class ConflictException(string message) : CustomException(message, 409) { }

public class UnauthorizedException(string message) : CustomException(message, 401) { }  

