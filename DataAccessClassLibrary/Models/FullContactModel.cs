namespace DataAccessClassLibrary.Models;

 public class FullContactModel
{
    public BasicContactModel BasicInfo { get; set; }
    public List<EmailAddressModel> EmailAddress { get; set; }

    public List<PhoneNumberModel> PhoneNumbers { get; set; }
}
