import React, { useEffect } from 'react'
import { Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { useNavigate } from 'react-router-dom';
import { COLORS } from "../theme.ts";


const DoctorsPage = () => {

    const [execute, status, data, ] = useAPIFetch(ENDPOINT.DOCTORS, "GET");
    const navigate = useNavigate();

    const doctors = data;
    
    useEffect(() => {
        if (status === 0) {
            execute();
        }
    }, [status, execute]);

    return (
        <Container component="main" maxWidth="md">
            <Box
                sx={{
                    width: '100%',
                    marginTop: 4,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    flexGrow: 1,
                }}>

                {status === 200 && doctors !== undefined && doctors !== null && doctors.length > 0 &&
                <><Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center'
                        sx={{ marginBottom: 4 }}
                    >Doctors</Typography>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Name</TableCell>
                                    <TableCell align="center">Surname</TableCell>
                                    <TableCell align="center">Email</TableCell>
                                    <TableCell align="center">Specialization</TableCell>
                                    <TableCell align="center">Schedule</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {doctors.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell component="th" scope="row" align="center">
                                            {row.name}
                                        </TableCell>
                                        <TableCell align="center">{row.surname}</TableCell>
                                        <TableCell align="center">{row.email}</TableCell>
                                        <TableCell align="center">{row.specialization}</TableCell>
                                        <TableCell align="center">
                                            <Button
                                                key={row.id}
                                                sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                                                onClick={() => {
                                                    navigate("/Schedule", { state: { doctorId: row.id } });
                                                } }
                                            >
                                            Change schedule
                                            </Button>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <Button
                        key="add_doctor"
                        sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                        onClick={() => {
                            navigate("/AddDoctor");
                        }}
                    >
                    Add doctor
                    </Button>
                    </>
                }
            </Box>
        </Container>
    )
}

export default DoctorsPage
