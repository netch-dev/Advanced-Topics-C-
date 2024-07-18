using System;
using System.Data;

// This class is purely responsible for converting a DataTable to a CSV file
public class DataTableToCsvMapper {
	public System.IO.MemoryStream Map(DataTable dt) {
		MemoryStream ReturnStream = new MemoryStream();

		StreamWriter sw = new StreamWriter(ReturnStream);
		WriteColumnNames(dt, sw);
		WriteRows(dt, sw);
		sw.Flush();
		sw.Close();

		return ReturnStream;
	}

	private static void WriteRows(DataTable dt, StreamWriter sw) {
		foreach (DataRow dr in dt.Rows) {
			WriteRow(dt, dr, sw);
			sw.WriteLine();
		}
	}

	private static void WriteRow(DataTable dt, DataRow dr, StreamWriter sw) {
		for (int i = 0; i < dt.Columns.Count; i++) {
			WriteCell(dr[i], sw);
			WriteSeparatorIfRequired(dt, i, sw);
		}
	}

	private static void WriteSeparatorIfRequired(DataTable dt, int columnIndex, StreamWriter sw) {
		if (columnIndex < dt.Columns.Count - 1) {
			sw.Write(",");
		}
	}

	private static void WriteCell(object cellObject, StreamWriter sw) {
		if (!Convert.IsDBNull(cellObject)) {
			string str = String.Format("\"{0:c}\"", cellObject.ToString()).Replace("\r\n", " ");
			sw.Write(str);
		} else {
			sw.Write("");
		}
	}

	private static void WriteColumnNames(DataTable dt, StreamWriter sw) {
		for (int i = 0; i < dt.Columns.Count; i++) {
			sw.Write(dt.Columns[i]);
			if (i < dt.Columns.Count - 1) {
				sw.Write(",");
			}
		}
		sw.WriteLine();
	}
}
