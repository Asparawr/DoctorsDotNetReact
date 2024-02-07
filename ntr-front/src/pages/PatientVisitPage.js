import React, { useEffect, useState } from 'react'
import { Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button, TextField } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";


const PatientVisitPage = () => {

    const [execute, status, data, , setStatus] = useAPIFetch(ENDPOINT.SCHEDULE_GET_VISIT_HISTORY, "GET");

    const [currentRow, set_current_row] = useState(0);
    
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

                {status === 201 && <>
                    <Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center'
                        sx={{ marginBottom: 4 }}
                    >Visit Description</Typography>
                    <Box
                        component="form"
                        noValidate
                        sx={{ mt: 1 }}
                    >
                    <Typography
                        variant="h6"
                        color="black"
                        fontSize="10"
                        align='left'
                        sx={{ marginBottom: 4, marginLeft: 10}}>

                        <br />
                        Specialization: {data[currentRow].specialization}
                        <br />
                        <br />
                        Doctor name: {data[currentRow].doctorName} 
                        <br />
                        <br />
                        Date: {data[currentRow].date.substring(0, 10) + " " + data[currentRow].date.substring(11, 16)}
                        <br />
                        <br />
                    </Typography>
                        <TextField
                            id="outlined-multiline-static"
                            multiline
                            rows={10}
                            variant="outlined"
                            sx={{ width: '100%', minWidth: 650 }}
                            disabled={true}
                            value={data[currentRow].description}
                        />
                    </Box>
                </>
                }
                {status === 200 && data !== undefined && data !== null && data.length > 0 &&
                <><Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center'
                        sx={{ marginBottom: 4 }}
                    >Visits</Typography>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Specialization</TableCell>
                                    <TableCell align="center">Name</TableCell>
                                    <TableCell align="center">Date</TableCell>
                                    <TableCell align="center">Add Description</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell component="th" scope="row" align="center">{row.specialization}</TableCell>
                                        <TableCell component="th" scope="row" align="center">{row.doctorName}</TableCell>
                                        <TableCell align="center">{row.date.substring(0, 10) + " " + row.date.substring(11, 16)}</TableCell>
                                        <TableCell align="center">
                                            <Button
                                                key={row.id}
                                                sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                                                onClick={() => {
                                                    set_current_row(index);
                                                    setStatus(201);
                                                } }
                                            >
                                                Show description
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

export default PatientVisitPage
