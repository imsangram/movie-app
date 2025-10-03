interface CacheItem<T> {
  data: T;
  timestamp: number;
}

interface CacheInstance {
  get: <T>(key: string, ttlMs?: number) => T | null;
  set: <T>(key: string, data: T) => void;
}

// Constants
const DEFAULT_CACHE_DURATION_MS = 5 * 60 * 1000; // 5 minutes

// Pure function to check if cache is still valid
const isCacheValid = (timestamp: number, ttlMs: number): boolean => {
  return Date.now() - timestamp < ttlMs;
};

// Cache factory function that creates a cache instance with closure
export const createCacheService = (): CacheInstance => {
  const cache = new Map<string, CacheItem<any>>();

  const get = <T>(key: string, ttlMs: number = DEFAULT_CACHE_DURATION_MS): T | null => {
    const cached = cache.get(key);
    
    if (cached && isCacheValid(cached.timestamp, ttlMs)) {
      return cached.data;
    }
    
    // Remove expired cache entry
    if (cached) {
      cache.delete(key);
    }
    
    return null;
  };

  const set = <T>(key: string, data: T): void => {
    cache.set(key, {
      data,
      timestamp: Date.now()
    });
  };

  return {
    get,
    set
  };
};

// Create a default cache instance
export const CacheNewService = createCacheService();

export default CacheNewService;
