namespace Netch.AdvancedTopics {
	public class CleanCode {
		#region Common Code Smells

		#region Poor Names
		// - Code should be self-explanatory and intention revealing
		// -- Not too short, not too long
		// -- Meaningful
		// -- Chosen from problem domain

		// -- Mysterious/poorly named variables, methods, classes, force the reader to look at the implementation
		// -- Long methods, classes, and files are hard to understand
		// -- Meaningless names/comments, comments that are out of date, comments that are redundant

		// - Noisy names:
		// -- theCustomer vs customer
		// -- listOfApprovedCustomers vs approvedCustomers
		#endregion

		#region Inconsistent Naming Conventions
		// - Follow the conventions of the language and the project
		// - This way the project will look like it was written by a single person

		// PascalCase: public class, method, property, event, namespace, enum, type, etc.
		// camelCase: local variable, parameter, method argument, private field, etc.
		public class Customer {
			private int _id;
			public string Name { get; set; }
			public void Charge(int amount) {
				double tax = amount * 0.1;
			}
		}
		#endregion

		#region Poor Method Signatures
		// - Orange GetCustomer(int airplane);
		// -- The method above is not clear about what it does

		// void Parse(int command); vs int Parse(string command);
		#endregion

		#region Long Parameter List 
		// - The more parameters we have in a method, the more difficult it is to understand and use
		// -- CheckNotifications(null, 1, true, false, DateTime.Now);

		// - If you have parameters that are used often together, encapsulate them in a class to reduce the number of parameters

		// - Try to limit the number of parameters to 3 or 4
		#endregion

		#region Output Parameters
		// int count = 0;
		// bool more = false;
		// var customers = GetCustomers(pageIndex, out count, out more);

		// - It doesn't really make sense to pass data in and return from the arguments.
		// - Unless you're working with a method that returns a flag and outs a value

		// - Return an object from a method instead of using output parameters
		#endregion

		#region Variable Declarations on the Top
		// - In methods with variables declared at the top instead of where theyre used, the reader will have to scroll up and down to understand the code

		// - Declare variables close to their usage
		#endregion

		#region Magic Numbers
		// - Avoid magic numbers at all times, they are hard to understand and can lead to unexpected behavior
		#endregion

		#region Nested Conditionals
		// - Nested conditionals make programs hard to read and test

		// - Using the Ternary operator: '?' can help reduce nesting
		public void TernaryOperatorExample() {
			if (1 == 1) {
				Console.WriteLine("Yes");
			} else {
				Console.WriteLine("No");
			}

			// Using the ternary operator the above code can be written as:
			Console.WriteLine(1 == 1 ? "Yes" : "No");
		}

		// - Do not go overboard and use them more than once
		// c = a ? b : d ? e : f;

		// - Guard statements can be used to reduce nesting
		#endregion

		#region Switch Statments
		// Switch statements violate the Open/Closed Principle
		// - They can be replaced with polymorphism

		// - Move the logic from switch statements to the derived classes
		// -- Use Push Member Down refactoring to move the method to the base class
		#endregion

		#region Comments
		// - Don't write comments, re-write your code!
		// -- Comments are not a substitute for clean code
		// -- var pf = 10; // Price Factor
		// --- Here the variable should be named priceFactor and the comment is not needed

		// - Comments that state the obvious are not useful

		// - Comments that are out of date are worse than no comments

		// - Dead code should be removed, not commented out. You can always pull it back from the version control system

		// - Comments should explain "whys" and "hows", not "whats"
		// - Examples of good comments: todo, solving a problem with constraints
		#endregion

		#endregion
	}
}