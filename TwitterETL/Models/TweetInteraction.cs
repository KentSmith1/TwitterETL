namespace TwitterETL.Models
{
    public class TweetInteraction
    {
        public int totalTweetCount { get; set; }
        public int totalChargeNowTweetCount { get; set; }
        public int? likeCount { get; set; }
        public int? retweetCount { get; set; }
        public int? quoteCount { get; set; }
        public int? replyCount { get; set; }
    }
}
