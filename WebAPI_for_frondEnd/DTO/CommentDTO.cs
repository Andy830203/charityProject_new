using System.ComponentModel.DataAnnotations;

namespace WebAPI_for_frondEnd.DTO
{
    public class CommentDTO
    {
        public int m_id { get; set; }

        public int e_id { get; set; }

        [Required(ErrorMessage = "評論內容不可為空。")]
        public string content { get; set; }

        [Range(1, 5, ErrorMessage = "評分必須在1到5之間")]
        public int score { get; set; }
    }
}
