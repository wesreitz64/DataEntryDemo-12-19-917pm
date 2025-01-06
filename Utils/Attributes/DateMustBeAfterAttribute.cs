using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DataEntryDemo.Utils.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DateMustBeAfterAttribute : ValidationAttribute
	{
		public DateMustBeAfterAttribute(string targetPropertyName)
			=> TargetPropertyName = targetPropertyName;

		public string TargetPropertyName { get; }

		public string GetErrorMessage(string propertyName) =>
			$"'{propertyName}' must be after '{TargetPropertyName}'.";

		protected override ValidationResult? IsValid(
			object? value, ValidationContext validationContext)
		{
			var targetValue = validationContext.ObjectInstance
				.GetType()
				.GetProperty(TargetPropertyName)
				?.GetValue(validationContext.ObjectInstance, null);

			if ((DateTime?)value < (DateTime?)targetValue)
			{
				var propertyName = validationContext.MemberName ?? string.Empty;
				return new ValidationResult(GetErrorMessage(propertyName), new[] { propertyName });
			}

			return ValidationResult.Success;
		}
	}
}

public class LettersAndNumbersAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid (object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            // Allow nulls if you want it to be optional
            return ValidationResult.Success;
        }

        var stringValue = value.ToString();
        var regex = new Regex("^[a-zA-Z0-9]*$"); // Matches only letters and numbers

        if (regex.IsMatch(stringValue))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("The field must only contain letters or numbers.");
    }
}
