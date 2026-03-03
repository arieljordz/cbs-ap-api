namespace CbsAp.Domain.Common.Interfaces
{
    public interface IBaseAudtitableEntity
    {
        // We are catering global audience we are using UTC for saving date and time.
        string? CreatedBy { get; set; }

        DateTimeOffset? CreatedDate { get; set; }
        string? LastUpdatedBy { get; set; }
        DateTimeOffset? LastUpdatedDate { get; set; }
    }
}