import { IconButton, Snackbar } from "@material-ui/core";
import { IoIosClose } from "react-icons/io";
import React from "react";
import { ToastrConsumer } from "../contexts/toastr.context";

const SharedSnackbar = () => (
  <ToastrConsumer>
    {({ snackbarIsOpen, message, closeSnackbar }) => (
      <Snackbar
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left"
        }}
        open={snackbarIsOpen}
        autoHideDuration={6000}
        onClose={closeSnackbar}
        message={message}
        action={[
          <IconButton key="close" color="inherit" onClick={closeSnackbar}>
            <Close />
          </IconButton>
        ]}
      />
    )}
  </ToastrConsumer>
);

export default SharedSnackbar;
