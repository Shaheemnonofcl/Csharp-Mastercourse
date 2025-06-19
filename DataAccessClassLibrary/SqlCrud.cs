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
        return db.LoadData<BasicContactModel, dynamic>(sql, new { }, connectionString);
    }

    public FullContactModel GetFullContactById(int Id)
    {
        string sql = "select id,firstname,lastname from dbo.contacts where id = @Id";
        FullContactModel output = new FullContactModel();

        output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = Id }, connectionString).FirstOrDefault();

        if (output.BasicInfo == null)
        {
            //do something if the contact is not found
            return null;
        }
        sql = @"select ea.*
                from EmailAddresses ea
                inner join ContactEmail ce
                on ea.Id=ce.EmailID
                where ce.ContactID=@Id";

        output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = Id }, connectionString);

        sql = @"select pa.*
            from PhoneNumbers pa
            inner join ContactPhoneNumbers cp
            on pa.Id=cp.PhoneNumberId
            where cp.ContactID=@Id";

        output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = Id }, connectionString);
        return output;
    }

    public void CreateContact(FullContactModel contact)
    {
        string sql = @"insert into dbo.contacts (FirstName, LastName)
                values (@FirstName, @LastName);
                select cast(scope_identity() as int)";
        //save the basic contact
        db.SaveData("", new
        {
            contact.BasicInfo.FirstName,
            contact.BasicInfo.LastName
        }, connectionString);
        //get the id number of the contact
        sql = @"select id from dbo.contacts
                where FirstName = @FirstName and LastName = @LastName";

        int contactId = db.LoadData<IdLookUpModel, dynamic>(sql, new
        {
            contact.BasicInfo.FirstName,
            contact.BasicInfo.LastName
        }, connectionString).First().Id;

        foreach (var phone in contact.PhoneNumbers)
        {
            if (phone.Id == 0)
            {
                sql = "insert into dbo.PhoneNumbers (PhoneNumber) Values(@PhoneNumber);";
                db.SaveData(sql, new { phone.PhoneNumber }, connectionString);

                sql = "select id from dbo.PhoneNumbers where PhoneNumber = @PhoneNumber;";
                phone.Id = db.LoadData<IdLookUpModel, dynamic>(sql, new { phone.PhoneNumber }, connectionString).First().Id;
            }

            sql = "insert into dbo.contactPhoneNumbers (contactId,PhoneNumberId) Values(@ContactId,@PhoneNumberId);";
            db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phone.Id }, connectionString);
        }

        foreach (var email in contact.EmailAddresses)
        {
            if (email.Id == 0)
            {
                sql = "insert into dbo.EmailAddresses (Email) values (@Email);";
                db.SaveData(sql, new { email.EmailAddress }, connectionString);

                sql = "select id from dbo.EmailAddresses where Email = @Email;";
                email.Id = db.LoadData<IdLookUpModel, dynamic>(sql, new { email.EmailAddress }, connectionString).First().Id;
            }

            sql = "insert into dbo.ContactEmail (ContactId, EmailId) values (@ContactId, @EmailId);";
            db.SaveData(sql, new { ContactId = contactId, EmailId = email.Id }, connectionString);
        }
    }

    public void UpdateContact(BasicContactModel contact)
    {
        string sql = "Update dbo.contacts set FirstName = @FirstName, LastName = @LastName where Id = @Id";
        db.SaveData(sql,new { contact }, connectionString);
    }

    public void RemovePhoneNumberFromContact(int id)
    {
        string sql = "delete from contactPhoneNumbers where PhoneNumberId=@Id";
        db.SaveData(sql, new { Id = id }, connectionString);


        sql = "delete from PhoneNumbers where id = @Id";
        db.SaveData(sql,new { Id = id }, connectionString);


    }
}

    