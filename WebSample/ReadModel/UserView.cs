using System.Collections.Generic;

namespace WebSample.ReadModel
{
    using System;

    public class UserView
    {
        public static ICollection<string> Adresses { get; set; }
        public static Guid UserId { get; set; }
    }
}