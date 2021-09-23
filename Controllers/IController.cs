
namespace Game1
{
    // I thought i'd need more in here.
    interface IController
    {
        public void Update();
        public void AddMapping(int key, ICommand command);
    }

}
