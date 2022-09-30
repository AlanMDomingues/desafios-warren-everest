using Application.Models.Requests;
using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Tests.Factories;

public static class CustomerFactory
{
    public static CreateCustomerRequest FakeCreateCustomerRequest()
    {
        var fakeCustomer = new Faker<CreateCustomerRequest>()
            .RuleFor(x => x.FullName, x => x.Name.FindName())
            .RuleFor(x => x.Cpf, x => x.Person.Cpf(false))
            .RuleFor(x => x.Cellphone, x => x.Phone.PhoneNumber("##9########"))
            .RuleFor(x => x.Email, x => x.Internet.Email())
            .RuleFor(x => x.Birthdate, x => x.Date.Past(130, DateTime.Now.AddYears(-18)))
            .RuleFor(x => x.EmailSms, x => x.Random.Bool())
            .RuleFor(x => x.Whatsapp, x => x.Random.Bool())
            .RuleFor(x => x.Country, x => x.Address.Country())
            .RuleFor(x => x.City, x => x.Address.City())
            .RuleFor(x => x.PostalCode, x => x.Address.ZipCode("########"))
            .RuleFor(x => x.Adress, x => x.Address.StreetAddress())
            .RuleFor(x => x.Number, x => x.Random.Int(1, 100000));

        var customer = fakeCustomer.Generate();
        if (customer.Whatsapp is false && customer.EmailSms is false)
        {
            customer.Whatsapp = true;
        }
        customer.EmailConfirmation = customer.Email;
        return customer;
    }

    public static UpdateCustomerRequest FakeUpdateCustomerRequest()
    {
        var fakeCustomer = new Faker<UpdateCustomerRequest>()
            .RuleFor(x => x.FullName, x => x.Name.FindName())
            .RuleFor(x => x.Cpf, x => x.Person.Cpf(false))
            .RuleFor(x => x.Cellphone, x => x.Phone.PhoneNumber("##9########"))
            .RuleFor(x => x.Email, x => x.Internet.Email())
            .RuleFor(x => x.Birthdate, x => x.Date.Past(130, DateTime.Now.AddYears(-18)))
            .RuleFor(x => x.EmailSms, x => x.Random.Bool())
            .RuleFor(x => x.Whatsapp, x => x.Random.Bool())
            .RuleFor(x => x.Country, x => x.Address.Country())
            .RuleFor(x => x.City, x => x.Address.City())
            .RuleFor(x => x.PostalCode, x => x.Address.ZipCode("########"))
            .RuleFor(x => x.Adress, x => x.Address.StreetAddress())
            .RuleFor(x => x.Number, x => x.Random.Int(1, 100000));

        var customer = fakeCustomer.Generate();
        if (customer.Whatsapp is false && customer.EmailSms is false)
        {
            customer.Whatsapp = true;
        }
        return customer;
    }

    public static Customer FakeCustomer()
    {
        var fakeCustomer = new Faker<Customer>()
            .CustomInstantiator(x => new Customer(
                fullName: x.Name.FindName(),
                email: x.Internet.Email(),
                cpf: x.Person.Cpf(false),
                cellphone: x.Phone.PhoneNumber("##9########"),
                birthdate: x.Date.Past(130, DateTime.Now.AddYears(-18)),
                emailSms: x.Random.Bool(),
                whatsapp: x.Random.Bool(),
                country: x.Address.Country(),
                city: x.Address.City(),
                postalCode: x.Address.ZipCode("########"),
                adress: x.Address.StreetAddress(),
                number: x.Random.Int(1, 100000)
                ))
            .RuleFor(x => x.Id, x => ++x.IndexVariable);


        var customer = fakeCustomer.Generate();
        if (customer.Whatsapp is false && customer.EmailSms is false)
        {
            customer.Whatsapp = true;
        }
        customer.EmailConfirmation = customer.Email;
        return customer;
    }

    public static List<Customer> FakeCustomers()
    {
        var fakeCustomer = new Faker<Customer>()
            .CustomInstantiator(x => new Customer(
                fullName: x.Name.FindName(),
                email: x.Internet.Email(),
                cpf: x.Person.Cpf(false),
                cellphone: x.Phone.PhoneNumber("##9########"),
                birthdate: x.Date.Past(130, DateTime.Now.AddYears(-18)),
                emailSms: x.Random.Bool(),
                whatsapp: x.Random.Bool(),
                country: x.Address.Country(),
                city: x.Address.City(),
                postalCode: x.Address.ZipCode("########"),
                adress: x.Address.StreetAddress(),
                number: x.Random.Int(1, 100000)
                ))
            .RuleFor(x => x.Id, x => ++x.IndexVariable);

        var customers = fakeCustomer.Generate(5);
        foreach (var customer in customers)
        {
            if (customer.Whatsapp is false && customer.EmailSms is false)
            {
                customer.Whatsapp = true;
            }

            customer.EmailConfirmation = customer.Email;
        }
        return customers;
    }
}
