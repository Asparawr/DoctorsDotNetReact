import React from 'react'
import { Typography, Box, Container, Alert } from "@mui/material";

const AccessDeniedPage = () => {

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
                    align='center'
                >
                <Alert severity="error">Access denied</Alert>
                </Typography>
            </Box>
        </Container>
    )
}

export default AccessDeniedPage
