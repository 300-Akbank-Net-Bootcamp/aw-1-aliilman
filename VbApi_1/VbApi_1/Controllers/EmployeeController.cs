using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
namespace VbApi.Controllers;

public class Employee
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public double HourlySalary { get; set; }

}
public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Name)
             .NotEmpty()
             .WithMessage("Name cannot be empty.")
             .Length(10, 250)
             .WithMessage("Name must be between 10 and 250 characters.");

        RuleFor(x => x.DateOfBirth)
             .NotEmpty()
             .WithMessage("Date of Birth is required.")
             .Must(BeAValidBirthDate)
             .WithMessage("Birthdate is not valid.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required.")
            .EmailAddress()
            .WithMessage("Email address is not valid.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is required.")
            .Must(BeAValidPhoneNumber)
            .WithMessage("Phone is not valid.");

        RuleFor(x => x.HourlySalary)
            .InclusiveBetween(50, 400)
            .WithMessage("Hourly salary must be between 50 and 400.");

        RuleFor(x => x)
            .Must(MinLegalSalaryRequiredAttribute)
            .WithMessage("Minimum hourly salary is not valid.");
    }
    private bool BeAValidBirthDate(DateTime dateOfBirth)
    {
        var minAllowedBirthDate = DateTime.Today.AddYears(-65);
        return minAllowedBirthDate <= dateOfBirth;
    }

    private bool BeAValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        // Özel bir telefon numarasý doðrulama mantýðý burada uygulanýr
        // Bu örnek, basit bir sayý uzunluðu kontrolü ile sýnýrlýdýr
        return Regex.IsMatch(phone, @"^\d{10}$"); // Örneðin, 10 basamaklý bir sayý kabul eder
    }
    private bool MinLegalSalaryRequiredAttribute(Employee employee)
    {
        double minJuniorSalary= 50;
        double minSeniorSalary= 200;

        var dateBeforeThirtyYears = DateTime.Today.AddYears(-30);
        var isOlderThanThirdyYears = employee.DateOfBirth <= dateBeforeThirtyYears;
        var hourlySalary = (double)employee.HourlySalary;

        var isValidSalary = isOlderThanThirdyYears ? hourlySalary >= minSeniorSalary : hourlySalary >= minJuniorSalary;
        return isValidSalary;
    }


}

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    public EmployeeController()
    {
    }

    [HttpPost]
    public IActionResult Post([FromBody] Employee value)
    {

        EmployeeValidator validator = new EmployeeValidator();

        var results = validator.Validate(value);

        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
            return BadRequest(results);
        }
        //if (value.DateOfBirth > DateTime.Now.AddYears(-30) && value.HourlySalary < 200)
        //{
            
        //}
        return Created("api/Employee", value);
    }
}