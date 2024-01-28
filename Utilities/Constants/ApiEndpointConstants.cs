namespace Utilities.Constants
{
    public static class ApiEndpointConstants
    {
        public const string RootEndPoint = "/api";
        public const string ApiVersion = "v1";
        public const string ApiEndpoint = RootEndPoint + "/" + ApiVersion;
        
        public class Auth
        {
            public const string Login = "login";
            public const string Register = "register";

        }
        public class Category
        {
            public const string GetCategorybyId = "{id}";
        }
    }
    
}
