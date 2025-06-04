using Services;
using WebFramework.Api;

namespace Kolbeh.Api
{
    public static class ServiceResultExtentions
    {
        public static ApiResult ToApiResult(this ServiceResult result)
        {
            return new ApiResult(result.IsSuccess, result.StatusCode, result.Message);
        }

        public static ApiResult<TData> ToApiResult<TData>(this ServiceResult<TData> result) where TData : class
        {
            return new ApiResult<TData>(result.IsSuccess, result.StatusCode, result.Data, result.Message);
        }

    }
}
