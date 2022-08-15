using Application.Models.Response;

namespace Application.Interfaces
{
    public interface ICustomerBankInfoAppService
    {
        public CustomerBankInfoResult Get(int id);
    }
}