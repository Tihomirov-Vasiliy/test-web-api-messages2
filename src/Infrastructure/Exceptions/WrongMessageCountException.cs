namespace Infrastructure.Exceptions
{
    public class WrongMessageCountException : Exception
    {
        public WrongMessageCountException() : base() { }
        public WrongMessageCountException(string message) : base(message) { }
    }
}
