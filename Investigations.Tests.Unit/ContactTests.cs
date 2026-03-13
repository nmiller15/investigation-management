using Investigations.Models;

namespace Investigations.Tests.Unit;

public class ContactTests
{
    [Theory]
    [InlineData(IContactMethod.Types.Home)]
    [InlineData(IContactMethod.Types.Work)]
    [InlineData(IContactMethod.Types.Mobile)]
    public void UpdateHomePhone_ReplacesExistingValueForPhoneType(IContactMethod.Types type)
    {
        var expectedPhoneNumber = "123-456-7890";
        var contact = new Contact
        {
            PhoneNumbers = new List<PhoneNumber>()
            {
                new PhoneNumber("111-111-1111", type)
            }
        };

        switch (type)
        {
            case IContactMethod.Types.Home:
                contact.HomePhone(expectedPhoneNumber);
                break;
            case IContactMethod.Types.Work:
                contact.WorkPhone(expectedPhoneNumber);
                break;
            case IContactMethod.Types.Mobile:
                contact.MobilePhone(expectedPhoneNumber);
                break;
        }

        Assert.Equal(contact.PhoneNumbers.First(p => p.Type == type).Value, expectedPhoneNumber);
        Assert.Single(contact.PhoneNumbers, p => p.Type == type);
    }

    [Theory]
    [InlineData(IContactMethod.Types.Email)]
    [InlineData(IContactMethod.Types.Mobile)]
    public void PrimaryContactMethod_SetsOnlySpecifiedTypeAsPrimary(IContactMethod.Types type)
    {
        var contact = new Contact()
            .Name("John", "Doe")
            .EmailAddress("jdoe@sample.email")
            .HomePhone("111-111-1111")
            .WorkPhone("111-111-1111")
            .MobilePhone("111-111-1111");

        contact.PrimaryContactMethod(type);

        switch (type)
        {
            case IContactMethod.Types.Email:
                Assert.True(contact.Email.IsPrimary);
                Assert.All(contact.PhoneNumbers, p => Assert.False(p.IsPrimary));
                break;
            case IContactMethod.Types.Mobile:
                Assert.True(contact.PhoneNumbers.First(p => p.Type == IContactMethod.Types.Mobile).IsPrimary);
                Assert.False(contact.Email.IsPrimary);
                Assert.All(contact.PhoneNumbers.Where(p => p.Type != IContactMethod.Types.Mobile), p => Assert.False(p.IsPrimary));
                break;
        }
    }
}
