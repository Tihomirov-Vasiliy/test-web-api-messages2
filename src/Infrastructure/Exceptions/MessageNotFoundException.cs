namespace Infrastructure.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException():base() { }   
        public MessageNotFoundException(string message):base(message) { }   
    }
}
