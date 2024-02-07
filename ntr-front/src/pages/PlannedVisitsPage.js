import React, { useEffect } from 'react'
import { Alert, Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";


const PlannedVisitsPage = () => {

    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.SCHEDULE_GET_PLANNED, "GET");
    const [executeCancel, statusCancel, , , setStatusCancel] = useAPIFetch(ENDPOINT.SCHEDULE_CANCEL, "POST");

    useEffect(() => {
        if (status === 0) {
            execute();
        }
        if (statusCancel === 200) {
            execute();
            setStatusCancel(201);
        }
    }, [status, isLoading, statusCancel, execute, setStatusCancel]);
    
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

                {status === 200 && data !== undefined && data !== null && data.length > 0 &&
                <><Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center'
                        sx={{ marginBottom: 4 }}
                    >Patients</Typography>
                    {statusCancel === 201 && <Alert severity="success">Visit cancelled!</Alert>}
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Specialization</TableCell>
                                    <TableCell align="center">Doctor Name</TableCell>
                                    <TableCell align="center">Date</TableCell>
                                    <TableCell align="center">Cancel</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell component="th" scope="row" align="center">{row.specialization}</TableCell>
                                        <TableCell align="center">{row.doctorName}</TableCell>
                                        <TableCell align="center">{row.date}</TableCell>
                                        <TableCell align="center">
                                             <Button
                                                key={row.id}
                                                sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.red,  }}
                                                onClick={() => {
                                                    executeCancel(row.id);
                                                } }
                                            >
                                                Cancel
                                            </Button>
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

export default PlannedVisitsPage
