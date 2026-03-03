namespace CbsAp.Application.Shared.ResultPatten
{
    /// <summary>
    ///  This will the TResponse in returning the  result pattern
    ///  in Validation Behaviour
    /// </summary>
    public abstract class BaseResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; }
    }
}