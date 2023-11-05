using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleCalendarAPI.Interfaces;

namespace GoogleCalendarAPI.Services
{
    public class CalendarServiceGen : ICalendarServiceGen
    {

        public CalendarService GenerateCalendarService()
        {
            try
            {
                string[] Scopes = { "https://www.googleapis.com/auth/calendar", "https://www.googleapis.com/auth/calendar.events" };
                string ApplicationName = "Google Canlendar Events Api";
                UserCredential credential;
                using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials.json"), FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                CalendarService service = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                return service;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    }
}
