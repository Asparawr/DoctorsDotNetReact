import React, { useEffect } from 'react'

import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import user from '../user.js';
import LogoutButton from "./LogoutButton.js";
import { COLORS } from "../theme.ts";

function ResponsiveAppBar() {
  const navigate = useNavigate();
  const [pages, setPages] = React.useState([]);
  const [names, setNames] = React.useState([]);

  useEffect(() => {
    if (!user.isLoggedIn) {
      setPages(['Login', 'Register']);
      setNames(['Login', 'Register']);
      if (window.location.pathname !== "/Login" && window.location.pathname !== "/Register")
        navigate("/Login");
    }
    else if (user.isDirector) {
      setPages(['Start', 'Patients', 'Doctors', 'Report']);
      setNames(['Start', 'Patients', 'Doctors', 'Report']);

    } else if (user.isDoctor) {
      setPages(['Start', 'DoctorSchedule', 'DoctorVisit']);
      setNames(['Start', 'Schedule', 'Visits']);

    } else {
      setPages(['Start', 'PlanVisit', 'PlannedVisits', 'PatientVisit']);
      setNames(['Start', 'Plan Visit', 'Planned Visits', 'Visits History']);
    }
  }, [navigate]);

  return (
    <AppBar container="true" spacing={2.5}>
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        sx={{ textAlign: "center", display: 'flex' }}>
        {pages.map((page, index) => (
          window.location.pathname === "/" + page ?
            <Button
              key={page}
              sx={{
                my: 2, color: 'white', display: 'block', backgroundColor: COLORS.blueHardest
                , ":hover": {
                  backgroundColor: COLORS.blueHardest,
                }
              }}
              onClick={() => navigate(`/${page}`)}
            >
              {names[index]}
            </Button>
            :
            <Button
              key={page}
              sx={{ my: 2, color: 'white', display: 'block' }}
              onClick={() => navigate(`/${page}`)}
            >
              {names[index]}
            </Button>

        ))}

      </Box>

      <Box >
        {(user.token === '') ? null : <LogoutButton />}
      </Box>
    </AppBar >
  );
}
export default ResponsiveAppBar;
