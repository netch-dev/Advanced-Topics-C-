using System;
using System.Web;

// This class displays the web page
namespace FooFoo {
	public partial class Download : System.Web.UI.Page {

		private readonly DataTableToCsvMapper _dataTableToCsvMapper = new DataTableToCsvMapper();
		private readonly TableReader _tableReader = new TableReader();
		protected void Page_Load(object sender, EventArgs e) {
			ClearResponse();

			SetCache();

			WriteContentToResponse(GetCsv());
		}

		private byte[] GetCsv() {
			System.IO.MemoryStream ms = _dataTableToCsvMapper.Map(_tableReader.GetDataTable("FooFoo"));
			byte[] byteArray = ms.ToArray();
			ms.Flush();
			ms.Close();
			return byteArray;
		}

		private static void WriteContentToResponse(byte[] byteArray) {
			Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
			Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
			Response.ContentType = "text/comma-separated-values";
			Response.AddHeader("Content-Disposition", "attachment; filename=FooFoo.csv");
			Response.AddHeader("Content-Length", byteArray.Length.ToString());
			Response.BinaryWrite(byteArray);
		}

		private static void SetCache() {
			Response.Cache.SetCacheability(HttpCacheability.Private);
			Response.CacheControl = "private";
			Response.AppendHeader("Pragma", "cache");
			Response.AppendHeader("Expires", "60");
		}

		private static void ClearResponse() {
			Response.Clear();
			Response.ClearContent();
			Response.ClearHeaders();
			Response.Cookies.Clear();
		}
	}
}