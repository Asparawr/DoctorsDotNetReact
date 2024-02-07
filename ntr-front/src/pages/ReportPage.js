import React, { useEffect, useState } from 'react'
import dayjs from 'dayjs';
import { Typography, Container, Box, Table, TableBody, TableCell, TableContainer,
         TableHead, TableRow,Paper, Button, Grid } from "@mui/material";

        
import {LocalizationProvider, DateCalendar} from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { COLORS } from "../theme.ts";


const ReportPage = () => {

    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.REPORT, "POST");

    const doctors = data;
    const [currentStartPicker, setcurrentStartPicker] = useState( dayjs('2024-01-01T8:30'));
    const [currentEndPicker, setcurrentEndPicker] = useState( dayjs('2024-01-30T15:30'));
    
    useEffect(() => {
    }, [status, isLoading]);

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
                >Report</Typography>

                {(status === 200 && doctors !== undefined && doctors !== null) ? 
                <>
                    <TableContainer component={Paper} align="center">
                        <Table sx={{ minWidth: 650, maxWidth: 1000 }} aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell align="center">Name</TableCell>
                                    <TableCell align="center">Work hours</TableCell>
                                    <TableCell align="center">Visit count</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {doctors.map((row, index) => (
                                    <TableRow
                                        key={row.id}
                                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                    >
                                        <TableCell align="center">{row.doctorName}</TableCell>
                                        <TableCell align="center">{row.workHours}</TableCell>
                                        <TableCell align="center">{row.visitCount}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    </>
                :
                <>
                    Select a date range to show a report.
                    <Grid container direction="row" style={{ marginTop: "50px", width: "50%" }} alignItems="center" justifyContent="center">
                        <Box sx={{ minWidth: 120}}>
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateCalendar showDaysOutsideCurrentMonth fixedWeekNumber={6} 
                                date={currentStartPicker}
                                defaultValue={currentStartPicker}
                                onChange={(newValue) => {
                                    setcurrentStartPicker(newValue);
                                }}
                            />
                            </LocalizationProvider>
                        </Box>
                        <Box sx={{ minWidth: 120}}>
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateCalendar showDaysOutsideCurrentMonth fixedWeekNumber={6} 
                                date={currentEndPicker}
                                defaultValue={currentEndPicker}
                                onChange={(newValue) => {
                                    setcurrentEndPicker(newValue);
                                }}
                            />
                            </LocalizationProvider>
                        </Box>
                    </Grid>
                    <Button variant="contained" sx={{ marginTop: 2, marginBottom: 1, backgroundColor: COLORS.primary,
                    color: COLORS.white, width: '20%', alignSelf: 'center'
                    }} onClick={() => {
                        execute({ "startDate": currentStartPicker.format('YYYY-MM-DD'), "endDate": currentEndPicker.format('YYYY-MM-DD') });
                    }}>
                        Show
                    </Button>
                </>
                }
            </Box>
        </Container>
    )
}

export default ReportPage
