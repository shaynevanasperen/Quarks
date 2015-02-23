using System.Collections.Generic;
using System.Linq.Expressions;

namespace Quarks.ExpressionVisitors
{
	class ParameterRebinderVisitor : ExpressionVisitor
	{
		readonly Dictionary<ParameterExpression, ParameterExpression> _map;

		internal ParameterRebinderVisitor(Dictionary<ParameterExpression, ParameterExpression> map)
		{
			_map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
		}

		internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
		{
			return new ParameterRebinderVisitor(map).Visit(exp);
		}

		protected override Expression VisitParameter(ParameterExpression p)
		{
			ParameterExpression replacement;
			if (_map.TryGetValue(p, out replacement))
			{
				p = replacement;
			}
			return base.VisitParameter(p);
		}
	}
}