using Bogus;
using Domain.Models;

namespace Tests.Factories;

public static class CustomerBankInfoFactory
{
    public static CustomerBankInfo FakeCustomerBankInfo()
    {
        var customerBankInfoFaker = new Faker<CustomerBankInfo>()
            .RuleFor(x => x.Account, x => x.Finance.Account(20))
            .RuleFor(x => x.AccountBalance, x => x.Finance.Amount(0, 10000));

        var customerBankInfo = customerBankInfoFaker.Generate();
        return customerBankInfo;
    }
}
