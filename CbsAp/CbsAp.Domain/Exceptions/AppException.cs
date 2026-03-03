namespace CbsAp.Domain.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string title, string message) : base(message)
            => Title = title;

        public string Title { get; }
    }
}