//namespace Services.Services.ServiceResults;

//public class ServiceResult<TData> : IServiceResult<TData>
//{
//    public TData Data { get; set; }
//    public string ErrorMessage { get; set; }
//    public bool IsSuccess { get; set; }

//    public IServiceResult<TData> Failure(string errorMessage)
//    {
//        ErrorMessage = errorMessage;
//        IsSuccess = false;
//        return this;
//    }

//    public IServiceResult<TData> Ok(TData data)
//    {
//        Data = data;
//        IsSuccess = true;
//        return this;
//    }

//    public IServiceResult<TData> OkFalse(string errorMessage)
//    {
//        ErrorMessage = errorMessage;
//        IsSuccess = false;
//        return this;
//    }
//}
//public class ServiceResult : IServiceResult
//{
//    public string ErrorMessage { get; set; }
//    public bool IsSuccess { get; set; }

//    public IServiceResult Failure(string errorMessage)
//    {
//        ErrorMessage = errorMessage;
//        IsSuccess = false;
//        return this;
//    }

//    public IServiceResult Ok()
//    {
//        IsSuccess = true;
//        return this;
//    }
//}
