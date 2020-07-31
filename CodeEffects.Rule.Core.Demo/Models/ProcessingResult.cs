
namespace CodeEffects.Rule.Core.Demo.Models
{
	public class ProcessingResult
	{
		public bool IsRuleEmpty { get; set; }
		public bool IsRuleValid { get; set; }
		public string Output { get; set; }
		public string ClientInvalidData { get; set; }
		public Patient Patient { get; set; }

		public ProcessingResult()
		{
			this.IsRuleEmpty = false;
			this.IsRuleValid = true;
		}
	}
}