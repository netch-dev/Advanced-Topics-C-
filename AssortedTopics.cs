using System.Diagnostics;
using System.Numerics;

namespace Netch.AdvancedTopics {
	public class AssortedTopics {
		#region Disposable objects
		// The 'using' statement is a shorthand for try-finally
		// Implementing the IDisposable interface allows you to put it inside a using statement
		// - As soon as you enter the scope of the using statement, the object is created
		// - When you exit the scope, the Dispose method is called

		public class DisposableObject : IDisposable {
			public DisposableObject() {
				Console.WriteLine("Hello");
			}

			public void Dispose() {
				Console.WriteLine("goodbye");
			}
		}

		public void DisposableObjectExample() {
			using (DisposableObject obj = new DisposableObject()) {
				Console.WriteLine("Inside using statement");
			}

			// After the using statement, the DisposableObjects Dispose method is called
			Console.WriteLine("Outside using statement");
		}

		// Use case for Disposables: Simple timer class
		public class SimpleTimer : IDisposable {
			private readonly Stopwatch st;
			public SimpleTimer() {
				st = new Stopwatch();
				st.Start();
			}

			public void Dispose() {
				st.Stop();
				Console.WriteLine($"Elapsed time: {st.ElapsedMilliseconds}ms");
			}
		}

		public void SimpleTimerExample() {
			// You can also construct the Disposable without a reference to it if you don't need it
			using (new SimpleTimer()) {
				Thread.Sleep(1000);
			}
		}

		// We can also make a custom Disposable that takes what to do at the start and the end
		public class GeneralPurposeDisposable : IDisposable {
			private readonly Action end;
			private GeneralPurposeDisposable(Action start, Action end) {
				this.end = end;
				start();
			}

			public void Dispose() {
				end();
			}

			// Factory method
			public static GeneralPurposeDisposable Create(Action start, Action end) {
				return new GeneralPurposeDisposable(start, end);
			}
		}

		public void GeneralPurposeDisposableExample() {
			Stopwatch st = new Stopwatch();
			using (GeneralPurposeDisposable.Create(() => st.Start(), () => st.Stop())) {
				Console.WriteLine("Performing some work...");
				Thread.Sleep(1000);
				Console.WriteLine($"Elapsed ms: {st.ElapsedMilliseconds}");
			}
		}
		#endregion

		#region Duck typing
		private ref struct Foo {
			public void Dispose() {
				Console.WriteLine("Disposing foo");
			}
		}

		// duck typing: you can call a method from a class, even if that method is not part of that interface
		// GetEnumerator() - foreach (IEnumerable<T>)

		// Dispose() - using (IDisposable)
		public void TestDuckTyping() {
			// This will invoke the Dispose method, even though we didn't implement IDisposable
			using Foo foo = new Foo();
		}

		#endregion

		#region Mixin
		// mixin: Add additional functionality to a class
		private interface IMyDisposable<T> : IDisposable {
			void IDisposable.Dispose() {
				Console.WriteLine($"Disposing {typeof(T).Name}");
			}
		}

		// We've taken the functionality of IMyDisposable (Dispose method) and added it to MyClass
		public class MyClass : IMyDisposable<MyClass> {

		}

		public void TestMixin() {
			// Because of duck typing, the c# compiler will find the Dispose method in IMyDisposable and call it
			using MyClass myClass = new MyClass();
		}
		#endregion

		#region Continuation passing style
		// Style of programming where you pass a function to another function, which will call it when it's done
		// - Useful for separating a complicated algorithms into smaller parts

		public class QuadraticEquationSolver {
			// Since we're using an entire class instead of a single function, we may use more than one function to solve the equation

			// ax^2 + bx + c == 0
			public Tuple<Complex, Complex> StartCalculation(double a, double b, double c) {
				double discriminant = (b * b) - (4 * a * c);
				if (discriminant < 0) {
					return SolveComplex(a, b, discriminant);
				} else {
					return SolveSimple(a, b, discriminant);
				}
			}

			private Tuple<Complex, Complex> SolveSimple(double a, double b, double discriminant) {
				double rootDisc = Math.Sqrt(discriminant);
				return Tuple.Create(
					new Complex(-b + (rootDisc / (2 * a)), 0),
					new Complex(-b - (rootDisc / (2 * a)), 0)
				);
			}

			private Tuple<Complex, Complex> SolveComplex(double a, double b, double discriminant) {
				Complex rootDisc = Complex.Sqrt(new Complex(discriminant, 0));
				return Tuple.Create(
					-b + (rootDisc / (2 * a)),
					-b - (rootDisc / (2 * a))
				);
			}
		}

		public void ContinuationPassingStyleExample() {
			QuadraticEquationSolver solver = new QuadraticEquationSolver();
			Tuple<Complex, Complex> result = solver.StartCalculation(1, 2, 3);
			Console.WriteLine($"Roots: {result.Item1}, {result.Item2}");
		}
		#endregion
	}
}