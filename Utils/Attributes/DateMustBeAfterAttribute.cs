using System.ComponentModel.DataAnnotations;

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
