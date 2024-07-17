
using System;

namespace CleanCode.DuplicatedCode {
	public class Time {
		public int Hours { get; set; }
		public int Minutes { get; set; }

		public Time(int hours, int minutes) {
			Hours = hours;
			Minutes = minutes;
		}

		public static Time Parse(string str) {
			int hours = 0;
			int minutes = 0;
			if (!string.IsNullOrWhiteSpace(str)) {
				int.TryParse(str.Replace(":", ""), out int t);
				hours = t / 100;
				minutes = t % 100;
			} else {
				throw new ArgumentNullException("str");
			}

			if (hours >= 24 || minutes >= 60) {
				throw new ArgumentException("Invalid time format");
			}

			return new Time(hours, minutes);
		}
	}

	public class DuplicatedCode {

		public void AdmitGuest(string name, string admissionDateTime) {
			// Some logic 
			// ...

			Time time = Time.Parse(admissionDateTime);

			// Some more logic 
			// ...
			if (time.Hours < 10) {

			}
		}

		public void UpdateAdmission(int admissionId, string name, string admissionDateTime) {
			// Some logic 
			// ...

			Time time = Time.Parse(admissionDateTime);

			// Some more logic 
			// ...
			if (time.Hours < 10) {

			}
		}
	}
}
