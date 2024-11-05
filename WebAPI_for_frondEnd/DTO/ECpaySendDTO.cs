namespace WebAPI_for_frondEnd.DTO {
    public class ECpaySendDTO {
        String MerchantID { get; set; }
        String MerchantTradeNo { get; set; }
        String MerchantTradeDate {  get; set; }
        String PaymentType { get; set; }
        int? TotalAmount { get; set; }
        String TradeDesc { get; set; }
        String ItemName { get; set; }
        String ReturnURL { get; set; }
        String ChoosePayment { get; set; }
        String CheckMacValue { get; set; }
        String EncryptType { get; set; }
        String StoreID { get; set; }
        String ClientBackURL { get; set; }
        String ItemURL { get; set; }
        String Remark { get; set; }
        String ChooseSubPayment { get; set; }
        String OrderResultURL { get; set; }
        String NeedExtraPaidInfo { get; set; }
        String IgnorePayment { get; set; }
        String PlatformID { get; set; }
        String CustomField1 { get; set; }
        String CustomField2 { get; set; }
        String CustomField3 { get; set; }
        String CustomField4 { get; set; }
        String Language { get; set; }
    }
}
