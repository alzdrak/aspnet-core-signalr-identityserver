import React, { Component } from "react";
//import "./login.css";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {};
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

  render() {
    this.getToken();

    return (
      <div className="login">
        <div />
      </div>
    );
  }
}

export default Login;
