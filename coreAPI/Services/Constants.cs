namespace coreAPI.Services
{
    public class Constants
    {
        /// <summary>
        /// SQL Errors -  THROW @ stored procedures
        /// </summary>
        public static class ErrorCode
        {

            public const int InvalidApplication = 500030;
            public const int InvalidAccount = 500021;
            public const int WrongPassword = 500022;
            public const int NoApplicationPermissions = 500023;
            public const int InvalidUser = 5000024;
            public const int InvaliPermission = 500031;

        }

        public static class Claims
        {
            public const string ID = "id";
            public const string ApplicationID = "appID";
        }
    }
}