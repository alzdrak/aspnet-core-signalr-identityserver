import React, { Component } from "react";
import {
  BrowserRouter as Router,
  Link,
  Switch
  //Redirect,
  //withRouter
} from "react-router-dom";
//import { TransitionGroup, CSSTransition } from "react-transition-group";
import "./app.css";

//contexts
import { AuthContext } from "./contexts/auth.context";

//components
import PrivateRoute from "./components/private.route";
import Login from "./components/login";
import Chat from "./components/chat/chat";
import Profile from "./components/profile/profile";
import PublicRoute from "./components/public.route";

//services
//import Auth from "./services/auth";

class App extends Component {
  constructor() {
    super();

    //this.state = { auth: new Auth() };
  }

  render() {
    return (
      <AuthContext.Provider>
        <Router>
          <div>
            <div>
              Authenticated:{" "}
              {this.state.auth.getIsAuthenticated() ? "true" : "false"}
            </div>
            <Profile auth={this.state.auth} />
            <ul>
              <li>
                <Link to="/login">Login</Link>
              </li>
              <li>
                <Link to="/">Chat</Link>
              </li>
            </ul>

            <br />
            <div>
              <Switch>
                {/* <PrivateRoute
                exact={true}
                path="/"
                render={props => <Chat {...props} />}
              /> */}

                <PrivateRoute
                  exact={true}
                  path="/"
                  auth={this.state.auth}
                  component={Chat}
                />

                <PublicRoute
                  path="/login"
                  auth={this.state.auth}
                  component={Login}
                />

                {/* <Route
                path="/login"
                render={props => <Login {...props} auth={this.state.auth} />}
              /> */}

                {/* <Redirect to='/' /> */}
              </Switch>
            </div>
          </div>
        </Router>
      </AuthContext.Provider>
    );
  }
}

App.contextType = AuthContext;
export default App;
