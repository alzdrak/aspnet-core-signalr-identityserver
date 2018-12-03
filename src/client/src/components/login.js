import React, { Component } from "react";
import { Redirect, withRouter } from "react-router-dom";
//import { Auth } from "./auth";
import "./login.css";
import jupiter from "../images/3.png";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = { auth: props.auth, redirectToReferrer: false };

    this.openLogin = this.openLogin.bind(this);
    this.login = this.login.bind(this);
  }

  openLogin() {
    var loginWrapper = document.querySelector(".login-wrapper");

    loginWrapper.classList.toggle("open");
  }

  login() {
    var username = document.querySelector("#username").value;
    var password = document.querySelector("#password").value;

    this.state.auth.authenticate(username, password);

    var token = this.state.auth.getToken();
    if (token !== {} && token.access_token !== null) {
      this.setState({ redirectToReferrer: true });
    }
  }

  //signup() {}

  render() {
    //const { redirectToReferrer } = this.state;

    if (this.state.redirectToReferrer) {
      return <Redirect to="/" />;
    }

    return (
      <div className="">
        {/* <div className="login"> */}
        <div className="login-wrapper">
          <div className="login-left">
            <button className="image-button" onClick={this.openLogin}>
              <img src={jupiter} alt="" />
            </button>

            {/* <button className="h1" onClick={this.openLogin}>
            Login
          </button> */}
          </div>
          <div>
            <div className="login-right">
              <div className="h2">Register</div>

              <div className="form-group">
                <label htmlFor="username">
                  <input
                    className="login-input"
                    id="username"
                    type="text"
                    placeholder=""
                    required
                  />
                  <span className="label-text">Username</span>
                  <span className="focus-border" />
                </label>
              </div>
              <div className="clearfix" />
              <div className="form-group">
                <label htmlFor="email">
                  <input
                    className="login-input"
                    id="email"
                    type="text"
                    placeholder=""
                    required
                  />
                  <span className="label-text">Email</span>
                  <span className="focus-border" />
                </label>
              </div>
              <div className="form-group">
                <label htmlFor="password">
                  <input
                    className="login-input"
                    id="password"
                    type="password"
                    placeholder=""
                    required
                  />
                  <span className="label-text">Password</span>
                  <span className="focus-border" />
                </label>
              </div>
              <div className="checkbox-container">
                <input type="checkbox" />
                <div className="text-checkbox">
                  I agree with the terms of service.
                </div>
              </div>
              <div className="button-area">
                <button className="btn btn-secondary" onClick={this.login}>
                  Login
                </button>
                <button className="btn btn-primary" onClick={this.signup}>
                  Sign Up
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default withRouter(Login);
