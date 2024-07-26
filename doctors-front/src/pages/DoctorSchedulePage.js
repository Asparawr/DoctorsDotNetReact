import React, { useEffect } from 'react'
import { Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';


const DoctorSchedulePage = () => {

    const [execute, status, data, ] = useAPIFetch(ENDPOINT.SCHEDULE_GET_DOCTOR_SCHEDULED, "GET");
    
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
                <Typography
                    variant="h4"
                    color="black"
                    fontSize="40"
                    fontWeight="bold"
                    align='center'
                    sx={{ marginBottom: 4 }}
                    >Schedule
                </Typography>
                {status === 200 && data !== undefined && data !== null && data.length > 0 &&
                <>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Date</TableCell>
                                    <TableCell align="center">Work day start</TableCell>
                                    <TableCell align="center">Work day end</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell component="th" scope="row" align="center">{row.date.substring(0, 10)}</TableCell>
                                        <TableCell align="center">{row.startTime}</TableCell>
                                        <TableCell align="center">{row.endTime}</TableCell>
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

export default DoctorSchedulePage
