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
        ReadAllContacts(sql);



        Console.ReadLine();
    }


    private static void ReadAllContacts(SqlCrud sql)
    {
        var rows = sql.GetAllContacts();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
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

