namespace Common.Exceptions;

public class GoneException : AppException
{
    public GoneException()
        : base(ApiResultStatusCode.BadRequest, "این صفحه 410 شده، یعنی رفته که رفته! شاید رفت دنبال نون، شاید هم اینترنتش قطع شده! 😄", System.Net.HttpStatusCode.Gone)
    {
    }

    public GoneException(string message)
        : base(ApiResultStatusCode.BadRequest, message, System.Net.HttpStatusCode.Gone)
    {
    }

    public GoneException(object additionalData)
        : base(ApiResultStatusCode.BadRequest, null, System.Net.HttpStatusCode.Gone, additionalData)
    {
    }

    public GoneException(string message, object additionalData)
        : base(ApiResultStatusCode.BadRequest, message, System.Net.HttpStatusCode.Gone, additionalData)
    {
    }

    public GoneException(string message, Exception exception)
        : base(ApiResultStatusCode.BadRequest, message, exception, System.Net.HttpStatusCode.Gone)
    {
    }

    public GoneException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.BadRequest, message, System.Net.HttpStatusCode.Gone, exception, additionalData)
    {
    }
}
