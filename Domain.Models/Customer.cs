using Infrastructure.Extensions;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Customer
    {
        public Customer(
            string fullName,
            string email,
            string cpf,
            string cellphone,
            DateTime birthdate,
            bool emailSms,
            bool whatsapp,
            string country,
            string city,
            string postalCode,
            string adress,
            int number
        )
        {
            FullName = fullName;
            Email = email;
            Cpf = cpf.FormatCpf();
            Cellphone = cellphone;
            Birthdate = birthdate;
            EmailSms = emailSms;
            Whatsapp = whatsapp;
            Country = country;
            City = city;
            PostalCode = postalCode;
            Adress = adress;
            Number = number;
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmation { get; set; }
        public string Cpf { get; set; }
        public string Cellphone { get; set; }
        public DateTime Birthdate { get; set; }
        public bool EmailSms { get; set; }
        public bool Whatsapp { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Adress { get; set; }
        public int Number { get; set; }

        public virtual CustomerBankInfo CustomerBankInfo { get; set; }

        public virtual ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}