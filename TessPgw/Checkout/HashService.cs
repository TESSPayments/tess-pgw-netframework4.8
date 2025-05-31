using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TessPgw.Core.Enums;
using TessPgw.Core;

namespace TessPgw.Checkout
{
    public class HashService
    {
        public static string Generate(Dictionary<string, object> parameters, Actions action)
        {
            var config = Config.Instance;
            var hashParams = new List<string>();

            switch (action)
            {
                case Actions.AUTHENTICATION:
                    hashParams.Add(GetValue(parameters, "order.number"));
                    hashParams.Add(GetValue(parameters, "order.amount"));
                    hashParams.Add(GetValue(parameters, "order.currency"));
                    hashParams.Add(GetValue(parameters, "order.description"));
                    break;

                case Actions.REFUND:
                    hashParams.Add(GetValue(parameters, "payment_id"));
                    hashParams.Add(GetValue(parameters, "amount"));
                    break;

                case Actions.VOID:
                    hashParams.Add(GetValue(parameters, "payment_id"));
                    break;

                case Actions.RECURRING:
                    hashParams.Add(GetValue(parameters, "recurring_init_trans_id"));
                    hashParams.Add(GetValue(parameters, "recurring_token"));
                    hashParams.Add(GetValue(parameters, "order.number"));
                    hashParams.Add(GetValue(parameters, "order.amount"));
                    hashParams.Add(GetValue(parameters, "order.description"));
                    break;

                case Actions.RETRY_RECURRING:
                    hashParams.Add(GetValue(parameters, "payment_id"));
                    break;

                case Actions.INQUIRY_BY_PAYMENT_ID:
                    hashParams.Add(GetValue(parameters, "payment_id"));
                    break;

                case Actions.INQUIRY_BY_ORDER_ID:
                    hashParams.Add(GetValue(parameters, "order_id"));
                    break;

                case Actions.CALLBACK_NOTIFICATION:
                    hashParams.Add(GetValue(parameters, "id"));
                    hashParams.Add(GetValue(parameters, "order_number"));
                    hashParams.Add(GetValue(parameters, "order_amount"));
                    hashParams.Add(GetValue(parameters, "order_currency"));
                    hashParams.Add(GetValue(parameters, "order_description"));
                    break;

                default:
                    throw new ArgumentException("Invalid action");
            }

            var rawString = string.Join("", hashParams);
            var merchantPass = config.Get("MERCHANT_PASSWORD");

            return Sha1Hash(Md5Hash((rawString + merchantPass).ToUpper()));
        }

        private static string GetValue(Dictionary<string, object> data, string key)
        {
            var keys = key.Split('.');
            object current = data;

            foreach (var k in keys)
            {
                if (current is Dictionary<string, object> dict && dict.ContainsKey(k))
                {
                    current = dict[k];
                }
                else
                {
                    return string.Empty;
                }
            }

            return current?.ToString() ?? string.Empty;
        }

        private static string Md5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            }
        }

        private static string Sha1Hash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            }
        }
    }
}
