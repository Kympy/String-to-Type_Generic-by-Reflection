using System.Diagnostics;
using System.Reflection;

namespace ConsoleApp2
{
	internal class FileName
	{
		public class TestClass { }
		private static string testClassTypeName = "TestClass";
		private static FileName instance;
		static void Main()
		{
			instance = new FileName();
			var type = instance.GetType();
			MethodInfo? method = type.GetMethod(nameof(PrintTypeName).ToString());

			MethodInfo? GenericMethod_For = method?.MakeGenericMethod(GetTypeByClassName_For(testClassTypeName));
			MethodInfo? GenericMethod_Linq = method?.MakeGenericMethod(GetTypeByClassName_Linq(testClassTypeName));
			MethodInfo? GenericMethod_Parallel = method?.MakeGenericMethod(GetTypeByClassName_Parallel(testClassTypeName));
			

			CheckTime("For", GenericMethod_For);
			CheckTime("Linq", GenericMethod_Linq);
			CheckTime("Parallel", GenericMethod_Parallel);
		}
		public static void CheckTime(string funcName, MethodInfo argMethod)
		{
			Console.Write($"{funcName} : ");
			Stopwatch timer = new Stopwatch();
			timer.Start();
			Action action = () => argMethod?.Invoke(null, null);
			action += () =>
			{
				Console.WriteLine($"  Elapsed Time : {string.Format("{0:0.00000000}", timer.Elapsed.TotalMilliseconds)}ms");
				timer.Stop();
			};
			action.Invoke();
		}
		public static void PrintTypeName<T>()
		{
			Console.Write(typeof(T).Name);
		}
		private static Type? GetTypeByClassName_For(string className)
		{
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] types = a.GetTypes();
				Type? targetType = types.FirstOrDefault((x) => x.Name == className);
				if (targetType != null)
				{
					return targetType;
				}
			}
			return null;
		}
		private static Type? GetTypeByClassName_Linq(string className)
		{
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] types = a.GetTypes();
				foreach (Type t in types)
				{
					if (t.Name == className)
					{
						return t;
					}
				}
			}
			return null;
		}
		private static Type? GetTypeByClassName_Parallel(string className)
		{
			Type? result = null;
			Parallel.For(0, AppDomain.CurrentDomain.GetAssemblies().Length, (i, state) =>
			{
				Type[] types = AppDomain.CurrentDomain.GetAssemblies()[i].GetTypes();

				Parallel.For(0, types.Length, (j, state_2) =>
				{
					if (types[j].Name == className)
					{
						result = types[j];
						state_2.Break();
						state.Break();
					}
				});
			});

			return result;
		}
	}
}
