using CodeEffects.Rule.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeEffects.Rule.Core.Demo.Services
{
	public class DataService
	{
		private static SelectListItem Convert(DataSourceItem item, bool selected)
		{
			return new SelectListItem { Text = item.Name, Value = item.ID.ToString(), Selected = selected };
		}
	}
}