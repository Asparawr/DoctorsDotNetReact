import React, { useEffect, useState } from 'react'
import dayjs from 'dayjs';
import { Typography, Container, Box, Button, Grid } from "@mui/material";
    
import {LocalizationProvider, DateCalendar} from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { TimePicker } from '@mui/x-date-pickers/TimePicker';
import { PickersDay } from '@mui/x-date-pickers';

import useAPIFetch, { ENDPOINT } from '../useFetch.ts';
import { useLocation } from 'react-router-dom';
import { COLORS } from "../theme.ts";



const SchedulePage = () => {

    const [execute, status, data, isLoading] = useAPIFetch(ENDPOINT.SCHEDULE_DATES, "POST");
    const [executeAdd, statusAdd, , isLoadingAdd, setStatusAdd] = useAPIFetch(ENDPOINT.SCHEDULE_ADD, "POST");
    const [executeRemove, statusRemove, , , setStatusRemove] = useAPIFetch(ENDPOINT.SCHEDULE_REMOVE, "POST");
    const [executeSelectDay, statusSelectDay, dataSelectDay, , setStatusSelectDay] = useAPIFetch(ENDPOINT.SCHEDULE_SELECT, "POST");
    const [executeCopy, statusCopy, , , setStatusCopy] = useAPIFetch(ENDPOINT.SCHEDULE_COPY_WEEK, "POST");
    const { state } = useLocation();
    const { doctorId } = state;

    const dates = data;
    const [selectedDate, setselectedDate] = React.useState(dayjs('2024-01-27'));
    const [currentStartPicker, setcurrentStartPicker] = useState( dayjs('2022-04-17T8:30'));
    const [currentEndPicker, setcurrentEndPicker] = useState( dayjs('2022-04-17T15:30'));
    
    useEffect(() => {
        if (status === 0 && doctorId !== undefined) {
            execute(doctorId);
        }
        if (statusAdd === 200) {
            execute(doctorId);
            setStatusAdd(0);
        }
        if (statusRemove === 200) {
            execute(doctorId);
            setStatusRemove(0);
        }
        if (statusSelectDay === 200) {
            setcurrentStartPicker(dayjs(dataSelectDay.workDayStart, 'HH:mm:ss'));
            setcurrentEndPicker(dayjs(dataSelectDay.workDayEnd, 'HH:mm:ss'));
            setStatusSelectDay(0);
        }
        if (statusCopy === 200) {
            execute(doctorId);
            setStatusCopy(0);
        }
    }, [status, isLoading, statusAdd, isLoadingAdd, doctorId, statusRemove, statusSelectDay, dataSelectDay, statusCopy, execute, setStatusAdd, setStatusCopy, setStatusRemove, setStatusSelectDay]);

    const addHours = () => {
        executeAdd({ "doctorId": doctorId, "Date": selectedDate.format('YYYY-MM-DD'), "WorkDayStart": currentStartPicker.format('HH:mm:ss'), "WorkDayEnd": currentEndPicker.format('HH:mm:ss') });
    }

    const CustomDay = (props) => { // check if day is in the array of dates
        const isHighlighted = dates.some((date) => dayjs(date).isSame(props.day, 'day'));
        const matchedStyles = isHighlighted
            ? { backgroundColor: "#ffe8e8", color: "#ff0000" }
            : {};
        return <PickersDay {...props} sx={{ ...matchedStyles }} />;
    };

    return (
        <Container component="main" maxWidth="d">
            <Box
                sx={{
                    width: '100%',
                    marginTop: 4,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    flexGrow: 1
                }}
                width="100%">

                {status === 200 && dates !== undefined && dates !== null && dates.length > 0 &&
                <><Typography
                        variant="h4"
                        color="black"
                        fontSize="40"
                        fontWeight="bold"
                        align='center' direction="row"
                        sx={{ marginBottom: 4 }}
                    >Schedule</Typography>

                    <Grid container direction="row" style={{ marginTop: "50px", width: "50%" }} alignItems="center" justifyContent="center">
                        <Box sx={{ minWidth: 120}}>
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateCalendar showDaysOutsideCurrentMonth fixedWeekNumber={6} 
                                date={selectedDate}
                                onChange={(newValue) => {
                                    setselectedDate(newValue);
                                    executeSelectDay({ "doctorId": doctorId, "Date": newValue.format('YYYY-MM-DD') });
                                }}
                                slots={{ day: CustomDay }}
                            />
                            </LocalizationProvider>
                        </Box>
                        <Grid container style={{ marginTop: "50px", width: "50%"} } alignItems="center" justifyContent="center">
                            <Typography
                                variant="h4"
                                color="black"
                                fontSize="40"
                                fontWeight="bold"
                                alignSelf='center'
                            >Working hours</Typography>
                            <Grid Item style={{ marginTop: "70px", width: "100%"} } alignSelf={'center'}>
                                <LocalizationProvider dateAdapter={AdapterDayjs}>
                                    <TimePicker
                                    label="Start"
                                    defaultValue={currentStartPicker}
                                    value={currentStartPicker}
                                    onChange={setcurrentStartPicker}
                                    />
                                    <TimePicker
                                    label="End"
                                    value={currentEndPicker}
                                    onChange={setcurrentEndPicker}
                                    />
                                    </LocalizationProvider>
                                    <Button variant="contained" sx={{ marginTop: 1, marginBottom: 1, backgroundColor: COLORS.primary, 
                                    color: COLORS.white, width: '47%', alignSelf: 'center'
                                    }} onClick={() => {
                                        addHours();
                                    }}>
                                        Set
                                    </Button>
                                    {dates.some((date) => dayjs(date).isSame(selectedDate, 'day')) && 
                                        <Button variant="contained" sx={{ marginTop: 0, marginBottom: 0, backgroundColor: COLORS.red,
                                        color: COLORS.white, width: '47%', alignSelf: 'center'
                                        }} onClick={() => {
                                            setStatusRemove(0);
                                            executeRemove({ "doctorId": doctorId, "Date": selectedDate.format('YYYY-MM-DD') });
                                        }}>
                                            Remove
                                        </Button>
                                    }
                            </Grid>
                        </Grid>
                    </Grid>
                    <Button variant="contained" sx={{ marginTop: 4, marginBottom: 4, backgroundColor: COLORS.primary, 
                    color: COLORS.white, width: '20%', alignSelf: 'center'
                    }} onClick={() => {
                        setStatusCopy(0);
                        executeCopy({ "doctorId": doctorId, "Date": selectedDate.format('YYYY-MM-DD') });
                    }}>
                        Copy previous week
                    </Button>
                </>
                }
            </Box>
        </Container>
    )
}

export default SchedulePage
