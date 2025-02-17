using UserService.DataStorage.DAL;
using UserService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using User = UserService.DataStorage.DAL.User;
using Microsoft.AspNetCore.Identity;

namespace UserService.Services
{
    public class GlamUserService : IUserService
    {
        private readonly UsersContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEventPublisher _eventPublisher;

        ///<inheritdoc/>
        public GlamUserService(IConfiguration configuration, UsersContext usersContext
            , IPasswordHasher<User> passwordHasher, IEventPublisher eventPublisher)
        {
            _passwordHasher = passwordHasher;
            _dbContext = usersContext;
            _eventPublisher = eventPublisher;
        }

        ///<inheritdoc/>
        public UserResponse GetUserByIdAsync(int id)
        {
            UserResponse userResponse = null;
            var user = _dbContext.Users
                .Include(u => u.UserAddresses)
                .FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                userResponse = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserAddresses = user.UserAddresses.Select(ud => new Address
                    {
                        Id = ud.Id,
                        AddressLine1 = ud.AddressLine1,
                        AddressLine2 = ud.AddressLine2,
                        City = ud.City,
                        PostalCode = ud.PostalCode,
                        State = ud.State,
                        Country = ud.Country,
                        IsDefault = ud.IsDefault
                    }).ToList()
                };
            }
            return userResponse;
        }

        ///<inheritdoc/>
        public async Task<(bool IsSuccess, string Message)> RegisterUserAsync(RegisterCustomerRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                return (false, "Email is already registered.");

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UserRoles = [new UserRole {
                RoleId = 1,
                }],
                UserAddresses = [new UserAddress{
                AddressLine1 = request.AddressFlat,
                AddressLine2 = request.AddressStreet,
                City = request.AddressCity,
                PostalCode = request.AddressZipCode,
                State = request.AddressState,
                Country = "IN",
                IsDefault = true,
                CreatedAt = DateTime.Now,
                }]
            };

            // Hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            //var userRole = new UserR
            // Add user to the database
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            //Publish the message to send the notification to user
            NotificationEvent<NewUser> notificationEvent = new()
            {
                EventType = "User.Registered",
                TimeStamp = DateTime.UtcNow,
                Payload = new NewUser
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                }
            };

            //Fire and forgot notification message push
            _ = _eventPublisher.PublishEventAsync(notificationEvent);

            return (true, "Registration successful.");
        }

        ///<inheritdoc/>
        public async Task<(bool IsSuccess, string Message)> UpdateUserAsync(UpdateCustomerRequest request)
        {
            var user = await _dbContext.Users
                .Include("UserAddresses")
                .FirstOrDefaultAsync(user => user.Id.ToString() == request.Id);
            if (user == null)
            {
                return (false, "User not found");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.UserAddresses = [new UserAddress{
                AddressLine1 = request.AddressFlat,
                AddressLine2 = request.AddressStreet,
                City = request.AddressCity,
                PostalCode = request.AddressZipCode,
                State = request.AddressState,
                Country = "IN",
                IsDefault = true,
                CreatedAt = DateTime.Now,
                }];
            await _dbContext.SaveChangesAsync();

            return (true, "User update successfully.");
        }
    }
}
