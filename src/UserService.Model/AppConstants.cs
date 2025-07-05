namespace UserService.Model
{
    public static class AppConstants
    {
        public const string GrantType_Password = "Password";
        public const string GrantType_ClientCredential = "client_credentials";
    }

    public enum UserType
    {
        Customer = 1,
        Inspector = 2,
        Manager = 3,
        Admin = 4
    }
}
