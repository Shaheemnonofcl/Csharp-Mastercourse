namespace DataAccessClassLibrary.Models;

 public class FullContactModel
{
    public BasicContactModel BasicInfo { get; set; }
    public List<EmailAddressModel> EmailAddresses { get; set; } = new();

    public List<PhoneNumberModel> PhoneNumbers { get; set; } = new();
}
