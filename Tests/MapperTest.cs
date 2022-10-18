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
                customer.Address,
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
                customer.Address,
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
                Address = customer.Address,
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
            foreach (var x in customer)
            {
                customerResultsExpected.Add(new CustomerResult()
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    Cpf = x.Cpf,
                    Cellphone = x.Cellphone,
                    Birthdate = x.Birthdate,
                    EmailSms = x.EmailSms,
                    Whatsapp = x.Whatsapp,
                    Country = x.Country,
                    City = x.City,
                    PostalCode = x.PostalCode,
                    Address = x.Address,
                    Number = x.Number
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
            var portfolio = PortfolioFactory.FakePortfolio();
            var portfolioProducts = new List<PortfolioProductResult>();
            foreach (var x in portfolio.PortfoliosProducts)
            {
                portfolioProducts.Add(new PortfolioProductResult()
                {
                    ProductId = x.ProductId
                });
            }
            var portfolioProductsConverted = portfolioProducts.AsEnumerable();

            var expectedPortfolio = new PortfolioResult()
            {
                Name = portfolio.Name,
                TotalBalance = portfolio.TotalBalance,
                Products = portfolioProductsConverted,
                CustomerId = portfolio.CustomerId
            };

            // Act
            var portfolioToMap = _mapper.Map<PortfolioResult>(portfolio);

            // Assert
            portfolioToMap.Should().BeEquivalentTo(expectedPortfolio);
        }

        [Fact]
        public void Should_Pass_And_Return_IEnumerable_Of_PortfolioResult_When_Trying_To_Map_List_Of_Portfolio_To_IEnumerable_Of_PortfolioResult()
        {
            // Arrange
            var portfolios = PortfolioFactory.FakePortfolios();
            var expectedPortfolios = new List<PortfolioResult>();
            foreach (var x in portfolios)
            {
                var products = new List<PortfolioProductResult>();
                foreach (var y in x.PortfoliosProducts)
                {
                    products.Add(new PortfolioProductResult()
                    {
                        ProductId = y.ProductId
                    });
                }
                expectedPortfolios.Add(new PortfolioResult()
                {
                    Name = x.Name,
                    TotalBalance = x.TotalBalance,
                    CustomerId = x.CustomerId,
                    Products = products.AsEnumerable()
                });
            }
            var expectedPortfoliosConverted = expectedPortfolios.AsEnumerable();

            // Act
            var portfolioToMap = _mapper.Map<IEnumerable<PortfolioResult>>(portfolios);


            // Assert
            portfolioToMap.Should().BeEquivalentTo(expectedPortfoliosConverted);
        }

        [Fact]
        public void Should_Pass_And_Return_Portfolio_When_Trying_To_Map_CreatePortfolioRequest_To_Portfolio()
        {
            // Arrange
            var portfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();
            var expectedPortfolio = new Portfolio(portfolioRequest.Name, portfolioRequest.Description)
            {
                CustomerId = portfolioRequest.CustomerId
            };

            // Act
            var portfolioToMap = _mapper.Map<Portfolio>(portfolioRequest);

            // Assert
            portfolioToMap.Should().BeEquivalentTo(expectedPortfolio);
        }

        #endregion Portfolio Mapping Tests

        #region Order Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_Order_When_Trying_To_Map_CreateOrderRequest_To_Order()
        {
            // Arrange
            var orderRequest = OrderFactory.FakeCreateOrderRequest();
            var expectedOrder = new Order(orderRequest.Quotes, orderRequest.PortfolioId, orderRequest.ProductId)
            {
                ConvertedAt = DateTime.UtcNow.Date
            };

            // Act
            var orderToMap = _mapper.Map<Order>(orderRequest);
            orderToMap.ConvertedAt = DateTime.UtcNow.Date;

            // Assert
            orderToMap.Should().BeEquivalentTo(expectedOrder);
        }

        [Fact]
        public void Should_Pass_And_Return_OrderResult_When_Trying_To_Map_Order_To_OrderResult()
        {
            // Arrange
            var order = OrderFactory.FakeOrder();
            var expectedOrder = new OrderResult()
            {
                Quotes = order.Quotes,
                UnitPrice = order.UnitPrice,
                NetValue = order.NetValue,
                ConvertedAt = DateTime.UtcNow.Date,
                PortfolioId = order.PortfolioId,
                ProductId = order.ProductId
            };

            // Act
            var orderToMap = _mapper.Map<OrderResult>(order);
            orderToMap.ConvertedAt = DateTime.UtcNow.Date;

            // Assert
            orderToMap.Should().BeEquivalentTo(expectedOrder);
        }

        [Fact]
        public void Should_Pass_And_Return_IEnumerable_Of_OrderResult_When_Trying_To_Map_List_Of_Order_To_IEnumerable_Of_OrderResult()
        {
            // Arrange
            var orders = OrderFactory.FakeOrders();
            var expectedOrders = new List<OrderResult>();
            foreach (var x in orders)
            {
                expectedOrders.Add(new OrderResult()
                {
                    Quotes = x.Quotes,
                    UnitPrice = x.UnitPrice,
                    NetValue = x.NetValue,
                    ConvertedAt = DateTime.UtcNow.Date,
                    PortfolioId = x.PortfolioId,
                    ProductId = x.ProductId
                });
            }
            var expectedOrdersConverted = expectedOrders.AsEnumerable();

            // Act
            var ordersToMap = _mapper.Map<IEnumerable<OrderResult>>(orders);
            foreach (var x in ordersToMap)
            {
                x.ConvertedAt = DateTime.UtcNow.Date;
            }

            // Assert
            ordersToMap.Should().BeEquivalentTo(expectedOrdersConverted);
        }

        #endregion Order Mapping Tests

        #region PortfolioProduct Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_IEnumerable_Of_PortfolioProductResult_When_Trying_To_Map_ICollection_Of_PortfolioProduct_To_IEnumerable_Of_PortfolioProductResult()
        {
            // Arrange
            var portfolioProduct = PortfolioProductFactory.FakePortfolioProduct();
            var portfolioProductExpected = new List<PortfolioProductResult>();
            foreach (var x in portfolioProduct)
            {
                portfolioProductExpected.Add(new PortfolioProductResult()
                {
                    ProductId = x.ProductId
                });
            }
            var portfolioProductExpectedConverted = portfolioProductExpected.AsEnumerable();

            // Act
            var portfolioProductToMap = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioProduct);

            // Assert
            portfolioProductToMap.Should().BeEquivalentTo(portfolioProductExpectedConverted);
        }

        #endregion PortfolioProduct Mapping Tests

        #region Product Mapping Tests

        [Fact]
        public void Should_Pass_And_Return_ProductResult_When_Trying_To_Map_Product_To_ProductResult()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            var productResultExpected = new ProductResult()
            {
                Symbol = product.Symbol,
                UnitPrice = product.UnitPrice
            };

            // Act
            var productToMap = _mapper.Map<ProductResult>(product);

            // Assert
            productToMap.Should().BeEquivalentTo(productResultExpected);
        }

        [Fact]
        public void Should_Pass_And_Return_IEnumerable_Of_ProductResult_When_Trying_To_Map_List_Of_Product_To_IEnumerable_Of_ProductResult()
        {
            // Arrange
            var products = ProductFactory.FakeProducts();
            var productResultExpected = new List<ProductResult>();
            foreach (var x in products)
            {
                productResultExpected.Add(new ProductResult()
                {
                    Symbol = x.Symbol,
                    UnitPrice = x.UnitPrice
                });
            }
            var productResultExpectedConverted = productResultExpected.AsEnumerable();

            // Act
            var productToMap = _mapper.Map<IEnumerable<ProductResult>>(products);

            // Assert
            productToMap.Should().BeEquivalentTo(productResultExpectedConverted);
        }

        [Fact]
        public void Should_Pass_And_Return_Product_When_Trying_To_Map_CreateProductRequest_To_Product()
        {
            // Arrange
            var productRequest = ProductFactory.FakeCreateProductRequest();
            var productExpected = new Product(productRequest.Symbol, productRequest.UnitPrice);

            // Act
            var productToMap = _mapper.Map<Product>(productRequest);

            // Assert
            productToMap.Should().BeEquivalentTo(productExpected);
        }

        [Fact]
        public void Should_Pass_And_Return_Product_When_Trying_To_Map_UpdateProductRequest_To_Product()
        {
            // Arrange
            var productRequest = ProductFactory.FakeUpdateProductRequest();
            var productExpected = new Product(productRequest.Symbol, productRequest.UnitPrice);

            // Act
            var productToMap = _mapper.Map<Product>(productRequest);

            // Assert
            productToMap.Should().BeEquivalentTo(productExpected);
        }

        #endregion Product Mapping Tests
    }
}
