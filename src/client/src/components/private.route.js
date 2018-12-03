import React, { Component } from "react";
import { Route, Redirect } from "react-router-dom";

import { AuthContext } from "./contexts/auth-context";

class PrivateRoute extends Component {
  state = { auth: props.auth };
  

  render() {
    return (
      <Route
        {...this.props}
        render={props =>
          this.state.auth.getIsAuthenticated() ? (
            <Component {...props} />
          ) : (
            <Redirect
              to={{
                pathname: "/login",
                state: { from: props.location }
              }}
            />
          )
        }
      />
    );
  }
}

//export default connect(mapStateToProps)(PrivateRoute);
export default PrivateRoute;

// import React from "react";
// import { Route, Redirect } from "react-router-dom";
// //import { connect } from 'react-redux';
// //import PropTypes from 'prop-types';

// const PrivateRoute = ({ component: Component, auth, ...rest }) => {
//   return (
//     <Route
//       {...rest}
//       render={props =>
//         auth.getIsAuthenticated() ? (
//           <Component {...props} />
//         ) : (
//           <Redirect to="/login" />
//         )
//       }
//     />
//   );
// };

// // PrivateRoute.propTypes = {
// //   auth: PropTypes.object.isRequired
// // };

// // const mapStateToProps = state => ({
// //   auth: state.auth
// // });

// //export default connect(mapStateToProps)(PrivateRoute);
// export default PrivateRoute;
