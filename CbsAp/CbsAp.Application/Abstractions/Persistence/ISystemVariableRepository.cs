
using CbsAp.Domain.Entities.System;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ISystemVariableRepository
    {
        IQueryable<SystemVariable> Query();
    }
}
