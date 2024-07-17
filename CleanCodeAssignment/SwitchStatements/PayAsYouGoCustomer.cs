using CleanCode.SwitchStatements;

namespace Advanced_Topics_C_.CleanCodeAssignment.SwitchStatements {
	public class PayAsYouGoCustomer : PhoneCustomer {
		public override MonthlyStatement GetMonthlyStatement(MonthlyUsage monthlyUsage) {
			MonthlyStatement monthlyStatement = new MonthlyStatement();
			monthlyStatement.CallCost = 0.12f * monthlyUsage.CallMinutes;
			monthlyStatement.SmsCost = 0.12f * monthlyUsage.SmsCount;
			monthlyStatement.TotalCost = monthlyStatement.CallCost + monthlyStatement.SmsCost;
			return monthlyStatement;
		}
	}
}
