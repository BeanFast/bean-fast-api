namespace DataTransferObjects.Account.Response
{
    public class LoginResponse
    {
        public string? RefreshToken { get; set; }

        public string? AccessToken { get; set; }
    }
}
