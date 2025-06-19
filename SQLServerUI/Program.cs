using DataAccessClassLibrary;
using DataAccessClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace SQLServerUI;

internal class Program
{
    static void Main(string[] args)
    {

        SqlCrud sql = new(GetConnectionString());
        //ReadAllContacts(sql);
        UpdateContact(sql);
        Console.WriteLine("Processing Done");

        Console.ReadLine();
    }

    public static void UpdateContact(SqlCrud sql)
    {
        BasicContactModel contact = new();
        contact.FirstName = "Eisa";
        contact.LastName = "Basim";
        contact.Id = 1;
        sql.UpdateContact(contact);
    }
    public static void CreateNewContact(SqlCrud sql)
    {


        FullContactModel userContact = new FullContactModel();

        userContact.BasicInfo = new BasicContactModel()
        {
            FirstName = "John",
            LastName = "Doe"
        };

        userContact.PhoneNumbers.Add(new PhoneNumberModel()
        {
            PhoneNumber = "1234567890"
        });

        userContact.PhoneNumbers.Add(new PhoneNumberModel()
        {
            PhoneNumber = "0987654321"
        });

        userContact.EmailAddresses.Add(new EmailAddressModel()
        {
            EmailAddress = "John@gmail.com"
        });

        sql.CreateContact(userContact);
    }

    private static void ReadAllContacts(SqlCrud sql)
    {
        var rows = sql.GetAllContacts();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
        }
    }
    
    private static void ReadContacts(SqlCrud sql,int contactId)
    {
        var contact = sql.GetFullContactById(contactId);

       
            Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName} ");
        foreach (var number in contact.PhoneNumbers)
        {
            Console.WriteLine(number.PhoneNumber);
        }
             foreach (var email in contact.EmailAddresses)
        {
            Console.WriteLine(email.EmailAddress);
        }
        
    }
    private static string GetConnectionString(string connnectionStringName = "Default")
    {
        string output = "";
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();

        output = config.GetConnectionString(connnectionStringName);

        return output;

    }


}

