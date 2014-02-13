namespace Thering.EventSourced.Eventing.Repositories
{
    public interface IEventPositionRepository
    {
        void SavePosition(int position);

        int? GetlastPosition();
    }
}