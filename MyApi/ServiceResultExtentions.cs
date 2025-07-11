using Services;
using WebFramework.Api;

namespace Kolbeh.Api
{
    /// <summary>
    /// Provides extension methods to convert ServiceResult to ApiResult.
    /// </summary>
    public static class ServiceResultExtentions
    {
        /// <summary>
        /// Converts a ServiceResult to a generic ApiResult.
        /// </summary>
        /// <param name="result">The ServiceResult instance.</param>
        /// <returns>An ApiResult containing status and message.</returns>
        public static ApiResult ToApiResult(this ServiceResult result)
        {
            return new ApiResult(result.IsSuccess, result.StatusCode, result.Message);
        }
        /// <summary>
        /// Converts a ServiceResult with data to a generic ApiResult with data.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="result">The ServiceResult containing data.</param>
        /// <returns>An ApiResult containing data, status, and message.</returns>

        public static ApiResult<TData> ToApiResult<TData>(this ServiceResult<TData> result) where TData : class
        {
            return new ApiResult<TData>(result.IsSuccess, result.StatusCode, result.Data, result.Message);
        }

    }
}
