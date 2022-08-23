using Application.Interfaces;
using AutoMapper;
using System;

namespace Application.Services
{
    public abstract class AppServicesBase : IAppServicesBase
    {
        protected IMapper Mapper { get; }
        public AppServicesBase(IMapper mapper)
            => Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
}
