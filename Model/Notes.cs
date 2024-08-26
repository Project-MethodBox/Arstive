namespace Arstive.Model
{
    public class Notes
    {
        public class Tap : Interfaces.NoteBase;
        public class Drag : Interfaces.NoteBase;

        public class Hold : Interfaces.NoteBase
        {
            /// <summary>
            /// End tap of hold hit
            /// </summary>
            public int EndTime { get; set; }
        }

        public enum NoteType
        {
            Tap,
            Drag,
            Hold
        }
    }
}
