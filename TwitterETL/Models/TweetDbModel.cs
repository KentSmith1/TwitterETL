using System.ComponentModel.DataAnnotations;

namespace TwitterETL.Models
{
    public class TweetDbModel
    {
        [Key]
        public string tweetID { get; set; }
        public string? userID { get; set; }
        public string? location { get; set; }
        public DateTime? dateTime { get; set; }
        public string hashtags { get; set; }
        public Boolean? retweet { get; set; }
        public int? likeCount { get; set; }
        public int? retweetCount { get; set; }
        public int? quoteCount { get; set; }
        public int? replyCount { get; set; }
    }
}
