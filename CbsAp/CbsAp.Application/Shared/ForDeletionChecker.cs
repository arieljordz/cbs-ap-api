using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Shared
{
    public static class ForDeletionChecker
    {
        public static ResponseResult<bool> DependencyCheckerResponseResult(DependencyCheckerType result, string metaData)
        {
            return result switch
            {
                DependencyCheckerType.HasDependencies => ResponseResult<bool>.BadRequest($"Cannot delete {metaData}. It is in use"),
                DependencyCheckerType.EntityNotFound => ResponseResult<bool>.BadRequest($"{metaData} metadata not found."),
                DependencyCheckerType.Error => ResponseResult<bool>.InternalServerError($"An error occurred while checking dependencies.{metaData}"),
                DependencyCheckerType.NoDependencies =>
                ResponseResult<bool>.Success(true),

                _ => ResponseResult<bool>.InternalServerError($"Unexpected dependency check result on {metaData}")
            };
        }
    }
}