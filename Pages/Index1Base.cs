using DataEntryDemo.Data;
using DataEntryDemo.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DataEntryDemo.Pages
{
	public class Index1Base : ComponentBase
	{
		public CarViewModel carViewModel = new();
		public EditContext? editContext;
		public ValidationMessageStore? carMessageStore;
		public bool savedOK = false;

		private string successMsgClass => savedOK ? "visible" : "hidden";

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}

		private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
		{
		}

		protected void OnHandleSubmit()
		{
		}

		protected void OnHandleInvalidSubmit()
		{
		}

		protected void Cancel()
		{
		}

		private async Task<List<Color>> PopulateColors()
		{
			return await Task.Run(() =>
				new List<Color>()
				{
				new Color() { Id = 0, Description = ""},
				new Color() { Id = 1, Description = "Black"},
				new Color() { Id = 2, Description = "White"},
				new Color() { Id = 3, Description = "Blue"},
				new Color() { Id = 4, Description = "Silver"},
				new Color() { Id = 5, Description = "Red"}
				});
		}

		private async Task<List<Model>> PopulateModels()
		{
			return await Task.Run(() =>
				new List<Model>()
					{
				new Model() { Id = 0, Description = ""},
				new Model() { Id = 1, Description = "Honda"},
				new Model() { Id = 2, Description = "Mercedes"},
				new Model() { Id = 3, Description = "BMW"},
				new Model() { Id = 4, Description = "Range Rover"},
				new Model() { Id = 5, Description = "Mazda"}
					});

		}

		private async Task<List<Status>> PopulateStatus()
		{
			return await Task.Run(() =>
				new List<Status>()
						{
				new Status() { Id = 0, Description = ""},
				new Status() { Id = 1, Description = "New"},
				new Status() { Id = 2, Description = "Used"},
				new Status() { Id = 3, Description = "Pre-owned"},
				new Status() { Id = 4, Description = "Mint"},
				new Status() { Id = 5, Description = "Questionable"}
						});
		}
	}
}
