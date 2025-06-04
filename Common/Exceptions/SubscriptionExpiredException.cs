namespace Common.Exceptions
{
    public class SubscriptionExpiredException : AppException
    {

        public SubscriptionExpiredException()
         : base(ApiResultStatusCode.ExpiredPlan, System.Net.HttpStatusCode.Forbidden)
        {
        }

        public SubscriptionExpiredException(string message)
            : base(ApiResultStatusCode.ExpiredPlan, message, System.Net.HttpStatusCode.Forbidden)
        {
        }
    }
}
