using CbsAp.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace CbsAp.Application.Configurations.constants
{
    /// <summary>
    /// Constants messages for the CBS AP
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MessageConstants
    {
        // TODO :  to be deleted
        public const string Exists = "{0} already exists.";

        public const string GetSuccess = "Successfully retrieved {0}.";
        public const string GetNotFound = "{0} not found.";

        public const string AddSuccess = "Successfully added {0}.";
        public const string AddError = "Error adding {0}.";

        public const string UpdateSuccess = "Successfully updated {0}.";
        public const string UpdateNotFound = "{0} not found for update.";
        public const string UpdateError = "Error updating {0}.";

        public const string DeleteSuccess = "Successfully deleted {0}.";
        public const string DeleteNotFound = "{0} not found for delete.";
        public const string DeleteError = "Error deleting {0}.";

        public static string FormatMessage(string template, params object[] args)
        {
            return string.Format(template, args);
        }

      

        public static string Message(string parameter, MessageOperationType operation)
        {
            return operation switch
            {
                MessageOperationType.NotFound => $"{parameter} not found.",
                MessageOperationType.Retrieve => $"Successfully retrieved {parameter}.",
                MessageOperationType.Create => $"Successfully added {parameter}.",
                MessageOperationType.Update => $"Successfully updated {parameter}.",
                MessageOperationType.Exist => $"{parameter} already exists.",
                MessageOperationType.Delete => $"Successfully deleted {parameter}.",
                
                _ => $"{parameter} operation not recognized."
            };
        }
        public static string Message(MessageOperationType? operation, params string[] parameter)
        {
            var parameterString = string.Join(" , ", parameter);

            return operation switch
            {
                MessageOperationType.NotFound => $"{parameterString} not found.",
                MessageOperationType.Retrieve => $"Successfully retrieved {parameterString}.",
                MessageOperationType.Create => $"Successfully added {parameterString}.",
                MessageOperationType.Update => $"Successfully updated {parameterString}.",
                MessageOperationType.Exist => $"{parameterString} already exists.",
                MessageOperationType.Delete => $"Successfully deleted {parameterString}.",
                _ => $"{parameter} operation not recognized."
            };
        }
    }
}