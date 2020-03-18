using System;

namespace Application
{
    public class InventAppContext
    {
        [ThreadStatic]
        public static string UserName;

        [ThreadStatic]
        public static string SecurityToken;

        public static string Environment { get; set; }

        public static bool IsLocal() => Environment == "DEV";

        public static string WebApiUrl()
        {
            return Environment == "DEV" ? "http://localhost:8080/WebApi/api" : "http://www.ucirod.inventapp-test.com:8083/WebApi/api";
        }

        public static string ClientUrl()
        {
            return Environment == "DEV" ? "http://localhost:8080/WebApi/swagger" : "http://www.ucirod.inventapp-test.com:8083/WebApi/Swagger";
        }
    }
}