using System.Drawing;
using TheRing.EventSourced.Application;

namespace WebSample.Commanding
{
    public class CreateUserCommand : CreateCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}