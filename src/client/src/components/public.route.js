import React from "react";
import { Route, Redirect } from "react-router-dom";
//import { connect } from 'react-redux';
//import PropTypes from 'prop-types';

const PublicRoute = ({ component: Component, auth, ...rest }) => {
  return (
    <Route
      {...rest}
      render={props =>
        auth.getIsAuthenticated() ? (
          <Redirect to="/" />
        ) : (
          <Component {...props} auth={auth} />
        )
      }
    />
  );
};

// PrivateRoute.propTypes = {
//   auth: PropTypes.object.isRequired
// };

// const mapStateToProps = state => ({
//   auth: state.auth
// });

//export default connect(mapStateToProps)(PrivateRoute);
export default PublicRoute;
