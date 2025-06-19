using DataAccessClassLibrary.Models;

namespace DataAccessClassLibrary;

public class SqlCrud
{
    private readonly string connectionString;
    private SqlDataAccess db = new();

    public SqlCrud(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<BasicContactModel> GetAllContacts()
    {
        string sql = "select id,firstname,lastname from dbo.contacts";
       return db.LoadData<BasicContactModel,dynamic>(sql, new { }, connectionString);
    }

    //public FullContactModel GetFullContactById(int Id)
    //{
    //    string sql = "select id,firstname,lastname from dbo.contacts where id = @Id";
    //    FullContactModel output = new FullContactModel();

    //    output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id }, connectionString).FirstOrDefault();

    //    if (output.BasicInfo == null)
    //    {
    //        //do something if the contact is not found
    //        return null;
    //    }
    //}
}
