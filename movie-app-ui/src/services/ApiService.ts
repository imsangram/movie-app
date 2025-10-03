import { Movie, MovieDetail } from '../types/Movie';
import { BASE_URL, TIMEOUT_MS } from '../config';
import { CacheNewService } from './CacheService';

const fetchWithTimeout = async <T>(url: string): Promise<T> => {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), TIMEOUT_MS);

  try {
    const response = await fetch(url, {
      signal: controller.signal,
      headers: { 'Content-Type': 'application/json' }
    });

    clearTimeout(timeoutId);

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    return response.json();
  } catch (error) {
    clearTimeout(timeoutId);
    if (error instanceof Error && error.name === 'AbortError') {
      throw new Error('Request timeout');
    }
    throw error;
  }
};

export const getMovies = async (): Promise<Movie[]> => {
  const cacheKey = 'movies_list';
  
  // Try to get from cache first
  const cachedMovies = CacheNewService.get<Movie[]>(cacheKey);
  if (cachedMovies) {
    return cachedMovies;
  }
  
  // Cache miss - fetch from API
  const movies = await fetchWithTimeout<Movie[]>(`${BASE_URL}movies`);
  
  // Cache the result
  CacheNewService.set(cacheKey, movies);
  
  return movies;
};

export const getMovieById = async (movieId: string): Promise<MovieDetail> => {
  return fetchWithTimeout<MovieDetail>(`${BASE_URL}movies/${movieId}`);
};

const ApiService = {
  getMovies,
  getMovieById
};

export default ApiService;
