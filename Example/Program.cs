using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TessPgw.Checkout;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var checkout = new CheckoutService("SANDBOX");

                // Simple Checkout
                //var simpleCheckout = SimpleCheckout(checkout);

                // 3DS Checkout
                //var threeDsInitPayment = ThreeDsInitPayment(checkout);

                var threeDsInitPayment = ThreeDsInitPayment(checkout);


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:");
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static Dictionary<string, object> SimpleCheckout(CheckoutService checkout)
        {
            var data = new Dictionary<string, object>
                {
                    { "operation", "purchase" },
                    { "methods", new List<string> { "applepay", "om-wallet", "card", "naps" } },
                    { "order", new Dictionary<string, object>
                        {
                            { "number", "ORDER123456" },
                            { "amount", "100.00" },
                            { "currency", "QAR" },
                            { "description", "Test 4" }
                        }
                    },
                    { "billing_address", new Dictionary<string, object>
                        {
                            { "country", "QA" },
                            { "state", "QA" },
                            { "zip", "123456" }
                        }
                    },
                    { "cancel_url", "https://example.com/cancel" },
                    { "success_url", "https://example.com/success" },
                    { "customer", new Dictionary<string, object>
                        {
                            { "name", "John Doe" },
                            { "email", "test@gmail.com" }
                        }
                    }
                };

            var response = checkout.StandardPayment(data);

            Console.WriteLine("Response:");
            foreach (var kv in response)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            return response;
        }

        static Dictionary<string, object> ThreeDsInitPayment(CheckoutService checkout)
        {
            var data = new Dictionary<string, object>
                {
                    { "operation", "purchase" },
                    { "methods", new List<string> { "card" } },
                    { "order", new Dictionary<string, object>
                        {
                            { "number", "ORDER123456" },
                            { "amount", "100.00" },
                            { "currency", "QAR" },
                            { "description", "Test 4" }
                        }
                    },
                    { "billing_address", new Dictionary<string, object>
                        {
                            { "country", "QA" },
                            { "state", "QA" },
                            { "zip", "123456" }
                        }
                    },
                    { "cancel_url", "https://example.com/cancel" },
                    { "success_url", "https://example.com/success" },
                    { "customer", new Dictionary<string, object>
                        {
                            { "name", "John Doe" },
                            { "email", "test@gmail.com" }
                        }
                    },
                    { "req_token", true }
                };

            var response = checkout.ThreeDsInitPayment(data);

            Console.WriteLine("Response:");
            foreach (var kv in response)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            return response;
        }
    }
}
