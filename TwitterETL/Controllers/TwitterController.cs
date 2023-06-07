using LinqToTwitter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
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
        private static Random random = new Random();
        public List<TweetEntityHashtag> AvailableHashtags = new List<TweetEntityHashtag>();
        

        public TwitterController(ILogger<TwitterController> logger)
        {
            _logger = logger;
            Mock<ITwitterService> mockObject = new Mock<ITwitterService>();
            mockObject.Setup(m => m.getLatestTweet()).Returns(
                new LinqToTwitter.Tweet { 
                    ID = "123456789", 
                    Text = "Loving it #ChargeNow", 
                    AuthorID = "JamaicaJames123", 
                    Geo = new TweetGeo { PlaceID = "12"},  
                    CreatedAt = new DateTime(),
                    Entities = new LinqToTwitter.TweetEntities { 
                        Hashtags = new List<TweetEntityHashtag> { 
                            new TweetEntityHashtag { Tag = "ChargeNow" } 
                        }
                    },
                    ReferencedTweets = new List<TweetReference> { },
                    PublicMetrics = new TweetPublicMetrics { LikeCount = 10, QuoteCount = 11, ReplyCount = 12, RetweetCount = 13}
               });

            mockObject.Setup(m => m.getTweets(It.IsAny<int>())).Returns(
                    new List<Tweet>()
                );
            AvailableHashtags.Add(new TweetEntityHashtag { Tag = "ChargeNow" });
            AvailableHashtags.AddRange(GenerateHashtags(19));

            _twitterService = mockObject.Object;            
        }

        [HttpGet(Name = "GetLatestTweet")]
        public TweetDTO GetTweet()
        {
            return MapToDTO(_twitterService.getLatestTweet());
        }

        [HttpPut(Name = "GetTweet")]
        public List<TweetDTO> GetTweets(int count)
        {
            var tweetList = generateRandomTweets(count);
            return tweetList.Select(x => MapToDTO(x)).ToList();
        }

        private TweetDTO MapToDTO(LinqToTwitter.Tweet tweet)
        {
            var x = new TweetDTO
            {
                userID = tweet.AuthorID,
                location = tweet.Geo.PlaceID,
                dateTime = tweet.CreatedAt,
                hashtags = tweet.Entities.Hashtags.Select(x => x.Tag).ToList(),
                retweet = tweet.ReferencedTweets.Count > 0,
                likeCount = tweet.PublicMetrics.LikeCount,
                retweetCount = tweet.PublicMetrics.RetweetCount,
                quoteCount = tweet.PublicMetrics.QuoteCount,
                replyCount = tweet.PublicMetrics.ReplyCount
            };
            return x;
        }

        private Tweet generateRandomTweet()
        {
            return new LinqToTwitter.Tweet
            {
                ID = new Guid().ToString(),
                Text = "Loving it #ChargeNow",
                AuthorID = RandomString(random.Next(4,12)),
                Geo = new TweetGeo { PlaceID = random.Next(1, 200).ToString() },
                CreatedAt = new DateTime(),
                Entities = new LinqToTwitter.TweetEntities
                {
                    Hashtags = getRandomHastags(random.Next(1, AvailableHashtags.Count()))
                },
                ReferencedTweets = generateReferenctTweets(random.Next(0,2)),
                PublicMetrics = new TweetPublicMetrics { LikeCount = random.Next(1, 200), QuoteCount = random.Next(1, 200), ReplyCount = random.Next(1, 200), RetweetCount = random.Next(1, 200) }
            };
        }


        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string RandomHashtag(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private List<TweetEntityHashtag> GenerateHashtags(int numberOfHashtags)
        {
            List<TweetEntityHashtag> list = new List<TweetEntityHashtag>();
            for(var i = 0; i < numberOfHashtags; i++)
            {
                list.Add(new TweetEntityHashtag { Tag = RandomHashtag(random.Next(4, 12)) });
            }
            return list;
        }

        private List<TweetEntityHashtag> getRandomHastags(int length)
        {
            List<TweetEntityHashtag> hashtagMaster = AvailableHashtags.Select(x => new TweetEntityHashtag { Tag = x.Tag}).ToList();
            List<TweetEntityHashtag> list = new List<TweetEntityHashtag>();
            for(var i = 0; i < length; i++)
            {
                int index = random.Next(length - i);
                list.Add(hashtagMaster[index]);
                hashtagMaster.RemoveAt(index);
            }
            return list;
        }

        private List<Tweet> generateRandomTweets(int count)
        {
            var list = new List<Tweet>();
            for(var i = 0; i < count; i++)
            {
                list.Add(generateRandomTweet());
            }
            return list;
        }

        private List<TweetReference> generateReferenctTweets(int length)
        {
            var list = new List<TweetReference>();
            for(int i = 0; i< length; i++)
            {
                list.Add(new TweetReference
                {
                    ID = RandomString(random.Next(4, 12))
                });
            }
            return list;
        }
    }
}
