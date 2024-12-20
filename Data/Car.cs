using DataEntryDemo.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataEntryDemo.Data
{
	public class Car
	{
		[Required]
		public string Name { set; get; } = "";
		[Required]
		[Range(1, 5)]
		public int ColorId { set; get; } = 0;
		[Required]
		[Range(2000, 2025)]
		public string? Year { set; get; } = null;
		[Required]
		[Range(1, 5)]
		public int ModelId { set; get; } = 0;
		[Required]
		[Range(1, 10)]
		public string? Quantity { set; get; } = null;
		[Required]
		[DataType(DataType.DateTime)]
		[DateMustBeBefore(nameof(LastAvailableDate))]
		public DateTime? ReleaseDate { set; get; } = null;
		[Required]
		[DateMustBeAfter(nameof(ReleaseDate))]
		[DataType(DataType.DateTime)]
		public DateTime? LastAvailableDate { set; get; } = null;
		[Required]
		public string? Total { set; get; } = null;
		[Required]
		public string? Owner { set; get; } = null;
		[Required]
		[Range(1, 5)]
		public int StatusId { set; get; } = 0;
	}
}
