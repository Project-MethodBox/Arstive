namespace Arstive.Model
{
    public class Notes
    {
        public class Tap : Interfaces.NoteBase;

        public class Drag : Interfaces.NoteBase;

        public enum NoteType
        {
            Tap,
            Drag,
            Hold
        }
    }
}
