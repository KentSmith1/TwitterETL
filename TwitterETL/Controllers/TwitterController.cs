using LinqToTwitter;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TwitterETL.Models;
using TwitterETL.Services;

namespace TwitterETL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger<TwitterController> _logger;
        private ITwitterService _twitterService;
        private ITweetGeneration _tweetGeneration;
        private ITweetService _tweetService;
        private IRealTimeTweetGeneration _realTimeTweetGeneration;


        public TwitterController(ILogger<TwitterController> logger, ITweetGeneration tweetGeneration, ITweetService tweetService, IRealTimeTweetGeneration realTimeTweetGeneration)
        {
            _logger = logger;
            Mock<ITwitterService> mockObject = new Mock<ITwitterService>();
            mockObject.Setup(m => m.getLatestTweet()).Returns(
                new LinqToTwitter.Tweet {
                    ID = "123456789",
                    Text = "Loving it #ChargeNow",
                    AuthorID = "JamaicaJames123",
                    Geo = new TweetGeo { PlaceID = "12" },
                    CreatedAt = new DateTime(),
                    Entities = new LinqToTwitter.TweetEntities {
                        Hashtags = new List<TweetEntityHashtag> {
                            new TweetEntityHashtag { Tag = "ChargeNow" }
                        }
                    },
                    ReferencedTweets = new List<TweetReference> { },
                    PublicMetrics = new TweetPublicMetrics { LikeCount = 10, QuoteCount = 11, ReplyCount = 12, RetweetCount = 13 }
                });

            mockObject.Setup(m => m.getTweets(It.IsAny<int>())).Returns(
                    new List<Tweet>()
                );
            _twitterService = mockObject.Object;
            _tweetGeneration = tweetGeneration;
            _tweetService = tweetService;
            _realTimeTweetGeneration = realTimeTweetGeneration;
        }

        [HttpGet("GetTweet")]
        public ActionResult<TweetDTO> GetTweet()
        {
            return Ok(_tweetGeneration.MapToDTO(_twitterService.getLatestTweet()));
        }

        [HttpGet("GenerateTweets/{count}")]
        public ActionResult<List<TweetDTO>> GenerateTweets(int count)
        {
            var tweetList = _tweetGeneration.generateRandomTweets(count);
            return Ok(_tweetService.AddTweets(tweetList.Select(x => _tweetGeneration.MapToDTO(x)).ToList()));
            //return tweetList.Select(x => _tweetGeneration.MapToDTO(x)).ToList();
        }

        [HttpGet("GetAllTweets")]
        public List<TweetDTO> GetAllTweets()
        {
            return _tweetService.GetTweets();
            //return new List<TweetDTO>();
        }

        [HttpGet("GetAllTweetsWhereChargeNow")]
        public List<TweetDTO> GetAllTweetsWhereChargeNow()
        {
            return _tweetService.GetTweets().Where(x => x.hashtags.Any(y => y == "ChargeNow")).ToList();
        }

        [HttpGet("GetAllTweetsWhereChargeNowCount")]
        public int GetAllTweetsWhereChargeNowCount()
        {
            return _tweetService.GetTweets().Where(x => x.hashtags.Any(y => y == "ChargeNow")).Count();
        }

        [HttpGet("GetAllTweetInteractionsWhereChargeNow")]
        public TweetInteraction GetTweetInteraction() {
            var tweets = _tweetService.GetTweets().Where(x => x.hashtags.Any(y => y == "ChargeNow"));
            return new TweetInteraction
            {
                totalTweetCount = _tweetService.GetTweets().Count(),
                totalChargeNowTweetCount = tweets.Count(),
                replyCount = tweets.Sum(_ => _.replyCount),
                retweetCount = tweets.Sum(_ => _.retweetCount),
                likeCount = tweets.Sum(_ => _.likeCount),
                quoteCount = tweets.Sum(_ => _.quoteCount),
            };
        }

        [HttpGet("StartTweetGeneration")]
        public ActionResult StartTweetGeneration()
        {
            _realTimeTweetGeneration.startTweetGeneration();
            return Ok();
        }
    }
}
