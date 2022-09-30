using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Factories;
using Xunit;

namespace Tests
{
    public class MapperTest
    {
        private readonly IMapper _mapper;

        public MapperTest(IMapper mapper) => _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        #region Customer Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_Customer_When_Trying_To_Map_CreateCustomerRequest_To_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeCreateCustomerRequest();
            var customerExpected = new Customer
            (
                customer.FullName,
                customer.Email,
                customer.Cpf,
                customer.Cellphone,
                customer.Birthdate,
                customer.EmailSms,
                customer.Whatsapp,
                customer.Country,
                customer.City,
                customer.PostalCode,
                customer.Adress,
                customer.Number
            );
            customerExpected.EmailConfirmation = customer.EmailConfirmation;

            // Act
            var customerToMap = _mapper.Map<Customer>(customer);

            // Assert
            customerToMap.Should().BeEquivalentTo(customerExpected);
        }

        [Fact]
        public void Should_Pass_And_Return_Customer_When_Trying_To_Map_UpdateCustomerRequest_To_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeUpdateCustomerRequest();
            var customerExpected = new Customer
            (
                customer.FullName,
                customer.Email,
                customer.Cpf,
                customer.Cellphone,
                customer.Birthdate,
                customer.EmailSms,
                customer.Whatsapp,
                customer.Country,
                customer.City,
                customer.PostalCode,
                customer.Adress,
                customer.Number
            );

            // Act
            var customerToMap = _mapper.Map<Customer>(customer);

            // Assert
            customerToMap.Should().BeEquivalentTo(customerExpected);
        }

        [Fact]
        public void Should_Pass_And_Return_CustomerResult_When_Trying_To_Map_Customer_To_CustomerResult()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            var customerExpected = new CustomerResult()
            {

                FullName = customer.FullName,
                Email = customer.Email,
                Cpf = customer.Cpf,
                Cellphone = customer.Cellphone,
                Birthdate = customer.Birthdate,
                EmailSms = customer.EmailSms,
                Whatsapp = customer.Whatsapp,
                Country = customer.Country,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Adress = customer.Adress,
                Number = customer.Number
            };

            // Act
            var customerToMap = _mapper.Map<CustomerResult>(customer);

            // Assert
            customerToMap.Should().BeEquivalentTo(customerExpected);
        }

        [Fact]
        public void Should_Pass_And_Return_IEnumerable_Of_CustomerResult_When_Trying_To_Map_List_Of_Customer_To_IEnumerable_Of_CustomerResult()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomers();
            var customerResultsExpected = new List<CustomerResult>();
            foreach (var item in customer)
            {
                customerResultsExpected.Add(new CustomerResult()
                {
                    FullName = item.FullName,
                    Email = item.Email,
                    Cpf = item.Cpf,
                    Cellphone = item.Cellphone,
                    Birthdate = item.Birthdate,
                    EmailSms = item.EmailSms,
                    Whatsapp = item.Whatsapp,
                    Country = item.Country,
                    City = item.City,
                    PostalCode = item.PostalCode,
                    Adress = item.Adress,
                    Number = item.Number
                });
            }
            var customerResultsExpectedConverted = customerResultsExpected.AsEnumerable();

            // Act
            var customerToMap = _mapper.Map<IEnumerable<CustomerResult>>(customer);

            // Assert
            customerToMap.Should().BeEquivalentTo(customerResultsExpectedConverted);
        }

        #endregion Customer Mapping Tests


        #region CustomerBankInfo Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_CustomerBankInfoResult_When_Trying_To_Map_CustomerBankInfo_To_CustomerBankInfoResult()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();

            var expectedCustomer = new CustomerBankInfoResult()
            {
                Account = customerBankInfo.Account,
                AccountBalance = customerBankInfo.AccountBalance
            };

            // Act
            var customerToMap = _mapper.Map<CustomerBankInfoResult>(customerBankInfo);

            // Assert
            customerToMap.Should().BeEquivalentTo(expectedCustomer);
        }

        #endregion CustomerBankInfo Mapping Tests


        #region Portfolio Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_PortfolioResult_When_Trying_To_Map_Portfolio_To_PortfolioResult()
        {
            // Arrange
            var portfolio = PortfolioFactory.CreatePortfolio();
            var portfolioProducts = new List<PortfolioProductResult>();
            foreach (var item in portfolio.PortfoliosProducts)
            {
                portfolioProducts.Add(new PortfolioProductResult()
                {
                    ProductId = item.ProductId
                });
            }
            var portfolioProductsConverted = portfolioProducts.AsEnumerable();

            var expectedPortfolio = new PortfolioResult()
            {
                Name = portfolio.Name,
                TotalBalance = portfolio.TotalBalance,
                Products = portfolioProductsConverted
            };

            // Act
            var portfolioToMap = _mapper.Map<PortfolioResult>(portfolio);
            var portfolioProductToMap = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolio.PortfoliosProducts);
            portfolioToMap.Products = portfolioProductToMap;

            // Assert
            portfolioToMap.Should().BeEquivalentTo(expectedPortfolio);
        }

        #endregion Portfolio Mapping Tests
    }
}
