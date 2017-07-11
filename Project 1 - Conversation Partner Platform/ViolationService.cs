using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPP2.Services
{
    public static class ViolationService
    {
        public static Dictionary<int, string> GetViolationDictionary()
        {
            return new Dictionary<int, string>()
        {
            {0, "Conversation Partner is defaming, verbally abusing, or stalking me." },
            {1, "Conversation Partner is threatening violence or harassment" },
            {2, "Conversation Partner is using a false identity for the purpose of misleading others" },
            {3, "Conversation Partner is spamming my email." },
            {4, "Conversation Partner was inappropriate during video/audio meeting." }
        };
        }
    }
}