using EventBooking.Api.Dtos;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Tests;

public class RegisterRequestTests
{
    [Fact]
    public void Register_ShortPassword_FailsValidation()
    {
        //Arange: a request with a 3-character password
        var request = new RegisterRequest
        {
            Name = "Sam",
            Email = "sam@x.com",
            Password = "abc"
        };

        //Act: run the same validation ASP.NET runs on incoming requests
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            request, new ValidationContext(request), results, true);

        //Assert: it must be rejected
        Assert.False(isValid);
    }

    [Fact]
    public void Register_ValidRequest_PassesValidation()
    {
        //Arange: a request with a proper payload
        var request = new RegisterRequest
        {
            Name = "Zorus2",
            Email = "zorus2@gmail.com",
            Password = "Zorus2123"
        };

        //Act: run the same validation ASP.NET runs on incoming requests
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            request, new ValidationContext(request), results, true);

        //Assert: it must be approved
        Assert.True(isValid);
    }
}
