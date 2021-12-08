using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Service
{
    public class SendSmsService : ISendSmsService
    {
        private readonly IHttpClientFactory _ClientFactory;
        private readonly IlogService _ilog;

        public SendSmsService(IHttpClientFactory httpClientFactory, IlogService ilog)
        {
            _ClientFactory = httpClientFactory;
            _ilog = ilog;
        }
        public async Task<(bool isSuccess, string error)> SendMessage(string To, string data, string pattern)
        {
            if (string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(pattern))
            {
                return (false, "ورودی ها نادرست است");
            }

            try
            {
                var number = To.Insert(0, "98").Remove(2, 1);
                string from = "test";
                string userName = "test";
                string pass = "test";
                string patternCode = pattern;
                string to = JsonConvert.SerializeObject(new string[] { number });
                string input_data = data;

                string url = $@"";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var client = _ClientFactory.CreateClient();
                var response = await client.SendAsync(request);
                response.Dispose();
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessage");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendMessageForAcceptAdver(string name, string phoneNumber)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["company"] = name,
                }), "xv86vjt2qe");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessageForAcceptAdver");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendMessageForPendindAdver(string name, string phoneNumber)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["company"] = name,
                }), "blu4tcmsce");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessageForPendindAdver");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendMessageForRejectAdver(string name, string phoneNumber)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["company"] = name,
                }), "2mcoqkxg3w");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessageForRejectAdver");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendMessageForReturnAdver(string name, string phoneNumber)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["company"] = name,
                }), "kb88rxmpjo");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessageForReturnAdver");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendVerificationCode(string name,string phoneNumber, string code)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["name"] = name,
                    ["code"] = code
                }), "uizo1k17yt");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendVerificationCode");

                return (false, "مشکلی رخ داده است");
            }
        }
        public async Task<(bool isSuccess, string error)> SendGiftCodeCode(string name, string phoneNumber, string code)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["name"] = name,
                    ["code"] = code
                }), "gx06wdsjkg");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendGiftCodeCode");

                return (false, "مشکلی رخ داده است");
            }
        }
        /// <summary>
        /// this function for Admin When Send GiftCode
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<(bool isSuccess, string error)> SendNewGiftCode(string name, string phoneNumber, string code)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["name"] = name,
                    ["code"] = code
                }), "gx06wdsjkg");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendNewGiftCode");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendMessageForBackMoney(string phoneNumber,  string trackingNumber)
        {
            //
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["trackingNubmber"] = trackingNumber,
                }), "tv42vbt9l9");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendMessageForBackMoney");

                return (false, "مشکلی رخ داده است");
            }
        }




        public async Task<(bool isSuccess, string error)> SendConfirmationStatusToEmployee(string status, string phoneNumber,  string JobAdvertisementTitle, int JobAdvertisementId)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["status"] = status,
                    ["JobAdvertisementTitle"] = JobAdvertisementTitle,
                    ["JobAdvertisementId"] = JobAdvertisementId.ToString(),
                }), "o1ybkh22l2");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendVerificationCode");
                return (false, "مشکلی رخ داده است");
            }
        }




        public async Task<(bool isSuccess, string error)> SendNewResomeComeInToEmployer(string name, string phoneNumber, string JobAdvertisementTitle)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    //["status"] = status,
                    ["JobAdvertisementTitle"] = JobAdvertisementTitle
                }), "gqd40wym99");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendVerificationCode");
                return (false, "مشکلی رخ داده است");
            }
        }
        public async Task<(bool isSuccess, string error)> SendForNewTicket(string phoneNumber, string title,string date)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["title"] = title,
                    ["date"] = date,
                }), "8zesqmtm6q");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendVerificationCode");
                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error)> SendForCreateGiftCode(string phoneNumber, string percent, string days, string code)
        {
            try
            {
                await SendMessage(phoneNumber, JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["percent"] = percent,
                    ["days"] = days,
                    ["code"] = code
                }), "t11smk21wk");
                return (true, "با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SendSms", "SendVerificationCode");
                return (false, "مشکلی رخ داده است");
            }
        }
    }
}
