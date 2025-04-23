using Cursus.Application.lib;
using Cursus.Application.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Cursus.Domain.Models;

namespace Cursus.Application.Credits
{
    public class CreditsService : ICreditsService
    {
        private readonly ICreditsRepository _creditsRepository;

		public CreditsService(ICreditsRepository creditsRepository)
		{
			_creditsRepository = creditsRepository;
		}

		private const string ApiBaseUrl = "https://v6.exchangerate-api.com/v6/";
        private const string ApiKey = "50b9de7e7d868bcac5711146";
        public string CreateRequestUrl(double money, string bankCode)
        {
            DateTime currentDate = DateTime.Now;
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", "7OB7OEJA");
            vnpay.AddRequestData("vnp_Amount", (money * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", currentDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            if (bankCode == "QR")
            {
                vnpay.AddRequestData("vnp_BankCode", "");
            }
            else if (bankCode == "Bank")
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (bankCode == "InternationalBank")
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng");
            vnpay.AddRequestData("vnp_OrderType", "order"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", "https://cursusmvc20240730144315.azurewebsites.net/Credits/Return");
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentURl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "BO57TPO2AHVZL3OMMIHKEK7XN4Q6ETF5");
            return paymentURl;
        }

        public async Task<double> ConvertUSDToVND(double usdAmount)
        {
            using (var client = new HttpClient())
            {
                // Build the request URL
                string requestUrl = $"{ApiBaseUrl}{ApiKey}/latest/USD";

                try
                {
                    // Send GET request to API
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read response content as JSON
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var jsonObject = JObject.Parse(jsonContent);

                        // Extract exchange rate for USD to VND
                        var conversionRate = (double)jsonObject["conversion_rates"]["VND"];

                        // Perform conversion
                        double vndAmount = usdAmount * conversionRate;

                        return vndAmount;
                    }
                    else
                    {
                        // Handle unsuccessful response
                        throw new HttpRequestException("Failed to retrieve exchange rates.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Console.WriteLine($"Error retrieving exchange rates: {ex.Message}");
                    throw;
                }
            }
        }

        public VnPaymentResponseViewModel PaymentExcute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderID = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, "BO57TPO2AHVZL3OMMIHKEK7XN4Q6ETF5");
            if (!checkSignature)
            {
                return new VnPaymentResponseViewModel
                {
                    Success = false,
                    // VnPayResponseCode = "97",
                    // OrderDescription = "Lỗi xác thực dữ liệu",
                    // OrderId = vnp_orderID.ToString(),
                    // PaymentId = vnp_vnpayTranId.ToString(),
                    // TransactionId = vnp_vnpayTranId.ToString(),
                    // Token = vnp_vnpayTranId.ToString()
                };
            }
            else
            {
                return new VnPaymentResponseViewModel
                {
                    Success = true,
                    PaymentMethod = "VnPay",
                    VnPayResponseCode = vnp_ResponseCode,
                    OrderDescription = vnp_OrderInfo,
                    OrderId = vnp_orderID.ToString(),
                    PaymentId = vnp_vnpayTranId.ToString(),
                    TransactionId = vnp_vnpayTranId.ToString(),
                    Token = vnp_vnpayTranId.ToString()
                };
            }
        }

		public Domain.Models.Account AddAccMoney(string userID, int accMoney)
		{
            return _creditsRepository.AddAccMoney(userID, accMoney);
		}

		public double GetAccMoney(string userID)
		{
            double accMoney = _creditsRepository.GetAccMoney(userID);
            return accMoney;
		}

		public List<Trading> GetAllTrading(string userID)
		{
            var listTrading = _creditsRepository.GetAllTrading(userID);
            return listTrading;
		}

		public Trading AddTrading(Trading trading, string userID)
		{
            if (trading == null)
            {
                return null;
            } _creditsRepository.AddTrading(trading, userID);
            return trading;
		}
	}
}

