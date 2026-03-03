using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Entities.System;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class SystemVariableRepository : ISystemVariableRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemVariableRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<SystemVariable> Query()
        {
            return _dbContext.Set<SystemVariable>().AsNoTracking().AsQueryable();
        }
    }
}
