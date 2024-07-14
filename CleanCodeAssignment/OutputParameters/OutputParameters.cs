using CleanCode.Comments;
using System;
using System.Collections.Generic;

namespace CleanCode.OutputParameters {
	public class OutputParameters {
		public void DisplayCustomers() {
			int totalCount = 0;
			Tuple<IEnumerable<Customer>, int> tuple = GetCustomers(1);
			totalCount = tuple.Item2;
			IEnumerable<Customer> customers = tuple.Item1;

			Console.WriteLine("Total customers: " + totalCount);
			foreach (Customer c in customers) {
				Console.WriteLine(c);
			}
		}

		public Tuple<IEnumerable<Customer>, int> GetCustomers(int pageIndex) {
			int totalCount = 100;
			return Tuple.Create((IEnumerable<Customer>)new List<Customer>(), totalCount);
		}
	}
}
