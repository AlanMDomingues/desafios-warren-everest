using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;

namespace Domain.Services.Services
{
    public class CustomerBankInfoService : ServiceBase, ICustomerBankInfoService
    {
        public CustomerBankInfoService(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork
        ) : base(repositoryFactory, unitOfWork)
        { }

        public void Create(Customer customer)
        {
            var randomNumber = new Random();
            string accountNumber = "";
            for (int i = 0; i < 10; i++)
                accountNumber += randomNumber.Next(0, 9).ToString();

            var repository = RepositoryFactory.Repository<CustomerBankInfo>();
            repository.Add(new CustomerBankInfo()
            {
                Account = accountNumber,
                AccountBalance = 0,
                Customer = customer
            });
        }
    }
}
