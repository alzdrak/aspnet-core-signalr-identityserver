import React, { Component } from "react";
import "./login.css";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {};

    this.openLogin = this.openLogin.bind(this);
  }

  /**
   * encode as uri string eg. ?client=test&username=alice...
   */
  encodeParams(params) {
    let body = "";
    for (let key in params) {
      if (body.length) {
        body += "&";
      }
      body += key + "=";
      body += encodeURIComponent(params[key]);
    }
    return body;
  }

  /**
   * Stores access token & refresh token.
   */
  //store(body) {
  //Helpers.setToken('id_token', body.access_token);
  //Helpers.setToken('refresh_token', body.refresh_token);
  // Calculates token expiration.
  //this.expiresIn = <number>body.expires_in * 1000; // To milliseconds.
  //Helpers.setExp(this.authTime + this.expiresIn);
  //}

  //get new token from identity server
  getToken() {
    let params = {
      client_id: "client",
      client_secret: "secret",
      grant_type: "password",
      username: "alice",
      password: "Pass123$",
      scope: "server offline_access"
    };

    //http request
    fetch("http://localhost:50000/connect/token", {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      },
      body: this.encodeParams(params)
    })
      .then(function(response) {
        return response.json();
      })
      .then(function(data) {
        if (typeof data.access_token !== "undefined") {
          // Stores access token & refresh token.
          //this.store(body);
          //this.getUserInfo();
          // Tells all the subscribers about the new status.
          //this.signinSubject.next(true);
        }
      })
      .catch(function(ex) {
        console.log("parsing failed", ex); // eslint-disable-line no-console
      });
  }

  openLogin() {
    var loginWrapper = document.querySelector(".login-wrapper");

    loginWrapper.classList.toggle("open");
  }

  render() {
    // openLoginRight.addEventListener("click", function() {
    //   loginWrapper.classList.toggle("open");
    // });

    this.getToken();

    return (
      <div className="login-wrapper">
        <div className="login-left">
          <button className="image-button" onClick={this.openLogin}>
            <img
              src="http://res.cloudinary.com/dzqowkhxu/image/upload/v1513679279/bg-login_bxxfkf.png"
              alt=""
            />
          </button>

          {/* <button className="h1" onClick={this.openLogin}>
            Login
          </button> */}
        </div>
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
            <button className="btn btn-secondary">Login</button>
            <button className="btn btn-primary">Sign Up</button>
          </div>
        </div>
      </div>
    );
  }
}

export default Login;
