using Bogus;
using Domain.Models;

namespace Tests.Factories;

public static class CustomerBankInfoFactory
{
    public static CustomerBankInfo FakeCustomerBankInfo()
    {
        var customerBankInfoFaker = new Faker<CustomerBankInfo>()
            .CustomInstantiator(x => new CustomerBankInfo(x.Finance.Account(20), x.Finance.Amount(0, 10000)))
            .RuleFor(x => x.Id, x => ++x.IndexVariable);

        var customerBankInfo = customerBankInfoFaker.Generate();
        return customerBankInfo;
    }
}
