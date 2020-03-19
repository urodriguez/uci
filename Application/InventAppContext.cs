using System;

namespace Application
{
    public class InventAppContext
    {
        [ThreadStatic]
        public static string UserName;

        [ThreadStatic]
        public static string SecurityToken;
    }
}