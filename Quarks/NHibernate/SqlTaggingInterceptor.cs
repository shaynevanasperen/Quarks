using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.SqlCommand;

namespace Quarks.NHibernate
{
	/// <summary>
	/// An NHibernate IInterceptor which adds an SQL comment containing a method name to all generated SQL.
	/// Finds the method name by walking the stack trace selecting the first method matching the supplied predicate.
	/// </summary>
	[Serializable]
	public class SqlTaggingInterceptor : EmptyInterceptor
	{
		readonly Func<MethodBase, bool> _isTaggingMethod;

		public SqlTaggingInterceptor(Func<MethodBase, bool> taggingMethodPredicate)
		{
			_isTaggingMethod = taggingMethodPredicate;
		}

		public override SqlString OnPrepareStatement(SqlString sql)
		{
			return new SqlString(getNHibernateComment()).Append(sql);
		}

		protected virtual string GetAdditionalTags(StackTrace stackTrace, int startAtFrameIndex)
		{
			return string.Empty;
		}

		string getNHibernateComment()
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var stackTrace = new StackTrace(2, false); // Skip 2 frames in order to ignore this class
			int foundAtFrameIndex;
			var methodName = getCurrentFullyQualifiedTaggingMethodName(stackTrace, out foundAtFrameIndex);

			return string.Format(CultureInfo.InvariantCulture,
				"/* NHibernate [Method: {0}]{1} (Resolved in {2}ms) */",
				methodName, GetAdditionalTags(stackTrace, foundAtFrameIndex), stopWatch.Elapsed.TotalMilliseconds);
		}

		string getCurrentFullyQualifiedTaggingMethodName(StackTrace stackTrace, out int foundAtFrameIndex)
		{
			foreach (var frameIndex in Enumerable.Range(0, stackTrace.FrameCount - 1))
			{
				var method = stackTrace.GetFrame(frameIndex).GetMethod();
				if (_isTaggingMethod(method))
				{
					foundAtFrameIndex = frameIndex;
					if (method.DeclaringType != null)
						return method.DeclaringType.FullName + "." + method.Name;
					return method.Name;
				}
			}

			foundAtFrameIndex = stackTrace.FrameCount;
			return "Unknown method";
		}
	}
}
