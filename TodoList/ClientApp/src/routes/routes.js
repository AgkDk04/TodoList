/*eslint unicode-bom: ["error", "never"]*/
import React from "react";
import { Redirect, Switch, Route, Router } from "react-router-dom";
import RouteCheckAuthToken from "./routeCheckAuthToken"
import { history } from '../utils/history';
import Tasks from "../pages/Tasks"
import LoginPage from "../pages/Login"

function Routes() {
    return (
        <Router history={history}>
            <Switch>
                <RouteCheckAuthToken
                    exact
                    path="/"
                    component={Tasks}
                />
                <Route
                    path="/login"
                    component={LoginPage}
                />
                <Redirect to="/" />
            </Switch>
        </Router>
    );
}
 
export default Routes
