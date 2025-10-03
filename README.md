# Movie App

This project aggregates movie information from multiple external providers (currently **CineWorld** and **FilmWorld**) and returns a unified response to clients.  
It is designed with **extensibility, resilience, and maintainability** in mind, allowing easy integration of future providers without impacting existing code.

---

## 🧠 Thought Process & Key Decisions

### 1. Resilience and Fault Tolerance with Polly
- Integrated **Polly** policies (retry, circuit breaker, etc.) with `HttpClient`.  
- Ensures resiliency when calling unreliable external APIs.
- Calls to providers are executed in **parallel** with `Task.WhenAll`.  
- If a provider fails:
  - The system **skips the failed result**.  
  - Still returns successful results from other providers.  
- This ensures **graceful degradation** instead of full failure.

### 2. Extensibility
- Built using **Clean Architecture** to keep flexible, maintainable by separating concerns.  
- Since the number of providers will likely grow, the architecture should allow adding new endpoints with minimal changes to existing code.

### 3. Error Handling
- Used a **Result pattern** to safely pass data and errors between layers as strong typing prevents unexpected exceptions and its easier to test for various business flows.  
- Implemented a **global exception handling middleware** to catch unexpected errors,prevent exposure of sensitive details and to return user-friendly error messages.  

### 4. Data Flow & Models
The project uses **three model layers** for clarity, which helps us with data abstraction.
1. **Provider DTOs** → map raw data from external APIs.  
2. **Domain Models** → internal representation with light business logic. 
3. **Client DTOs** → data returned from our API to the frontend.
4. **Automapper**  → offers centralized, reusable configuration for consistency to map objects

### 5. Frontend (React)
- **List Page (Home)**:
  - Displays all distinct aggregated movies.  
- **Detail Page**:  
  - Displays movie details + cheapest price.
- **Caching with some trade-offs**
  - Listing data has been cached locally to avoid making expensive calls to the API as the data is less likely to change where as movie detail API call was not cached to make sure we always show latest and cheapest pricing.

## ⚡ Possible Improvements

1. If the movie listing grows exponentially, it would be ideal to have it with pagination and filtering support.
2. If the numper of providers increase, we may need to find more appropriate solution where client can be updated through events or stream about the cheapest price.
3. Integration testing to make sure that API doesn't fail when contract changes.
4. Server side caching using memcached or redis.
5. Improve security using JWT
6. Rate limiting at infrastucture level or system level using polly.

---


## 🚀 Running the React and .Net core project
Frontend: http://localhost:3000

Backend: http://localhost:8001

```sh
cd movie-app-ui
npm install
npm start


cd movie-app-service
dotnet restore
dotnet run --project src/MovieApp.Api/MovieApp.Api.csproj
