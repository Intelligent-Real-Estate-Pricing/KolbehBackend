namespace Common.Exceptions
{
    public class OtpExpiredException : AppException
    {

        public OtpExpiredException()
         : base(ApiResultStatusCode.ExpireCode, System.Net.HttpStatusCode.Forbidden)
        {
        }

        public OtpExpiredException(string message)
            : base(ApiResultStatusCode.ExpireCode, message, System.Net.HttpStatusCode.Forbidden)
        {
        }
    }
}
