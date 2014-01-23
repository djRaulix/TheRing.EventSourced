using TheRing.EventSourced.Application;
using WebSample.Domain.User;

namespace WebSample.Commanding
{
    public class UserCommandHandler : IRunCommand<User, AddAdressCommand>, IRunCommand<User, CreateUserCommand>
    {
        public void Run(User agg, AddAdressCommand command)
        {
            agg.AddAddress(command.Address);
        }

        public void Run(User agg, CreateUserCommand command)
        {
            agg.Create(command.FirstName, command.LastName);
        }
    }
}