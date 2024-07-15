using System;

namespace CleanCode.NestedConditionals {
	public class Customer {
		public int LoyaltyPoints { get; set; }

		public bool IsGoldMember() {
			return LoyaltyPoints > 100;
		}
	}

	public class Reservation {
		public Reservation(Customer customer, DateTime dateTime) {
			From = dateTime;
			Customer = customer;
		}

		public DateTime From { get; set; }
		public Customer Customer { get; set; }
		public bool IsCanceled { get; set; }

		public void Cancel() {
			if (IsCancellationPeriodOver()) {
				throw new InvalidOperationException("It's too late to cancel.");
			}

			IsCanceled = true;
		}

		private bool IsCancellationPeriodOver() {
			int maxHours = Customer.IsGoldMember() ? 24 : 48;
			return (From - DateTime.Now).TotalHours < maxHours;
		}
	}
}
