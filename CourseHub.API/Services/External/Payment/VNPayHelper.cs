using CourseHub.API.Helpers.AppStart;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CourseHub.API.Services.External.Payment;

public class VNPayHelper
{
#pragma warning disable CS8618
#pragma warning disable IDE1006
    public class VNPayRequest
    {
        public string vnp_Version { get; set; } = "2.1.0";
        public string vnp_Command { get; set; } = "pay";
        public string vnp_TmnCode { get; set; }
        public int vnp_Amount { get; set; }
        public string? vnp_BankCode { get; set; }
        public string vnp_CreateDate { get; set; }
        public string vnp_CurrCode { get; set; } = "VND";
        public string vnp_IpAddr { get; set; } = "127.0.0.1";
        public string vnp_Locale { get; set; } = "vn";
        public string vnp_OrderInfo { get; set; }
        public string? vnp_OrderType { get; set; } = "other";
        public string vnp_ReturnUrl { get; set; }
        public string vnp_TxnRef { get; set; }
    }

    public class VNPayResponse
    {
        public long vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string? vnp_BankTranNo { get; set; }
        public string vnp_CardType { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }
    }
#pragma warning restore IDE1006
#pragma warning restore CS8618

    public string BuildPaymentUrl(VNPayRequest request)
    {
        PaymentOptions options = Configurer.GetPaymentOptions();
        request.vnp_TmnCode = options.TmnCode;
        // url
        // hashsecret..

        request.vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        request.vnp_TxnRef = DateTime.Now.Ticks.ToString();

        // Sorted
        SortedList<string, string?> _requestData = new(new VNPayComparer())
        {
            { nameof(request.vnp_Version), request.vnp_Version },
            { nameof(request.vnp_Command) , request.vnp_Command },
            { nameof(request.vnp_TmnCode) , request.vnp_TmnCode },
            { nameof(request.vnp_Amount), (request.vnp_Amount * 100).ToString() },
            { nameof(request.vnp_BankCode), request.vnp_BankCode },
            { nameof(request.vnp_CreateDate), request.vnp_CreateDate },
            { nameof(request.vnp_CurrCode) , request.vnp_CurrCode },
            { nameof(request.vnp_IpAddr), request.vnp_IpAddr },
            { nameof(request.vnp_Locale), request.vnp_Locale },
            { nameof(request.vnp_OrderInfo), request.vnp_OrderInfo },
            { nameof(request.vnp_OrderType), request.vnp_OrderType },
            { nameof(request.vnp_ReturnUrl), request.vnp_ReturnUrl },
            { nameof(request.vnp_TxnRef), request.vnp_TxnRef }
        };

        StringBuilder data = new();
        foreach (KeyValuePair<string, string?> kvp in _requestData)
            if (!string.IsNullOrEmpty(kvp.Value))
                data.Append(WebUtility.UrlEncode(kvp.Key) + "=" + WebUtility.UrlEncode(kvp.Value) + "&");
        string queryString = data.ToString();

        string baseUrl = options.Url;
        baseUrl += "?" + queryString;
        string signData = queryString;
        if (signData.Length > 0)
            signData = signData.Remove(data.Length - 1, 1);
        string vnp_SecureHash = HashWithHMACSHA512(options.HashSecret, signData);
        baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

        return baseUrl;
    }

    public string CheckIPN(VNPayResponse parameters, int amount, string txnRef)
    {
        System.Diagnostics.Debug.WriteLine(parameters.vnp_Amount);
        System.Diagnostics.Debug.WriteLine(parameters.vnp_ResponseCode);
        System.Diagnostics.Debug.WriteLine(parameters.vnp_ResponseCode[0]);
        if (parameters.vnp_Amount < amount * 100)
            //Save
            return "_1";
        //vnp_BankCode -> valid
        //vnp_BankTranNo -> Save
        //vnp_CardType -> valid
        //vnp_OrderInfo -> valid
        //vnp_PayDate -> Save
        if (parameters.vnp_ResponseCode[0] != '0')
            //Save
            return "_2";
        //vnp_TmnCode -> Valid
        //vnp_TransactionNo -> Save
        //vnp_TransactionStatus -> Valid
        if (!parameters.vnp_TxnRef.Equals(txnRef))
            return "_3";
        //SecureHash -> Valid
        return parameters.vnp_ResponseCode;
    }

    /*public string BuildIPNUrl(VNPayReturnParams parameters)
    {
        //https://sandbox.vnpayment.vn/tryitnow/Home/VnPayIPN?
        //&vnp_TransactionNo=12996460
        //&vnp_TxnRef=23597
        //&vnp_SecureHashType=SHA256
        //&vnp_SecureHash=20081f0ee1cc6b524e273b6d4050fefd
    }*/






    public class VNPayComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y)
                return 0;
            if (x is null)
                return -1;
            if (y is null)
                return 1;
            var Compare = CompareInfo.GetCompareInfo("en-US");
            return Compare.Compare(x, y, CompareOptions.Ordinal);
        }
    }

    private static string HashWithHMACSHA512(string key, string inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
                hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }






    /*private readonly SortedList<string, string> _responseData = new();

    public void AddResponseData(string key, string value) => _responseData.Add(key, value);
    public string GetResponseData(string key) => _responseData[key];

    public string GetResponseData()
    {
        StringBuilder data = new();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
            _responseData.Remove("vnp_SecureHashType");
        if (_responseData.ContainsKey("vnp_SecureHash"))
            _responseData.Remove("vnp_SecureHash");
        foreach (KeyValuePair<string, string> kv in _responseData)
            if (!string.IsNullOrEmpty(kv.Value))
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
        if (data.Length > 0)
            data.Remove(data.Length - 1, 1);
        return data.ToString();
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        string rspRaw = GetResponseData();
        string myChecksum = HashWithHMACSHA512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }*/
}