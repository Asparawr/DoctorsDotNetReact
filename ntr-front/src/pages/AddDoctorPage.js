import React, { useEffect, useState } from 'react'
import { Alert, Typography, Container, Box, TextField, InputLabel, MenuItem, FormControl,
    Select,Button } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { useNavigate } from 'react-router-dom';
import { SPECIALIZATIONS } from "../data/Specializations.js";


const AddDoctorPage = () => {
    const navigate = useNavigate();
    const [execute, status, , isLoading] = useAPIFetch(ENDPOINT.DOCTORS, "POST");

    useEffect(() => {
        if (status === 200 && !isLoading) {
            navigate('/Doctors');
        }
    }, [status, isLoading, navigate]);

    const handleSubmit = (event) => {
        event.preventDefault();
        if (error == null) {
            const data = Object.fromEntries(new FormData(event.currentTarget).entries());
            execute(data);
        }
    }

    const [error, setError] = useState(null);

    function isValidEmail(email) {
        return /^[A-Z0-9._%+-]+@[A-Z0-9.-]/i.test(email);
    }

    const handleEmailChange = event => {
        if (!isValidEmail(event.target.value)) {
            setError('Incorrect email.');
        } else {
            setError(null);
        }
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
                >Register</Typography>
                {status === 404 && <Alert severity="error">User already exists.</Alert>}

                <Box component="form" onSubmit={handleSubmit}>
                    <FormControl fullWidth>
                        <InputLabel id="specialization-label">Specialization</InputLabel>
                        <Select
                            labelId="specialization-label"
                            id="specialization"
                            name="specialization"
                            label="Specialization"
                        >
                            {SPECIALIZATIONS.map((specialization) => (
                                <MenuItem value={specialization}>{specialization}</MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                    
                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Name"
                        id="name"
                        name="name"
                    ></TextField>

                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Surname"
                        id="surname"
                        name="surname"
                    ></TextField>

                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Email"
                        id="email"
                        name="email"
                        onChange={handleEmailChange}
                    ></TextField>

                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Password"
                        id="password"
                        type="password"
                        name="password"
                    ></TextField>

                    <TextField
                        fullWidth
                        required
                        margin="normal"
                        label="Confirm password"
                        id="confirmPassword"
                        type="password"
                        name="confirmPassword"
                    ></TextField>

                    <Typography
                        sx={{
                            color: 'red',
                            fontSize: "4"
                        }}>
                        {error}
                    </Typography>

                    <Button
                        fullWidth
                        variant="contained"
                        type="submit">
                        Reqister</Button>


                </Box>
            </Box>
        </Container>
    )
}

export default AddDoctorPage
