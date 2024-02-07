import React, { useEffect, useState } from 'react'
import { Alert, Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button, TextField } from "@mui/material";

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";


const DoctorVisitPage = () => {

    const [execute, status, data, isLoading, setStatus] = useAPIFetch(ENDPOINT.SCHEDULE_GET_DOCTOR_VISITS, "GET");
    const [executeVisit, statusVisit, , , setStatusVisit] = useAPIFetch(ENDPOINT.SCHEDULE_ADD_DESC, "POST");

    const [currentRow, set_current_row] = useState(0);
    const [currentDesc, set_current_desc] = useState("");
    
    useEffect(() => {
        if (status === 0) {
            execute();
        }
        if (statusVisit === 200) {
            execute();
            setStatusVisit(201);
        }
    }, [status, isLoading, statusVisit, execute, setStatusVisit]);
    
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
                    >Describe visit</Typography>
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
                        Patient name: {data[currentRow].patientName} 
                        <br />
                        <br />
                        Date: {data[currentRow].date.substring(0, 10) + " " + data[currentRow].date.substring(11, 16)}
                        <br />
                        <br />
                    </Typography>
                        <TextField
                            id="outlined-multiline-static"
                            label="Description"
                            multiline
                            rows={10}
                            variant="outlined"
                            sx={{ width: '100%', minWidth: 650}}
                            onChange={(e) => set_current_desc(e.target.value)}
                        />

                        <Button
                            sx={{ mt: 3, mb: 2, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                            onClick={() => {
                                executeVisit( {id: data[currentRow].id, description: currentDesc} );
                                setStatus(200);
                            } }
                        >
                            Submit and end visit
                        </Button>
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
                    {statusVisit === 201 && <Alert severity="success">Visit completed!</Alert>}
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
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
                                        <TableCell component="th" scope="row" align="center">{row.patientName}</TableCell>
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
                                                Add description
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

export default DoctorVisitPage
