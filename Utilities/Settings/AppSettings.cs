
namespace Utilities.Settings
{
    public class AppSettings
    {
        public FirebaseSettings Firebase { get; set; } = default!;

        public TwilioSettings Twilio { get; set; } = default!;
    }
    
}
