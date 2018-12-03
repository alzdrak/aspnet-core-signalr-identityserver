import React, { Component } from "react";
import { withRouter } from "react-router-dom";

class Profile extends Component {
  constructor(props) {
    super(props);

    this.state = { auth: props.auth };
  }

  render() {
    return this.state.auth.getIsAuthenticated() ? (
      <p>
        Welcome!{" "}
        <button
          onClick={() => {
            this.state.auth.signOut(() => this.props.history.push("/"));
          }}
        >
          Sign out
        </button>
      </p>
    ) : (
      <p>You are not logged in.</p>
    );
  }
}

export default withRouter(Profile);
