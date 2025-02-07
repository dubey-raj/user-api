using UserService.Model;

namespace UserService.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Register a new customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Message)> RegisterUserAsync(RegisterCustomerRequest request);

        /// <summary>
        /// Update the customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Message)> UpdateUserAsync(UpdateCustomerRequest request);

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User details, null if passed credentials are invalid</returns>
        UserResponse GetUserByIdAsync(int id);
    }
}
