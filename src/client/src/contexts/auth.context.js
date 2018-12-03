import React from "react";
import Auth from "../services/auth";

export const auth = new Auth();

export const AuthContext = React.createContext({
  auth: auth // default value
});
