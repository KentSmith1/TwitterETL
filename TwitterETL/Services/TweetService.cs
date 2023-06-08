using TwitterETL.Models;
using TwitterETL.Repositories;

namespace TwitterETL.Services
{
    public interface ITweetService
    {
        List<TweetDTO> GetTweets();
        List<TweetDTO> AddTweets(List<TweetDTO> tweets);
    }
    public class TweetService: ITweetService
    {
        private ITweetRepository _tweetRepository;
        public TweetService(ITweetRepository tweetRepository) {
            _tweetRepository = tweetRepository;
        }

        public List<TweetDTO> AddTweets(List<TweetDTO> tweets)
        {
            return _tweetRepository.AddTweets(tweets);
        }

        public List<TweetDTO> GetTweets()
        {
            return _tweetRepository.GetAllTweets();
        }
    }
}
