# BestStoriesAPI

A RESTful API to retrieve the details of the first 20 "best stories" from the Hacker News API.

## Run the API

First <a href=https://github.com/hgpimentel/BestStoriesAPI/archive/master.zip>download</a> or <a href=https://github.com/hgpimentel/BestStoriesAPI.git>clone</a> the project.

Open the console line in the solution folder and run:

dotnet build --configuration Release

dotnet test

dotnet run --configuration Release --project ./API/API.csproj

## Access the API

### Browser

- http://localhost:5000/api/stories
- https://localhost:5001/api/stories

### Console

- curl -X GET http://localhost:5000/api/stories
- curl -X GET https://localhost:5001/api/stories

## Assumptions

1. The API always returns 200 OK if there is no internal exceptions, otherwise it will return 500 Internal Server Error. 
2. Best case scenario (Hacker News API is responding correctly)
	- Returns an array of the 20 best stories either by getting them from the Hacker News API or its cache (set to 1 min) 
3. Failure getting the top stories ids:
	- Returns an empty array of stories and does not cache
4. Failure getting an individual story:
	- Returns an array of successful returned stories and caches if any

## Improvements

1. Circuit breaker policies should be implement when calling Hacker News API in order to not overload it if there is a failure.
2. Caching should be implement in a fine-grained way if we want to mix getting information from it and the API
3. The API response can have different status codes when there is no information to show (e.g. 404 Not Found or 204 No Content) 