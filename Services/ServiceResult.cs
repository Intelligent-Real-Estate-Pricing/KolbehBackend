using Common;

namespace Services
{
    public class ServiceResult
    {

        public ServiceResult(bool isSuccess, string message, ApiResultStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }



        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }

        public static ServiceResult Ok(string message)
        {
            return new ServiceResult(true, message, ApiResultStatusCode.Success);
        }
        public static ServiceResult Ok()
        {
            return Ok("Operation Success");
        }
        //public static ServiceResult Fail(string message, ApiResultStatusCode statusCode)
        //{
        //    return new ServiceResult(false, message, statusCode);
        //}

        public static ServiceResult ServerError()
        {
            return new ServiceResult(false, "Internal Server Error", ApiResultStatusCode.ServerError);
        }

        public static ServiceResult NotFound(string message)
        {
            return new ServiceResult(false, message, ApiResultStatusCode.NotFound);
        }

        public static ServiceResult<TData> NotFound<TData>(string message) where TData : class
        {
            return new ServiceResult<TData>(false, message, ApiResultStatusCode.NotFound, null);
        }

        public static ServiceResult ExpiredCode(string message)
        {
            return new ServiceResult(false, message, ApiResultStatusCode.ExpiredCode);
        }
        public static ServiceResult BadRequest(string message)
        {
            return new ServiceResult(false, message, ApiResultStatusCode.BadRequest);
        }
        public static ServiceResult<TData> BadRequest<TData>(string message) where TData : class
        {
            return new ServiceResult<TData>(false, message, ApiResultStatusCode.BadRequest, null);
        }
        public static ServiceResult<TData> Fail<TData>(TData data, string messsage, ApiResultStatusCode statusCode) where TData : class
        {
            return new ServiceResult<TData>(false, messsage, statusCode, data);
        }
        public static ServiceResult<TData> Fail<TData>(TData data) where TData : class
        {
            return new ServiceResult<TData>(false, "Fail", ApiResultStatusCode.BadRequest, data);
        }
        public static ServiceResult<TData> Ok<TData>(TData data, string message) where TData : class
        {
            return new ServiceResult<TData>(true, message, ApiResultStatusCode.Success, data);
        }
        public static ServiceResult<TData> Ok<TData>(TData data) where TData : class
        {
            return new ServiceResult<TData>(true, "Operation Success", ApiResultStatusCode.Success, data);
        }
    }

    public class ServiceResult<TData> : ServiceResult where TData : class
    {
        public ServiceResult(bool isSuccess, string message, ApiResultStatusCode statusCode, TData data) : base(isSuccess, message, statusCode)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}
