using Application.Interfaces;
using Application.Models.Response;
using AutoMapper;
using Domain.Services.Interfaces;

namespace Application.Services
{
    public class CustomerBankInfoAppService : ICustomerBankInfoAppService
    {
        private readonly ICustomerBankInfoService _customerBankInfoService;
        private readonly IMapper _mapper;

        public CustomerBankInfoAppService(ICustomerBankInfoService customerBankInfoService, IMapper mapper)
        {
            _customerBankInfoService = customerBankInfoService;
            _mapper = mapper;
        }

        public CustomerBankInfoResult Get(int id)
        {
            var customerBankInfo = _customerBankInfoService.Get(id);
            var result = _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
            return result;
        }
    }
}
