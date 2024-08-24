using Arstive.Display.Element;
using Arstive.Display.Element;
using Microsoft.Extensions.ObjectPool;

namespace Arstive.Model.ObjectPool
{
    internal class NotePooledObjectPolicy : IPooledObjectPolicy<INoteDisplay>
    {
        public INoteDisplay Create()
        {
            return new TapDisplay();
        }

        public bool Return(INoteDisplay note) => true;
    }
}
