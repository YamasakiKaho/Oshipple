using System.ComponentModel.DataAnnotations;

namespace Oshipple.Models
{
    public class AllSongs
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Kana { get; set; }
        public string Artist { get; set; }
        public string Artist_Kana { get; set; }
        public string Selector { get; set; }
        public string Mood1 { get; set; }
        public string Mood2 { get; set; }
        public int Rank { get; set; }
        public string? Comment { get; set; }
    }
}
