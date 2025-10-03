export interface Movie {
  title: string;
  year: string;
  id: string;
  type: string;
  poster: string;
}

export interface MovieDetail extends Movie {
    rated?: string;
    director?: string;
    writer?: string;
    actors?: string;
    genre?: string;
    runtime?: string;
    imdbRating?: string;
    released?: string;
    plot?: string;
    metascore?: string;
    rating?: string;
    votes?: string;
    price?: number;
    language?: string;
    country?: string;
    awards?: string;
    boxOffice?: string;
    production?: string;
    website?: string;
}

export interface ApiResponse<T> {
  data?: T;
  error?: string;
  loading: boolean;
}
