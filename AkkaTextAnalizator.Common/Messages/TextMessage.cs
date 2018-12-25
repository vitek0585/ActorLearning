namespace AkkaTextAnalizatorCommon.Messages
{
    public class TextMessage
    {
        public string Message { get; set; }

        public int Id { get; set; }

        public TextMessage(string message, int id)
        {
            Message = message;
            Id = id;
        }
    }
}