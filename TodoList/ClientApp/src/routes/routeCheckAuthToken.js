/*eslint unicode-bom: ["error", "never"]*/
import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const RouteCheckAuthToken = ({ component: Component, ...rest }) => {
    function authTokenExists() {
        let flag = false;

        if (localStorage.getItem("token")) {
            flag = true; 
        } else {
            flag = false
        }
        return flag
    }

    return (
        <Route {...rest}
            render = { props => (
                authTokenExists()
                    ? <Component {...props} />
                    : <Redirect to={{ pathname: '/login' }} />
            )}
        />
    );
};

export default RouteCheckAuthToken;