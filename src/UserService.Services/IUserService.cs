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

        /// <summary>
        /// Get List of user based on availability
        /// </summary>
        /// <param name="region">region inpsectory belongs to</param>
        /// <param name="limit">number of records needs to be returned, default is 1</param>
        /// <returns></returns>
        Task<List<AvailableUserResponse>> GetAvailableUserAsync(string role, string region, int limit=1);

        /// <summary>
        /// Update the assigned cases to user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Message)> UpdateUserAssignmentAsync(UpdateAssignedCaseCountRequest request);
    }
}
