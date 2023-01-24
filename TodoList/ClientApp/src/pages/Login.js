/*eslint unicode-bom: ["error", "never"]*/
import React, { useState } from "react";
import axios from "axios";
import './../custom.css'
import { configHeadersToken } from "../utils/configHeadersToken"

function Login() {
    const [username, setUsername] = useState("")
    const handleQuery = (query) => { 
        setUsername(query.target.value);
    }
    const handleSubmit = () => {
        axios.post("http://localhost:5126/Auth", null, { params: { username : username } })
            .then(response => {
                const token = response.data.token;
                localStorage.setItem("token", token);
                configHeadersToken(token);
                window.location.href = '/'
            })
            .catch(err => { alert(err); console.log(err) });
    };
    return (
        <div className="login-form">
            <form
                onSubmit={(event) => {
                    event.preventDefault()
                    handleSubmit(username);
                }}
            >
                <div class="input-group">
                    <span class="input-group-text">Username</span>
                    <input type="text" class="form-control mr-2" id="username" required placeholder="Please enter your username" onChange={handleQuery} value={username} />
                    <button type="submit" class="btn btn-primary">Submit</button>    
                </div>
                <small id="usernameHelp" class="form-text text-muted mt-md">Valid usernames are Batman, Spiderman, Superman, Ironman</small>
            </form>
        </div>   
    );
}

export default Login