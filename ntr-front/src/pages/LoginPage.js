import React, { useEffect } from 'react'
import { TextField, Alert, Typography, Button, Box, Container } from "@mui/material";
import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import user from '../user.js';
import { useNavigate } from 'react-router-dom';

const LoggingPage = () => {
    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.LOGIN, "POST");
    const navigate = useNavigate();

    useEffect(() => {
        if (status === 200 && !isLoading) {
            console.log(data);
            user.isDirector = false;
            user.isDoctor = false;
            user.isAcceptedPatient = false;
            if (data === "Doctor")
                user.isDoctor = true;
            else if (data === "Director")
                user.isDirector = true;
            else if (data === "Patient")
                user.isAcceptedPatient = true;
            user.text = data;
            user.isLoggedIn = true;
            navigate('/Start');
        }
    }, [status, isLoading]);

    const handleSubmit = (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        execute(Object.fromEntries(data.entries()));
    }


    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}>

                <Typography
                    variant="h4"
                    color="black"
                    fontSize="40"
                    fontWeight="bold"
                >Logowanie</Typography>

                <Box component="form" onSubmit={handleSubmit}
                    sx={{
                        marginTop: 2,
                    }}
                >
                    {status === 404 && <Alert severity="error">User not found.</Alert>}
                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Email"
                        id="email"
                        name="email"
                        autoComplete='email'
                    >
                    </TextField>

                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Password"
                        id="password"
                        type="password"
                        name="password"
                        autoComplete='current-password'
                    >
                    </TextField>

                    <Button
                        variant="contained"
                        type="submit"
                        fullWidth
                        sx={{
                            marginTop: 2
                        }}
                        disabled={isLoading}>
                        {
                            isLoading ? "Loading..." : "Login"
                        }
                    </Button>

                </Box>
            </Box>
        </Container>
    )
}

export default LoggingPage
