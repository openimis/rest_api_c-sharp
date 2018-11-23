using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.Utils
{
	static public class LinqExtensions
	{
		public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
		{
			return en.Skip(page * pageSize).Take(pageSize);
		}

		public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
		{
			return en.Skip(page * pageSize).Take(pageSize);
		}

		static public IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> DescendBy)
		{
			foreach (T value in source)
			{
				yield return value;

				foreach (T child in DescendBy(value).Descendants<T>(DescendBy))
				{
					yield return child;
				}
			}
		}

		public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
		{
			var stack = new Stack<T>(items);
			while (stack.Any())
			{
				var next = stack.Pop();
				yield return next;
				foreach (var child in childSelector(next))
					stack.Push(child);
			}
		}
	}
}
