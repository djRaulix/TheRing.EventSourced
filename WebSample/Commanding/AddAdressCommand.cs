using TheRing.EventSourced.Application;

namespace WebSample.Commanding
{
    public class AddAdressCommand : UpdateCommand
    {
        public string Address { get; set; }
    }
}
