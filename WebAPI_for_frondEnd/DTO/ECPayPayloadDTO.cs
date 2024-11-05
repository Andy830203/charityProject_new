namespace WebAPI_for_frondEnd.DTO {
    public class ECPayPayloadDTO {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; }
        public string TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; }
        public string ChoosePayment { get; set; }
        public int EncryptType { get; set; }
        public string CheckMacValue { get; set; }
    }
}
