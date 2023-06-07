using LinqToTwitter;
using TwitterETL.Models;

namespace TwitterETL.Services
{
    public interface ITwitterService
    {
        LinqToTwitter.Tweet getLatestTweet();
        List<Tweet> getTweets(int count);

        //TweetDTO MapToDTO(LinqToTwitter.Tweet tweet);
    }

    public class TwitterService : ITwitterService
    {
        //TwitterClient client = new TwitterSharp.Client.TwitterClient(bearerToken);

        public TwitterService() { }

        public LinqToTwitter.Tweet getLatestTweet()
        {
            throw new NotImplementedException();
        }

        public List<Tweet> getTweets(int count)
        {
            throw new NotImplementedException();
        }
    }
}
