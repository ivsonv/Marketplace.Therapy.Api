using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.dto.payment;
using Marketplace.Domain.Models.dto.provider;
using Marketplace.Integrations.Payment.Nexxera.repository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Marketplace.Integrations.Payment.Nexxera
{
    public static class NexxeraClient
    {
        #region ..:  dados para teste :..

        //private static string TokenDeAcesso = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3MiOlsiY2FyZFBheW1lbnRzIiwicmVjdXJyZW5jZVBsYW5zIiwicmVjdXJyZW5jZXMiLCJjaGVja291dCIsImJvbGV0b1BheW1lbnRzIl0sImlzcyI6ImFkOGI0M2M1LTFmZWQtNDc5Ni1hMTg3LTFiZjMzZDM2OTFiMSIsImV4cCI6MTg4NjY3NjkzNX0.I-LqnRYbkO7YjmblPomBtU-BeV6lVCgf7XywrTZyJzg";
        //public static readonly string CheckOutURL = "https://checkout-nix-qa.cloudint.nexxera.com/checkout/"; // TST
        //public static readonly string ApiUrl = "https://gateway-nix-qa.cloudint.nexxera.com/v2/"; // TST
        //public static readonly string MerchantApiUrl = "https://merchant-sign-up-core-receivable-gateway-qa.cloudint.nexxera.com/registration/api/"; // TST
        ////URL Base QA/Testes: https://merchant-sign-up-core-receivable-gateway-qa.cloudint.nexxera.com/registration/api
        //private readonly static string TokenDeAcessoMerchantApiURl = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9kdWN0SWQiOiJhNTM3NzM1NC02NjJlLTQ3MDEtZTQxOS0wOGQzYzIyNmEwMTUiLCJjYXJkUmVwb3NpdG9yeUlkIjoiOTUxMDg4ZWUtMjNjYi00YzU5LTlmZmQtYjY0ZjM4MzEwYmVlIiwic3ViIjoiOWNmZTQ2NDctMzY1OS00ZWM1LTg3ZGItMDhkN2Q1OTYxOTcxIiwiYWNjZXNzIjoibWVyY2hhbnRTaWduVXAifQ.5YLDyXBKBI38p2Zu8fB7CdJM0Ot7m8hiEWk1L7rI4hc";
        #endregion

        #region ..: Dados para produção :..

        // URL Base Produção:  https://merchant-sign-up-core-receivable-gateway-prd.nexxera.io/registration/api/
        //Produção
        // ProductID: 63245ee3-81bb-4fc3-ec31-08d39e8e2006
        // Card Repository ID : dbc09b29-43ab-430c-a98a-ec648fec7121

        private readonly static string TokenDeAcessoMerchantApiURl = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9kdWN0SWQiOiJhNTM3NzM1NC02NjJlLTQ3MDEtZTQxOS0wOGQzYzIyNmEwMTUiLCJjYXJkUmVwb3NpdG9yeUlkIjoiOTUxMDg4ZWUtMjNjYi00YzU5LTlmZmQtYjY0ZjM4MzEwYmVlIiwic3ViIjoiOWNmZTQ2NDctMzY1OS00ZWM1LTg3ZGItMDhkN2Q1OTYxOTcxIiwiYWNjZXNzIjoibWVyY2hhbnRTaWduVXAifQ.5YLDyXBKBI38p2Zu8fB7CdJM0Ot7m8hiEWk1L7rI4hc";
        private static string TokenDeAcesso = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3MiOlsiY2FyZFBheW1lbnRzIiwicmVjdXJyZW5jZVBsYW5zIiwicmVjdXJyZW5jZXMiLCJjaGVja291dCIsImJvbGV0b1BheW1lbnRzIl0sImlzcyI6IjY2ODkxYWYyLWVkZDMtNDAyOS1hYzcwLTUyMWUxODI1MDQ3ZCIsImV4cCI6MTkwNDgxNDk3NH0.v7wcfTBDHgFlvLZr7wzIrMztaV0-e-MFUIkj81X6cBo";
        private static readonly string CheckOutURL = "https://checkout-nix.nexxera.io/checkout/"; // PROD
        private static readonly string ApiUrl = "https://gateway-nix.nexxera.io/v2/"; // PROD
        private static readonly string MerchantApiUrl = "https://merchant-sign-up-core-receivable-gateway-prd.nexxera.io/registration/api/products/a5377354-662e-4701-e419-08d3c226a015/Customers/merchant/signup";
        #endregion

        private static void IsValidarMerchant(providerDto provider)
        {
            if (provider.bankAccounts.IsEmpty())
                throw new ArgumentException("Merchant sem dados bancários");

            if (provider.splitAccounts.Any(a => a.payment_provider == Enumerados.PaymentProvider.nexxera))
                throw new ArgumentException($"split já criado anteriormente para junto a nexxera.");

            var bank = provider.bankAccounts.First();
            if (bank.bank_code.IsEmpty())
                throw new ArgumentException("Merchant sem banco selecionado.");

            if (bank.agency_number.IsEmpty())
                throw new ArgumentException("Merchant sem Número de agencia no cadastro dados bancarios.");

            if (bank.account_number.IsEmpty())
                throw new ArgumentException("Merchant sem Número de conta no cadastro dados bancarios.");

            if (bank.account_digit.IsEmpty())
                throw new ArgumentException("Merchant sem Digito de conta no cadastro dados bancarios.");
        }
        public static void CreateMerchant(providerDto provider)
        {
            IsValidarMerchant(provider);

            string _operation = (provider.bankAccounts.First().operation.IsNotEmpty()
                ? provider.bankAccounts.First().operation
                : provider.bankAccounts.First().operation).Trim();

            // body
            string corporate = $"PSYCHEIN {provider.fantasy_name} {provider.company_name}";
            if (corporate.Length > 50) corporate = corporate.Substring(0, 49);

            string soft = $"PSI{provider.fantasy_name}";
            if (soft.Length > 11) soft = soft.Substring(0, 10);

            // request
            var _request = new repository.MerchantRq()
            {
                CorporateName = corporate,
                SocialNumber = !provider.cpf.IsEmpty() ? provider.cpf : provider.cnpj,
                SocialNumberType = !provider.cpf.IsEmpty() ? "CPF" : "CNPJ",
                emailAdmin = "ivsonv@gmail.com",
                SoftDescriptor = soft,
                Address = new repository.Address()
                {
                    Neighborhood = provider.address.First().neighborhood,
                    ZipCode = provider.address.First().zipcode,
                    Street = provider.address.First().address,
                    Number = provider.address.First().number,
                    City = provider.address.First().city,
                    State = provider.address.First().uf,
                    Country = "Brasil"
                },
                // AccountType
                //0 – CheckingAccount(Conta corrente)
                //1 – SavingAccount(Conta poupança)
                //2 – PrePaidSavingAccount(Conta pré - paga)
                BankAccount = new repository.BankAccount()
                {
                    AccountType = ((int)provider.bankAccounts.First().account_bank_type).ToString(),
                    BranchDigit = provider.bankAccounts.First().agency_digit,
                    Number = $"{_operation}{provider.bankAccounts.First().account_number}",
                    Branch = provider.bankAccounts.First().agency_number,
                    Digit = provider.bankAccounts.First().account_digit,
                    BankId = provider.bankAccounts.First().bank_code,
                    Holder = new repository.Holder()
                    {
                        Name = $"{provider.fantasy_name} {provider.company_name}",
                        SocialNumber = !provider.cpf.IsEmpty() ? provider.cpf : provider.cnpj
                    }
                },
                Contacts = new List<repository.Contact>()
                {
                    new repository.Contact()
                    {
                        Name = $"{provider.fantasy_name} {provider.company_name}",
                        PhoneNumber = provider.phone,
                        Email = provider.email
                    }
                }
            };

            try
            {
                string RsJSON = postMerchant(MerchantApiUrl, _request.Serialize());
                var _return = RsJSON.Deserialize<MerchantRs>();
                if (_return.errors != null && _return.errors.Any())
                    throw new ArgumentException($"Erro ao criar estabelecimento (${string.Join("#", _return.errors)})");

                if (_return.production == null && _return.sandbox == null)
                    throw new ArgumentException($"{_return.message} (status: {_return.status})");

                // popular
                provider.splitAccounts = new List<Domain.Entities.ProviderSplitAccount>()
                {
                    new Domain.Entities.ProviderSplitAccount() {
                        payment_provider = Enumerados.PaymentProvider.nexxera,
                        json = RsJSON
                    }
                };
            }
            catch { throw; }
        }

        public static void Buy(PaymentDto _payment)
        {
            _payment.payments.ForEach(_pay =>
            {
                // account merchant
                var _account = _pay.Provider.splitAccounts.First(f => f.payment_provider == Enumerados.PaymentProvider.nexxera);

                // split merchant
                var _split = _account.json.Deserialize<MerchantRs>();

                // token transaction
                TokenDeAcesso = GerarTokenJWT(_split.production.accessKey, _split.production.apiSecretKey);

                // request
                var req = new CheckOutRq()
                {
                    returnUrl = $"{_pay.webhook_url}/payment?code={_pay.productSale.id}",
                    merchantOrderId = _pay.productSale.id.ToString(),
                    amount = ((int)_pay.totalprice).ToString(),
                    installments = 1
                };

                #region ..: Template :..

                req.template = new CheckOutTemplate()
                {
                    isFullScreen = false,
                    useSummaryItemTemplate = false,
                    backgroundBodyColor = "#fafafa",
                    secundaryColor = "#82868b",
                    primaryColor = "#9aa1b0",
                    logoURL = "https://imagem.cliqueterapia.com.br/terms/logo.png"
                };
                #endregion

                #region ..: customer :..

                req.customer = new CheckOutCustomer()
                {
                    identity = _pay.Customer.cpf.IsNotEmpty() ? _pay.Customer.cpf : _pay.Customer.cnpj,
                    identityType = _pay.Customer.cpf.IsNotEmpty() ? "CPF" : "CNPJ",
                    name = _pay.Customer.name.ToUpper(),
                    email = _pay.Customer.email,
                    birthdate = ""
                };
                #endregion

                #region ..: opções de pagamento :..

                // debito
                //req.debitCard = new CheckOutdebitCard() { amount = (int)_pay.totalprice };

                // credito
                req.creditCard = new CheckOutcreditCard()
                {
                    installments = new List<CheckOutcreditCardinstallments>()
                    {
                        new CheckOutcreditCardinstallments()
                        {
                            template = "1x de R$ " + decimal.Round(((decimal)_pay.totalprice / 1), 2).ToString("N2"),
                            amount = ((int)_pay.totalprice).ToString(),
                            Number = 1,
                        },
                        //new repository.CheckOutcreditCardinstallments()
                        //{
                        //    template = "2x de R$ " + decimal.Round(((decimal)_pay.totalprice / 2), 2).ToString("N2"),
                        //    amount = ((int)_pay.totalprice).ToString(),
                        //    Number = 2,
                        //}
                    }
                };
                #endregion

                #region ..: description :..

                // description
                req.items = new List<string>()
                {
                    $"TERAPIA ONLINE COM {_pay.Provider.fantasy_name} {_pay.Provider.company_name}"
                };
                #endregion

                string rq = req.Serialize();
                string rs = "";

                try
                {
                    //send
                    rs = post(CheckOutURL, req.Serialize(), "Order");

                    // convert
                    var chRs = rs.Deserialize<CheckOutRs>();

                    // erro generico
                    if (chRs.errors.IsNotEmpty())
                        throw new ArgumentException($"Erro ao processar pagamento, {string.Join("#", chRs.errors)}");

                    // url de checkout
                    if (chRs.checkoutUrl.IsEmpty())
                        throw new ArgumentException($"Erro ao processar checkout, {string.Join("#", chRs.errors)}");

                    // url redirect
                    _pay.transactionUrl = chRs.checkoutUrl;

                    // transaction code
                    if (chRs.checkoutUrl.Contains("payment?orderId="))
                        _pay.transactionCode = chRs.checkoutUrl.Split("payment?orderId=")[1];
                }
                catch { throw; }
                finally
                {
                    _pay.LogRq = rq;
                    _pay.LogRs = rs;
                };
            });
        }
        public static void Cancel(PaymentDto _payment)
        {
            _payment.payments.ForEach(_pay =>
            {
                // account merchant
                var _account = _pay.Provider.splitAccounts.First(f => f.payment_provider == Enumerados.PaymentProvider.nexxera);

                // split merchant
                var _split = _account.json.Deserialize<MerchantRs>();

                // token transaction
                TokenDeAcesso = GerarTokenJWT(_split.production.accessKey, _split.production.apiSecretKey);

                // request
                var req = new CancelRq()
                {
                    amount = _pay.totalprice.ToString("N2").Replace(",", "").Replace(".", ""),
                    paymentToken = _pay.transactionCode
                };

                string rq = req.Serialize();
                string rs = "";

                try
                {
                    //send
                    rs = Put(ApiUrl + "Orders/CardPayments/Reverse", req.Serialize());

                    // convert
                    var chRs = rs.Deserialize<CancelRs>();

                    // erro generico
                    if (chRs.errors.IsNotEmpty())
                        throw new ArgumentException($"Erro ao cancelar pagamento, {string.Join("#", chRs.errors)}");

                    // cancel
                    _pay.cancel = true;
                }
                catch { throw; }
                finally
                {
                    _pay.LogRq = rq;
                    _pay.LogRs = rs;
                };
            });
        }
        public static void Search(PaymentDto dto)
        {
            var _pay = dto.payments[0];
            string rs = "";

            try
            {
                // account merchant
                var _account = _pay.Provider.splitAccounts.First(f => f.payment_provider == Enumerados.PaymentProvider.nexxera);

                // split merchant
                var _split = _account.json.Deserialize<MerchantRs>();

                // token transaction
                TokenDeAcesso = GerarTokenJWT(_split.production.accessKey, _split.production.apiSecretKey);
                rs = Get(ApiUrl, $"orders/cardpayments/{_pay.transactionCode}");

                if (!rs.IsNotEmpty())
                    throw new ArgumentException($"Codigo de transação informado não existe.");

                // convert
                var payRs = rs.Deserialize<repository.ConsultRs>();

                // trata erros
                if (payRs.errors.IsNotEmpty())
                    throw new ArgumentException($"Erro ao verificar pagamento, {string.Join("#", payRs.errors)}");

                // retorno
                // 0 - Iniciado
                // 1 - Pré - autorizado
                // 2 - Capturado
                // 3 - Cancelado
                // 4 - Em análise
                // 9 - Não autorizado
                switch (payRs.payment.paymentStatus.Trim())
                {
                    case "2":
                        _pay.paymentStatus = Enumerados.PaymentStatus.confirmed;
                        _pay.status = Enumerados.AppointmentStatus.confirmed;
                        break;
                    case "3":
                        _pay.paymentStatus = Enumerados.PaymentStatus.canceled;
                        _pay.status = Enumerados.AppointmentStatus.canceled;
                        break;
                    case "9":
                        _pay.paymentStatus = Enumerados.PaymentStatus.notAuthorized;
                        _pay.status = Enumerados.AppointmentStatus.notAuthorized;
                        break;
                    default:
                        _pay.paymentStatus = Enumerados.PaymentStatus.pending;
                        _pay.status = Enumerados.AppointmentStatus.pending;
                        break;
                }
            }
            catch { throw; }
            finally
            {
                _pay.LogRq = null;
                _pay.LogRs = rs;
            };

        }
        private static string dsPayment(string codigo)
        {

            switch (codigo.Trim())
            {
                case "0": return "iniciado";
                case "1": return "pre-autorizado";
                case "2": return "capturado";
                case "3": return "cancelado";
                case "4": return "em analise";
                case "9": return "nao autorizado";

                default:
                    return "desconhecido: " + codigo;
            }
        }

        private static string GerarTokenJWT(string accesskey, string apikey)
        {
            byte[] key = Convert.FromBase64String(apikey);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(signingCredentials: credentials);
            token.Payload.Add("access", new string[] { "cardPayments", "recurrencePlans", "recurrences", "checkout", "boletoPayments" });
            token.Payload.Add("iss", accesskey);
            token.Payload.Add("exp", 1906911936);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        private static string postMerchant(string url, string json)
        {
            string res = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Authorization", "JWT-TOKEN " + TokenDeAcessoMerchantApiURl);
                request.ContentType = "application/json";
                request.UseDefaultCredentials = true;
                request.Method = "POST";

                var byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(byteArray, 0, byteArray.Length);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    res = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }
                }

                if (res.IsEmpty())
                    throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return res;
        }

        private static string post(string url, string json, string scao)
        {
            string res = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + scao);
                request.Headers.Add("Authorization", "Bearer " + TokenDeAcesso);
                request.ContentType = "application/json";
                request.UseDefaultCredentials = true;
                request.Method = "POST";

                var byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(byteArray, 0, byteArray.Length);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    res = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(res))
                    throw;
            }
            catch { throw; }

            return res;
        }
        private static string Put(string url, string json)
        {
            string res = "";
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add("Authorization", "Bearer " + TokenDeAcesso);
                request.Headers.Add("RequestId", CustomExtensions.getGuid);
                request.ContentType = "application/json";
                request.UseDefaultCredentials = true;
                request.Method = "PUT";

                var encoding = new System.Text.ASCIIEncoding();
                var byteArray = encoding.GetBytes(json);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            res = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }
                }
                if (string.IsNullOrWhiteSpace(res))
                    throw new ArgumentException(ex.Message);
            }
            catch { throw; }
            return res;
        }
        private static string Get(string url, string scao)
        {
            string res = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + scao);
                request.Headers.Add("Authorization", "Bearer " + TokenDeAcesso);
                request.ContentType = "application/json";
                request.UseDefaultCredentials = true;
                request.Method = "GET";

                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        res = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex) { throw; }

            return res;
        }
    }
}