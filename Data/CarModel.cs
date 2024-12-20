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

        public string GetColorFromID (int id)
		{
			return Colors?.FirstOrDefault(a => a.Id == id)?.Description ?? "";
		}


        public string GetModelFromID (int id)
        {
            return Models?.FirstOrDefault(a => a.Id == id)?.Description ?? "";
        }

        public string GetStatusFromID (int id)
        {
            return Statuses?.FirstOrDefault(a => a.Id == id)?.Description ?? "";
        }
    }
}
