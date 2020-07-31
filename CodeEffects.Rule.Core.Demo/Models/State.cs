using CodeEffects.Rule.Attributes;

namespace CodeEffects.Rule.Core.Demo.Models
{
	public enum State
	{
		Arizona,
		California,
		Florida,
		[EnumItem("North Carolina")]
		NorthCarolina,
		Georgia,
		[ExcludeFromEvaluation]
		Unknown
	}
}