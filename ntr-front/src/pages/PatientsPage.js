import React, { useEffect, useState } from 'react'
import { Alert, Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";


const PatientsPage = () => {

    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.PATIENTS, "GET");
    const [executeAccept, statusAccept, , isLoadingAccept, setStatusAccept] = useAPIFetch(ENDPOINT.PATIENTS, "POST");

    const patients = data;
    const [current_patient_index, set_current_patient_index] = useState(0);
    
    useEffect(() => {
        if (status === 0) {
            execute();
        }
        if (statusAccept === 200 && !isLoadingAccept) {
            patients[current_patient_index].isAccepted = true;
            setStatusAccept(201);
        }
    }, [status, isLoading, statusAccept, isLoadingAccept, current_patient_index, execute, patients, setStatusAccept]);
    
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
                {statusAccept === 201 && <Alert severity="success">Patient accepted!</Alert>}

                {status === 200 && patients !== undefined && patients !== null && patients.length > 0 &&
                <><Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center'
                        sx={{ marginBottom: 4 }}
                    >Patients</Typography>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Name</TableCell>
                                    <TableCell align="center">Surname</TableCell>
                                    <TableCell align="center">Email</TableCell>
                                    <TableCell align="center">Accept</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {patients.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell component="th" scope="row" align="center">
                                            {row.name}
                                        </TableCell>
                                        <TableCell align="center">{row.surname}</TableCell>
                                        <TableCell align="center">{row.email}</TableCell>
                                        <TableCell align="center">
                                            {!row.isAccepted && <Button
                                                key={row.id}
                                                sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                                                onClick={() => {
                                                    set_current_patient_index(index);
                                                    executeAccept(row.id);
                                                } }
                                            >
                                                Accept
                                            </Button>}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer></>
                }
            </Box>
        </Container>
    )
}

export default PatientsPage
