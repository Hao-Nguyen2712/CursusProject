namespace Cursus.MVC.Models
{
	public class TradingViewModel
	{
		public int TdId { get; set; }
		public DateTime? TdDate { get; set; }
		public decimal? TdMoney { get; set; }
		public string TdMethodPayment { get; set; }
		public int? AccountId { get; set; }
	}
}
