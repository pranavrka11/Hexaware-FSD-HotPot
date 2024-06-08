import { useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";

function Login(){
    var [username, setUsername]=useState("");
    var [password, setPassword]=useState("");
    var [loggedIn, setLoggedIn]=useState(false);

    var user={};
    var navigate=useNavigate();

    var Login=(e)=>{
        e.preventDefault();
        user.userId=0;
        user.userName=username;
        user.password=password;
        user.role="";
        user.token="";

        console.log(user);

        var requestOptions={
            method:'POST',
            headers: {'Content-Type':'application/json'},
            body: JSON.stringify(user)
        };

        fetch("http://localhost:5249/api/Customer/LogIn", requestOptions)
        .then(r=>r.json())
        .then(r=>{
            sessionStorage.setItem("Token", r.token);
            sessionStorage.setItem("UserName", r.userName);
            sessionStorage.setItem("UserId", r.userId);
            setLoggedIn(true);
            navigate('/');
        })
        .catch(e=>{
            console.log(e);
            setLoggedIn(false);
        })
    }
    return(
        <div className="container">
            <br /><br /><br /><br /><br />
            {/* {loggedIn==true?<h2 className="alert alert-success">Login successful!. Welcome {username}</h2>:null} */}
            <form>
            <label htmlFor="exampleFormControlInput1" className="form-label">UserName</label>
            <input type="text" className="form-control" value={username} onChange={(e)=>setUsername(e.target.value)}/>
            <label htmlFor="exampleFormControlInput1" className="form-label">Password</label>
            <input type="password" className="form-control" value={password} onChange={(e)=>setPassword(e.target.value)}/>
            <button className="btn btn-success" onClick={Login}>Submit</button>
            <button className="btn btn-danger">Cancel</button>
            </form>
        </div>
    )
}

export default Login;