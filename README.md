# TwitterETL
ETL Pipeline for twitter 

# Startup
The application can be started up by using Docker.
The ports 80 and 443 need to be mapped.

## Docker Build
docker build -f "./TwitterETL/Dockerfile" -t twitteretl:latest .

## Docker Run
docker run -p 5050:80 -p 5051:443 -v $env:USERPROFILE\.aspnet\https:/https/ --name TwitterETL -i twitteretl:latest -it -rm

Note: The volume that is mounted must point to where the dotnet generated SSL certificate is located. To generate a dotnet self-signed SSL certificate, run the following command:

dotnet dev-certs https -ep $env:USERPROFILE/.aspnet/https/aspnetapp.pfx -p {PASSWORD}

dotnet dev-certs https --trust

Where {PASSWORD} is the password in the dockerfile under the environment variable: ASPNETCORE_Kestrel__Certificates__Default__Password

# How to use the ETL
Due to twitter support not getting back to me with a clientID, I was never able to access the TwitterAPI. I instead created Mock responses, using the dotnet Moq library to simulate reading tweets from the API.

To start the application, go to https://localhost:5051/swagger/index.html on your webbrowser (Or which ever port you mapped 443 to).

From there, run the api call /api/Twitter/StartTweetGeneration. This will start the production of a random number of tweets (between 1 and 100) every 15 seconds. You can follow this generation in the logs.

After the first tweets are generated, you can query all of the tweets stored in the in-memory database, by running /api/Twitter/GetAllTweets or /api/Twitter/StartTweetGenerationWhereChargeNow to get all the tweets with the Hashtag #ChargeNow.

You can also get the metrics of the tweets by running /api/Twitter/GetAllTweetInteractionsWhereChargeNow which will return the total amount of tweets processed, the tweets featuring ChargeNow and the amount of likes, retweets, quotes and replies on those tweets.

# Design Choices
I decided to make a deviation from the document. I did not implement the number of followers, as there a user cannot follow a hashtag on twitter, like on instagram. Also counting the number of followers per user that made the tweet seems like the wrong way to go, as it doesn't really capture the interactions. Instead I replaced the number of followers with the likes, retweets, quotes and replies as that captures the impact of the tweet better.

