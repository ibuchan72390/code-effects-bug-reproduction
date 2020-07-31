using CodeEffects.Rule.Attributes;

namespace CodeEffects.Rule.Core.Demo.Models
{
	public enum Gender
	{
		Male,
		Female,
		[ExcludeFromEvaluation]
		Unknown
	}
}