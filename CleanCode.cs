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

		#endregion
	}
}