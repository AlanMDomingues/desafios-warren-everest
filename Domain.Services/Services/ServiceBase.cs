using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;

namespace Domain.Services.Services;

public abstract class ServiceBase : IServiceBase
{
    protected IRepositoryFactory RepositoryFactory { get; }
    protected IUnitOfWork UnitOfWork { get; }

    public ServiceBase(
        IRepositoryFactory<DataContext> repositoryFactory,
        IUnitOfWork<DataContext> unitOfWork
    )
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        RepositoryFactory = repositoryFactory ?? (IRepositoryFactory)UnitOfWork;
    }
}
