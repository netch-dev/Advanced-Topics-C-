using CleanCode.SwitchStatements;

namespace Advanced_Topics_C_.CleanCodeAssignment.SwitchStatements {
	public class UnlimitedCustomer : PhoneCustomer {
		public override MonthlyStatement GetMonthlyStatement(MonthlyUsage monthlyUsage) {
			MonthlyStatement monthlyStatement = new MonthlyStatement();
			monthlyStatement.TotalCost = 54.90f;
			return monthlyStatement;
		}
	}
}
