using LinqToTwitter;
using System.Text.Json;
using TwitterETL.Context;
using TwitterETL.Models;

namespace TwitterETL.Repositories
{
    public interface ITweetRepository
    {
        TweetDTO? GetTweet(string id);
        List<TweetDTO> AddTweets(List<TweetDTO> tweets);
        List<TweetDTO> GetAllTweets();
        TweetDTO UpdateTweet(TweetDTO tweet);
    }
    public class TweetRepository: ITweetRepository
    {
        TweetContext context = new TweetContext();

        public List<TweetDTO> AddTweets(List<TweetDTO> tweets)
        {
            context.Tweets.AddRange(tweets.Select(x => mapToModel(x)).ToList());
            context.SaveChanges();
            return tweets;
        }

        public List<TweetDTO> GetAllTweets()
        {
            var tweets = context.Tweets.ToList();
            return tweets.Select(x => mapToDTO(x)).ToList();
        }

        public TweetDTO? GetTweet(string id)
        {
            return mapToDTO(context.Tweets.FirstOrDefault(x => x.tweetID == id));
        }       

        public TweetDTO UpdateTweet(TweetDTO tweet)
        {
            throw new NotImplementedException();
        }

        private TweetDTO mapToDTO(TweetDbModel model)
        {
            return new TweetDTO
            {
                tweetID = model.tweetID,
                userID = model.userID,
                location = model.location,
                dateTime = model.dateTime,
                hashtags = JsonSerializer.Deserialize<List<string>>(model.hashtags),
                retweet = model.retweet,
                likeCount = model.likeCount,
                retweetCount = model.retweetCount,
                quoteCount = model.quoteCount,
                replyCount = model.replyCount
            };
        }

        private TweetDbModel mapToModel(TweetDTO tweet)
        {
            return new TweetDbModel
            {
                tweetID = tweet.tweetID,
                userID = tweet.userID,
                location = tweet.location,
                dateTime = tweet.dateTime,
                hashtags = JsonSerializer.Serialize(tweet.hashtags),
                retweet = tweet.retweet,
                likeCount = tweet.likeCount,
                retweetCount = tweet.retweetCount,
                quoteCount = tweet.quoteCount,
                replyCount = tweet.replyCount
            };
        }
    }
}
