using System.ComponentModel.DataAnnotations;
using DataEntryDemo.Data;

namespace DataEntryDemo.Data
{
	public class CarModel
	{
		[Required]
		public Car? Car { set; get; } = new Car();
		public List<Color>? Colors { set; get; } = new List<Color>();
		public List<Model>? Models { set; get; } = new List<Model>();
		public List<Status>? Statuses { set; get; } = new List<Status>();
	}
}
