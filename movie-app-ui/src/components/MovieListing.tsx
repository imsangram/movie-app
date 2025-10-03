import React, { useState, useEffect, useRef } from "react";
import {
  Container,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Avatar,
  Typography,
  Box,
  Alert,
  CircularProgress,
  Skeleton,
  Button,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Movie } from "../types/Movie";
import { getMovies } from "../services/ApiService";

function HomePage() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const hasFetched = useRef(false);

  useEffect(() => {
    if (!hasFetched.current) {
      hasFetched.current = true;
      fetchMovies();
    }
  }, []);

  const fetchMovies = async () => {
    setLoading(true);
    setError(null);

    try {
      const data = await getMovies();
      setMovies(data);
    } catch (error) {
      setMovies([]);
      setError("Invalid data format received from server.");
    } finally {
      setLoading(false);
    }
  };

  const handleMovieClick = (id: string) => {
    navigate(`/movie/${id}`);
  };

    const renderSkeletonList = () => {
    return Array.from({ length: 8 }).map((_, index) => (
      <ListItem key={index} sx={{ py: 2 }}>
        <ListItemAvatar>
          <Skeleton variant="rectangular" width={80} height={120} />
        </ListItemAvatar>
        <ListItemText
          primary={<Skeleton variant="text" width="60%" height={24} />}
          secondary={<Skeleton variant="text" width="40%" height={20} />}
          sx={{ ml: 2 }}
        />
      </ListItem>
    ));
  };

    if (loading) {
    return (
      <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom align="center" sx={{ mb: 4 }}>
          Movie Collection
        </Typography>
        <Box display="flex" justifyContent="center" mb={4}>
          <CircularProgress size={40} />
        </Box>
        <List>
          {renderSkeletonList()}
        </List>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom align="center" sx={{ mb: 4 }}>
          Movie Collection
        </Typography>
        <Alert 
          severity="error" 
          sx={{ mb: 2 }}
          action={
            <Button color="inherit" size="small" onClick={() => fetchMovies()}>
              Retry
            </Button>
          }
        >
          {error}
        </Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Box display="flex" justifyContent="space-between" alignItems="center" sx={{ mb: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 0 }}>
          Movie Collection
        </Typography>
        {/* <Button 
          variant="outlined" 
          size="small" 
          onClick={handleRefresh}
          disabled={loading}
        >
          Refresh
        </Button> */}
      </Box>
      
      {!Array.isArray(movies) || movies.length === 0 ? (
        <Box textAlign="center" sx={{ mt: 4 }}>
          <Typography variant="h6" color="text.secondary">
            No movies found
          </Typography>
        </Box>
      ) : (
        <List>
          {movies.map((movie) => (
            <ListItem 
              key={movie.id}
              onClick={() => handleMovieClick(movie.id)}
              sx={{ 
                cursor: 'pointer',
                borderRadius: 2,
                mb: 1,
                '&:hover': { 
                  backgroundColor: 'action.hover',
                  transform: 'scale(1.01)',
                  transition: 'all 0.2s ease-in-out'
                },
                border: '1px solid',
                borderColor: 'divider'
              }}
            >
              <ListItemAvatar>
                <Avatar 
                  src={movie.poster} 
                  alt={movie.title}
                  variant="rounded"
                  sx={{ width: 80, height: 120 }}
                />
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Typography variant="h6" component="h2">
                    {movie.title}
                  </Typography>
                }
                secondary={
                  <Typography variant="body2" color="text.secondary">
                    {movie.year} • {movie.type}
                  </Typography>
                }
                sx={{ ml: 2 }}
              />
            </ListItem>
          ))}
        </List>
      )}
    </Container>
  );
}

export default HomePage;
