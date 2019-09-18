namespace EDennis.AspNetCore.Base.Logging
{
    public class RequestLoggingOptions
    {
        public int MinimumHttpStatusCode { get; set; } = 400;
        public bool LogHeaders { get; set; } = false;
        public bool LogUserClaims { get; set; } = true;
        public bool LogRequestBody { get; set; } = true;
        public bool LogResponseBody { get; set; } = true;
        public bool LogExceptionMessage { get; set; } = true;
        public bool LogStackTrace { get; set; } = false;
    }
}
