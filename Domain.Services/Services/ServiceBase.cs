using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;

namespace Domain.Services.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        protected IRepositoryFactory<DataContext> RepositoryFactory { get; }
        protected IUnitOfWork<DataContext> UnitOfWork { get; }

        public ServiceBase(
            IRepositoryFactory<DataContext> repositoryFactory,
            IUnitOfWork<DataContext> unitOfWork
        )
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            RepositoryFactory = repositoryFactory ?? UnitOfWork;
        }
    }
}
