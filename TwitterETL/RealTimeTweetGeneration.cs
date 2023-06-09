using System;
using System.Timers;
using TwitterETL.Services;

namespace TwitterETL
{
    public interface IRealTimeTweetGeneration
    {
        public void startTweetGeneration();
    }
    public class RealTimeTweetGeneration: IRealTimeTweetGeneration
    {
        private System.Timers.Timer aTimer;
        private Random random = new Random();
        private ITweetGeneration _tweetGeneration;
        private ITweetService _tweetService;
        private ILogger<RealTimeTweetGeneration> _logger;
        public RealTimeTweetGeneration(ITweetGeneration tweetGeneration, ITweetService tweetService, ILogger<RealTimeTweetGeneration> logger)
        {
            _tweetGeneration = tweetGeneration;
            _tweetService = tweetService;
            SetTimer();
            _logger = logger;
        }

        public void startTweetGeneration()
        {
            return;
        }

        private void SetTimer()
        {
            // Create a timer with a 15 second interval.
            aTimer = new System.Timers.Timer(15000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var numberOfTweets = random.Next(1, 100);
            var tweetList = _tweetGeneration.generateRandomTweets(numberOfTweets);
            _tweetService.AddTweets(tweetList.Select(x => _tweetGeneration.MapToDTO(x)).ToList());
            _logger.LogInformation(numberOfTweets + " tweets generated at " + e.SignalTime);
        }
    }
}
