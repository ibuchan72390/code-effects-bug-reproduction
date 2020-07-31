using System.Collections.Generic;

namespace CodeEffects.Rule.Core.Demo.Models
{
	/// <summary>
	/// A supportive class that demonstrates the use of dynamic menu data sources
	/// </summary>
	public class Physician
	{
		/// <summary>
		/// Returns a list of fictitious physicians
		/// </summary>
		/// <returns>List of data source items</returns>
		public static List<Rule.Common.DataSourceItem> List()
		{
			List<Rule.Common.DataSourceItem> physicians = new List<Rule.Common.DataSourceItem>();

			physicians.Add(new Rule.Common.DataSourceItem(1, "John Smith"));
			physicians.Add(new Rule.Common.DataSourceItem(2, "Anna Taylor"));
			physicians.Add(new Rule.Common.DataSourceItem(3, "Robert Brown"));
			physicians.Add(new Rule.Common.DataSourceItem(4, "Stephen Lee"));
			physicians.Add(new Rule.Common.DataSourceItem(5, "Joe Wilson"));
			physicians.Add(new Rule.Common.DataSourceItem(6, "Samuel Thompson"));

			return physicians;
		}
	}
}	