using System.Collections.Generic;
using TessPgw.Core;
using TessPgw.Core.Enums;
using TessPgw.Checkout;

namespace TessPgw.Checkout
{
    public class CheckoutService
    {
        private readonly string _integrationType;
        private readonly string _baseUrl;
        private readonly string _merchantId;
        private readonly string _merchantPassword;

        public CheckoutService(string integrationType = "LIVE")
        {
            _integrationType = integrationType.ToUpper();
            _baseUrl = Config.GetBaseUrl(_integrationType);
            var config = Config.Instance;
            _merchantId = config.Get("MERCHANT_ID");
            _merchantPassword = config.Get("MERCHANT_PASSWORD");
        }

        private Dictionary<string, object> PrepareRequest(Dictionary<string, object> data, Actions action)
        {
            data["merchant_key"] = _merchantId;
            data["hash"] = HashService.Generate(data, action);
            return data;
        }

        public Dictionary<string, object> StandardPayment(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.AUTHENTICATION);
            return SendRequest("api/v1/session", request);
        }

        public Dictionary<string, object> ThreeDsInitPayment(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.AUTHENTICATION);
            return SendRequest("api/v1/session", request);
        }

        public Dictionary<string, object> RefundPayment(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.REFUND);
            return SendRequest("api/v1/payment/refund", request);
        }

        public Dictionary<string, object> VoidPayment(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.VOID);
            return SendRequest("api/v1/payment/void", request);
        }

        public Dictionary<string, object> RecurringPayment(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.RECURRING);
            return SendRequest("api/v1/session", request);
        }

        public Dictionary<string, object> RetryRecurring(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.RETRY_RECURRING);
            return SendRequest("api/v1/payment/recurring", request);
        }

        public Dictionary<string, object> InquiryByPaymentId(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.INQUIRY_BY_PAYMENT_ID);
            return SendRequest("/api/v1/payment/status", request);
        }

        public Dictionary<string, object> InquiryByOrderId(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.INQUIRY_BY_ORDER_ID);
            return SendRequest("/api/v1/payment/status", request);
        }

        public Dictionary<string, object> CallbackNotification(Dictionary<string, object> data)
        {
            var request = PrepareRequest(data, Actions.CALLBACK_NOTIFICATION);
            return request;
        }

        private Dictionary<string, object> SendRequest(string endpoint, Dictionary<string, object> data)
        {
            var client = new HttpClientWrapper(_baseUrl);
            return client.Post<Dictionary<string, object>>(endpoint, data);
        }
    }
}
