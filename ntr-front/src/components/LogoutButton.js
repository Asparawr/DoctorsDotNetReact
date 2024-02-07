import * as React from 'react';
import { AppBar, Button, Typography } from '@mui/material'

function LogoutButton() {
  return (
    <AppBar container="true">
      <Typography component="span" sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        position: "fixed",
        top: 0,
        right: 20,
      }}>
        <Button
          key="logout"
          sx={{ my: 2, color: 'white', display: 'block' }}
          href="/"
        >
          Logout
        </Button>
      </Typography>
    </AppBar>
  );
}
export default LogoutButton;
