import {
  BrowserRouter as Router,
  Routes,
  Route,
} from 'react-router-dom';

import LoginPage from "./pages/LoginPage.js";
import RegisterPage from "./pages/RegisterPage.js";
import StartPage from "./pages/StartPage.js";
import PatientsPage from "./pages/PatientsPage.js";
import AccessDeniedPage from "./pages/AccessDeniedPage.js";
import ErrorPage from "./pages/ErrorPage.js";
import DoctorsPage from "./pages/DoctorsPage.js";
import AddDoctorPage from "./pages/AddDoctorPage.js";
import SchedulePage from "./pages/SchedulePage.js";
import ReportPage from "./pages/ReportPage.js";
import PlanVisitPage from "./pages/PlanVisitPage.js";
import PlannedVisitsPage from "./pages/PlannedVisitsPage.js";
import DoctorSchedulePage from "./pages/DoctorSchedulePage.js";
import DoctorVisitPage from "./pages/DoctorVisitPage.js";
import PatientVisitPage from "./pages/PatientVisitPage.js";
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import ResponsiveAppBar from './components/ResponsiveAppBar.js'

const theme = createTheme({
  components: {
    MuiButton: {
      variants: [
        {
          props: { variant: "contained" },
          style: {
            minWidth: 200, minHeight: 60
          }
        }
      ]
    }
  }
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <div className="App">
        <Typography component="span" sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          position: "fixed",
          bottom: 0,
        }}>
        </Typography>
        <header className="App-header">
          <Box
            sx={{
              marginTop: 10,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}>


            <Router>
              <ResponsiveAppBar />
              <Routes>
                <Route exact path="/" element={<LoginPage />} />
                <Route exact path="/Login" element={<LoginPage />} />
                <Route exact path="/Register" element={<RegisterPage />} />
                <Route exact path="/Start" element={<StartPage />} />
                <Route exact path="/Patients" element={<PatientsPage />} />
                <Route exact path="/AccessDenied" element={<AccessDeniedPage />} />
                <Route exact path="/Error" element={<ErrorPage />} />
                <Route exact path="/Doctors" element={<DoctorsPage />} />
                <Route exact path="/AddDoctor" element={<AddDoctorPage />} />
                <Route exact path="/Schedule" element={<SchedulePage />} />
                <Route exact path="/Report" element={<ReportPage />} />
                <Route exact path="/PlanVisit" element={<PlanVisitPage />} />
                <Route exact path="/PlannedVisits" element={<PlannedVisitsPage />} />
                <Route exact path="/DoctorSchedule" element={<DoctorSchedulePage />} />
                <Route exact path="/DoctorVisit" element={<DoctorVisitPage />} />
                <Route exact path="/PatientVisit" element={<PatientVisitPage />} />
              </Routes>
            </Router>
          </Box>
        </header>
      </div>
    </ThemeProvider>
  );
}

export default App;
