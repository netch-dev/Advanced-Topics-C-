using System.Configuration;
using System.Data;
using System.Data.SqlClient;

// This class is purely responsible for getting data from the database
internal class TableReader {
	public DataTable GetDataTable(string tableName) {
		string strConn = ConfigurationManager.ConnectionStrings["FooFooConnectionString"].ToString();
		SqlConnection conn = new SqlConnection(strConn);
		SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM [{tableName}] ORDER BY id ASC", conn);
		DataSet dataSet = new DataSet();
		dataAdapter.Fill(dataSet, $"{tableName}");
		DataTable dataTable = dataSet.Tables[$"{tableName}"];
		return dataTable;
	}
}