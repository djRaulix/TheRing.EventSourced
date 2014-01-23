using System.Collections.Generic;

namespace WebSample.ReadModel
{
    using System;

    public class Database
    {
        public static ICollection<string> Adresses { get; set; }
        public static Guid UserId { get; set; }
    }
}