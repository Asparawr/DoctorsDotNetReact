import React from 'react'
import user from '../user.js';
import { Typography, Box, Container } from "@mui/material";

const StartPage = () => {
    const roleText = user.isDirector ? "Director" : user.isDoctor ? "Doctor" : "Patient";

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
                >Hello {roleText}</Typography>
                {roleText === "Patient" && !user.isAcceptedPatient && <Typography
                    variant="h4"
                    color="black"
                    fontSize="40"
                    fontWeight="bold"
                    align='center'
                >{"You are not accepted\n Please wait for access"}</Typography>}
            </Box>
        </Container>
    )
}

export default StartPage
