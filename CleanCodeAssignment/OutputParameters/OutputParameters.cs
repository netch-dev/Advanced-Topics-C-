﻿using CleanCode.Comments;
using System;
using System.Collections.Generic;

namespace CleanCode.OutputParameters {
	public class GetCustomersResult {
		public IEnumerable<Customer> Customers { get; set; }
		public int TotalCount { get; set; }
	}

	public class OutputParameters {
		public void DisplayCustomers() {
			GetCustomersResult customersResult = GetCustomers(pageIndex: 1);

			Console.WriteLine("Total customers: " + customersResult.TotalCount);
			foreach (Customer c in customersResult.Customers) {
				Console.WriteLine(c);
			}
		}

		public GetCustomersResult GetCustomers(int pageIndex) {
			int totalCount = 100;
			return new GetCustomersResult {
				Customers = new List<Customer>(),
				TotalCount = totalCount
			};
		}
	}
}
