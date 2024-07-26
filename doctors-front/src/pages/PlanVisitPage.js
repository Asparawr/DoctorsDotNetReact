import React, { useEffect, useState } from 'react'
import { Alert, Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button, Select, MenuItem } from "@mui/material";
import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";
import { SPECIALIZATIONS } from "../data/Specializations.js";


const PlanVisitPage = () => {

    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.SCHEDULE_SPECIALIZATION, "POST");
    const [executePlan, statusPlan, , isLoadingPlan, setStatusPlan] = useAPIFetch(ENDPOINT.SCHEDULE_PLAN, "POST");

    const doctors = data;
    const [currentSpecialization, setCurrentSpecialization] = useState( SPECIALIZATIONS[0] );
    
    useEffect(() => {
        if (statusPlan === 200) {
            setStatusPlan(201);
            execute(currentSpecialization);
        }
    }, [status, isLoading, statusPlan, isLoadingPlan, execute, currentSpecialization, setStatusPlan]);

    return (
        <Container component="main" maxWidth="d">
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
                >Plan Visit</Typography>

                {statusPlan === 201 && <Alert severity="success">Registered!</Alert>}
                {(status === 200 && doctors !== undefined && doctors !== null) ? 
                <>
                    <TableContainer component={Paper} align="center">
                        <Table sx={{ maxWidth: 650 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Name</TableCell>
                                    <TableCell align="center">Date</TableCell>
                                    <TableCell align="center">Schedule</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {doctors.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell align="center">{row.doctorName}</TableCell>
                                        <TableCell align="center">{row.date.substring(0, 10) + " " + row.date.substring(11, 16)}</TableCell>
                                        <TableCell align="center">{
                                        <Button
                                            key={row.id}
                                            sx={{ my: 0, color: 'white', display: 'block', width: '100%', height: '100%', backgroundColor: COLORS.blueHardest }}
                                            onClick={() => {
                                                executePlan(row.id);
                                            }}>
                                                Plan visit
                                            </Button>
                                        }</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    </>
                :
                <>
                    Select specialization.
                    <Select
                        labelId="specialization-label"
                        id="specialization"
                        name="specialization"
                        label="Specialization"
                        defaultValue={SPECIALIZATIONS[0]}
                        onChange={ (event) => { setCurrentSpecialization(event.target.value) } }
                        sx={{ marginTop: 3, marginBottom: 2, width: '20%', alignSelf: 'center' }}
                    >
                        {SPECIALIZATIONS.map((specialization) => (
                            <MenuItem value={specialization}>{specialization}</MenuItem>
                        ))}
                    </Select>
                    <Button variant="contained" sx={{ marginTop: 2, marginBottom: 1, backgroundColor: COLORS.primary,
                    color: COLORS.white, width: '20%', alignSelf: 'center'
                    }} onClick={() => {
                        execute(currentSpecialization);
                    }}>
                        Show doctors
                    </Button>
                </>
                }
            </Box>
        </Container>
    )
}

export default PlanVisitPage
