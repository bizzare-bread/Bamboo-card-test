# Bamboo Card - Developer Coding Test

## Overview

This project implements a RESTful API using ASP.NET Core to retrieve the details of the best *n* stories from the Hacker News API, based on their score. The number *n* is specified by the caller to the API.

## Implementation Details

- **ASP.NET Core Minimal API**: The project utilizes dotnet minimal API to streamline development and keep the codebase concise.
- **HackerNews Library**: A separate library, `HackerNews`, is created to encapsulate the logic for interacting with the Hacker News API and fetching story details.
- **BambooCard Web App**: The main application, `BambooCard`, is built to consume the `HackerNews` library and handle HTTP requests from clients.
- **Efficient Request Handling**: Although there is currently no rate limit on the Hacker News API, the application is designed to efficiently handle large numbers of requests without risking overloading the Hacker News API.

## Assumptions

- The primary goal is to retrieve the best stories from Hacker News based on their score.
- Docker support can be added to facilitate deployment and containerization of the application.
- Rate limiting is not implemented due to the absence of rate limits on the Hacker News API.
- For further scalability and efficiency, a message queue system (e.g., AWS SQS, Azure Service Bus) could be integrated. This would involve restructuring the project to handle requests asynchronously by sending them to a message queue and processing them separately.

## Running the Application

To run the application:

1. Clone the repository to your local machine.
2. Navigate to the root directory of the project.
3. Run the following command to build and run the application:

   ```bash
     dotnet run --project BambooCard
   ```

4. Access the API endpoint to retrieve the best stories. You can use this endpoint by Swagger `https://localhost:5001/swagger` or directly by enpoint url `https://localhost:5001/beststories?quantity={n}`, where port for localhost will be assigned by your system (check your terminal logs from the previous step).

## Future Enhancements

- **Docker Support**: Integrate Docker support to enable easy deployment and containerization of the application.
- **Message Queue Integration**: Implement a message queue system to handle requests asynchronously and improve scalability.
- **Logging and Monitoring**: Enhance logging and monitoring capabilities to track the performance and health of the application.
- **Caching**: Implement caching mechanisms to cache frequently requested data and reduce the load on external APIs.

### Rate Limit Handling

If the Hacker News API enables rate limiting in the future, additional logic can be added to handle it gracefully. This may include:
- Checking the `X-RateLimit-Limit` or `X-RateLimit-Remaining` HTTP headers to determine the current rate limit status.
- Implementing try-catch blocks to handle HTTP status code 429 (Too Many Requests) and processing the exception appropriately, such as retrying the request after a certain interval or informing the client about the rate limit exceeded status.

### Load Balancing

Here you can see a result of a load balancing test. 

Parameters:
- 10 Virtual Users per second
- 10 second test duration

Results:
- 100% success (0.00% of requests failed)
- 15.7 seconds total time spent
- other analysis data you can see at the screenshot below

<img width="819" alt="image" src="https://github.com/bizzare-bread/Bamboo-card-test/assets/26627866/b688ea40-91a4-4372-b5a7-802b3c57f9e1">

