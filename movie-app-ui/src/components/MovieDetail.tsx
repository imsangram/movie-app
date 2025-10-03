import React, { useState, useEffect, useRef } from "react";
import {
  Container,
  Typography,
  Box,
  Alert,
  Skeleton,
  Button,
  Divider,
  CardContent,
  Card,
  CardMedia,
  Chip,
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { MovieDetail } from "../types/Movie";
import { getMovieById } from "../services/ApiService";
import { ArrowBack } from "@mui/icons-material";

function MovieDetailPage() {
  const [movie, setMovie] = useState<MovieDetail | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [isNotFound, setIsNotFound] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const hasFetched = useRef(false);
  const { movieId } = useParams<{ movieId: string }>();

  useEffect(() => {
    if (!hasFetched.current) {
      hasFetched.current = true;
      if (movieId) {
        fetchMovie(movieId);
      } else {
        setError("Movie ID is missing.");
        setLoading(false);
      }
    }
  }, [movieId]);

  const handleBackClick = () => {
    navigate("/");
  };

  const fetchMovie = async (id: string) => {
    try {
      setLoading(true);
      setError(null);
      setIsNotFound(false);
      const movieData = await getMovieById(id);
      setMovie(movieData);
    } catch (err) {
      console.error("Error fetching movie detail:", err);
      // Check if it's a 404 error
      if (
        err instanceof Error &&
        (err.message.includes("404") || err.message.includes("status: 404"))
      ) {
        setIsNotFound(true);
        setError(
          "Movie not found. The movie you are looking for does not exist."
        );
      } else {
        setError("Failed to fetch movie details. Please try again later.");
      }
    } finally {
      setLoading(false);
    }
  };

  const renderSkeleton = () => (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Box display="flex" alignItems="center" mb={3}>
        <Skeleton variant="circular" width={40} height={40} />
        <Skeleton variant="text" width={100} height={30} sx={{ ml: 2 }} />
      </Box>

      <Box display="flex" flexDirection={{ xs: "column", md: "row" }} gap={4}>
        <Box flex="0 0 300px">
          <Skeleton variant="rectangular" width={300} height={450} />
        </Box>

        <Box flex={1}>
          <Skeleton variant="text" width="80%" height={40} />
          <Skeleton variant="text" width="40%" height={30} sx={{ mt: 1 }} />
          <Skeleton variant="text" width="100%" height={20} sx={{ mt: 2 }} />
          <Skeleton variant="text" width="100%" height={20} />
          <Skeleton variant="text" width="90%" height={20} />
          <Skeleton variant="text" width="60%" height={20} sx={{ mt: 2 }} />
          <Skeleton variant="text" width="50%" height={20} sx={{ mt: 1 }} />
        </Box>
      </Box>
    </Container>
  );

  const notFound = () => (
    <Box textAlign="center" sx={{ mt: 8, mb: 4 }}>
      <Typography
        variant="h1"
        component="h1"
        sx={{ fontSize: "6rem", fontWeight: "bold", color: "text.secondary" }}
      >
        404
      </Typography>
      <Typography variant="h4" component="h2" gutterBottom sx={{ mt: 2 }}>
        Movie Not Found
      </Typography>
      <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
        The movie you are looking for does not exist or may have been removed.
      </Typography>
      <Button variant="contained" onClick={handleBackClick} sx={{ mt: 2 }}>
        Browse All Movies
      </Button>
    </Box>
  );

  if (loading) {
    return renderSkeleton();
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Button
          startIcon={<ArrowBack />}
          onClick={handleBackClick}
          sx={{ mb: 3 }}
        >
          Back to Movies
        </Button>

        {isNotFound ? (
          notFound()
        ) : (
          <Alert
            severity="error"
            sx={{ mb: 2 }}
            action={
              <Button
                color="inherit"
                size="small"
                onClick={() => movieId && fetchMovie(movieId)}
              >
                Retry
              </Button>
            }
          >
            {error}
          </Alert>
        )}
      </Container>
    );
  }
  if (!movie) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Button
          startIcon={<ArrowBack />}
          onClick={handleBackClick}
          sx={{ mb: 3 }}
        >
          Back to Movies
        </Button>

        <Typography variant="h6" color="text.secondary" align="center">
          Movie not found
        </Typography>
      </Container>
    );
  }
  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Button
        startIcon={<ArrowBack />}
        onClick={handleBackClick}
        sx={{ mb: 3 }}
      >
        Back to Movies
      </Button>

      <Card>
        <Box display="flex" flexDirection={{ xs: "column", md: "row" }}>
          <CardMedia
            component="img"
            sx={{
              width: { xs: "100%", md: 300 },
              height: { xs: 400, md: 450 },
              objectFit: "cover",
            }}
            image={movie.poster}
            alt={movie.title}
          />

          <CardContent sx={{ flex: 1, p: 3 }}>
            <Typography variant="h4" component="h1" gutterBottom>
              {movie.title}
            </Typography>

            <Box display="flex" gap={1} mb={2} flexWrap="wrap">
              <Chip label={movie.year} color="primary" size="small" />
              <Chip label={movie.type} color="secondary" size="small" />
              {movie.genre && (
                <Chip label={movie.genre} variant="outlined" size="small" />
              )}
            </Box>

            {movie.plot && (
              <>
                <Typography variant="h6" gutterBottom sx={{ mt: 3 }}>
                  Plot
                </Typography>
                <Typography variant="body1" paragraph>
                  {movie.plot}
                </Typography>
              </>
            )}

            <Divider sx={{ my: 2 }} />

            <Box display="flex" flexDirection="column" gap={1}>
              {movie.director && (
                <Typography variant="body2">
                  <strong>Director:</strong> {movie.director}
                </Typography>
              )}
              {movie.actors && (
                <Typography variant="body2">
                  <strong>Cast:</strong> {movie.actors}
                </Typography>
              )}
              {movie.runtime && (
                <Typography variant="body2">
                  <strong>Runtime:</strong> {movie.runtime}
                </Typography>
              )}
              {movie.released && (
                <Typography variant="body2">
                  <strong>Released:</strong> {movie.released}
                </Typography>
              )}
              {movie.imdbRating && (
                <Typography variant="body2">
                  <strong>IMDB Rating:</strong> {movie.imdbRating}
                </Typography>
              )}
              {movie.language && (
                <Typography variant="body2">
                  <strong>Language:</strong> {movie.language}
                </Typography>
              )}
              {movie.country && (
                <Typography variant="body2">
                  <strong>Country:</strong> {movie.country}
                </Typography>
              )}
              {movie.awards && (
                <Typography variant="body2">
                  <strong>Awards:</strong> {movie.awards}
                </Typography>
              )}
            </Box>
          </CardContent>
        </Box>
      </Card>
    </Container>
  );
}

export default MovieDetailPage;
