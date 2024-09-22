namespace charity.ViewModels {
    public class UploadedImage {
        public string ImgName { get; set; } // 圖片路徑
        public int ImgId { get; set; } // 圖片 ID (用於刪除或編輯)
    }
}
