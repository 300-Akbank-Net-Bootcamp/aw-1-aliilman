using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace VbApi.Controllers;

public class Staff
{
    //[Required]
    //[StringLength(maximumLength: 250, MinimumLength = 10)]
    public string? Name { get; set; }

    //[EmailAddress(ErrorMessage = "Email address is not valid.")]
    public string? Email { get; set; }

    //[Phone(ErrorMessage = "Phone is not valid.")]
    public string? Phone { get; set; }

    //[Range(minimum: 30, maximum: 400, ErrorMessage = "Hourly salary does not fall within allowed range.")]
    public decimal? HourlySalary { get; set; }
}
public class StaffValidator : AbstractValidator<Staff>
{
    public StaffValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name is required.")
            .Length(10, 250)
            .WithMessage("Name must be between 10 and 250 characters.");

        RuleFor(x => x.Email)
            //.NotEmpty()
            //.WithMessage("Email address is required.")
            .EmailAddress()
            .WithMessage("Email address is not valid.");

        RuleFor(x => x.Phone)
            //.NotEmpty()
            //.WithMessage("Phone is required.")
            .Must(BeAValidPhoneNumber)
            .WithMessage("Phone is not valid.");

        RuleFor(x => x.HourlySalary)
            //.NotNull()
            //.WithMessage("Hourly salary is required.")
            .InclusiveBetween(30, 400)
            .WithMessage("Hourly salary must be between 30 and 400.");
    }

    private bool BeAValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        // Özel bir telefon numarasý doðrulama mantýðý burada uygulanýr
        // Bu örnek, basit bir sayý uzunluðu kontrolü ile sýnýrlýdýr
        return Regex.IsMatch(phone, @"^\d{10}$"); // Örneðin, 10 basamaklý bir sayý kabul eder
    }
}


[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    public StaffController()
    {
    }

    [HttpPost]
    public IActionResult Post([FromBody] Staff value)
    {
        //Staff staff = new Staff();
        StaffValidator validator = new StaffValidator();

        var result = validator.Validate(value);


        if (!result.IsValid)
        {
            foreach (var failure in result.Errors)
            {
                Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
            return BadRequest(result);
        }
        return Created("api/Staff", value);
    }
}