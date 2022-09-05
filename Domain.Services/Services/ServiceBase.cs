using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;

namespace Domain.Services.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        protected IRepositoryFactory RepositoryFactory { get; }
        protected IUnitOfWork UnitOfWork { get; }

        public ServiceBase(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork
        )
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            RepositoryFactory = repositoryFactory ?? UnitOfWork;
        }
    }
}
