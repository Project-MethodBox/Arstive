using Arstive.Display.Element.Element;
using Microsoft.Extensions.ObjectPool;

namespace Arstive.Model.ObjectPool
{
    internal class NotePooledObjectPolicy : IPooledObjectPolicy<NoteDisplay>
    {
        public NoteDisplay Create()
        {
            return new NoteDisplay();
        }

        public bool Return(NoteDisplay note) => true;
    }
}
