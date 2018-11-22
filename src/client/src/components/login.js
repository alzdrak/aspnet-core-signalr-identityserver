import React, { Component } from 'react';
import './login.css';

class Login extends Component {

  constructor(props) {
    super(props);

    this.state = {};
  }

  getToken() {
    var authorizationUrl = 'http://localhost:50000/connect/authorize';
    var client_id = 'client';
    //var redirect_uri = 'http://localhost:28895/index.html';
    var response_type = "token";
    var scope = "server";
    var state = Date.now() + "" + Math.random();
    localStorage["state"] = state;
    var url =
        authorizationUrl + "?" +
        "client_id=" + encodeURI(client_id) + "&" +
        "response_type=" + encodeURI(response_type) + "&" +
        "scope=" + encodeURI(scope) + "&" +
        "state=" + encodeURI(state);
    window.location = url;
  }

  render() {
    return (
      <div className="login">
        <div></div>
      </div>
    );
  }
}

export default Login;
