namespace Quincy.Events
{
    public interface IStoreStateProvider
    {
        IStoreState GetState();
    }
}