import { useState } from "react";
import axios from "axios";
import './Restaurant.css'
import { NavLink } from "react-router-dom";

function RestaurantNav() {
    var [restaurants, setRestaurants] = useState([{
        "id": 0,
        "restaurantName": "",
        "email": "",
        "phone": ""
    }]);

    var data = axios.get("http://localhost:5249/api/Customer/GetRestaurantsByCity?city=pune")
        .then(function (response) {
            setRestaurants(response.data);
        })
        .catch(function (error) {
            console.log(error);
        })

    // var getData=()=>fetch("http://localhost:5249/api/Customer/GetRestaurantsByCity?city=pune")
    // .then(r=>r.json())
    // .then(r=>{
    //     setRestaurants(r);
    // })
    // .catch(e=>console.log(e));

    return (
        // <nav className="navbar navbar-expand-lg fixed-top" style={{ backgroundColor: '#EBEBEB' }}>
        //     <div className="container-fluid">
        //         <div className="navbar-brand-name">
        //             <a className="navbar-brand me-auto" href="index.html">HotPot</a>
        //         </div>

        //         <div className="offcanvas offcanvas-end" tabIndex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">
        //             <div className="offcanvas-header">
        //                 <h5 className="offcanvas-title" id="offcanvasNavbarLabel">HotPot</h5>
        //                 <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        //             </div>
        //             <div className="offcanvas-body">
        //                 <ul className="navbar-nav justify-content-center flex-grow-1 pe-3">
        //                     <li className="nav-item">
        //                         <a className="nav-link mx-lg-2 active" aria-current="page" href="index.html">Home</a>
        //                     </li>
        //                     <li className="nav-item">
        //                         <a className="nav-link mx-lg-2" href="restaurants.html">Restaurants</a>
        //                     </li>
        //                     <li className="nav-item">
        //                         <a className="nav-link mx-lg-2" href="about.html">About</a>
        //                     </li>
        //                     <li className="nav-item">
        //                         <a className="nav-link mx-lg-2" href="#">Contact</a>
        //                     </li>
        //                 </ul>
        //             </div>
        //         </div>
        //         <a className="login-button" href="#">LogIn</a>
        //         <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
        //             <span className="navbar-toggler-icon"></span>
        //         </button>
        //     </div>
        // </nav>

        <nav className="navbar navbar-expand-lg fixed-top" style={{ backgroundColor: '#EBEBEB' }}>
            <div className="container-fluid">
                <NavLink className="navbar-brand me-auto" to='/'>HotPot</NavLink>

                <div className="offcanvas offcanvas-end" tabIndex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">
                    <div className="offcanvas-header">
                        <h5 className="offcanvas-title" id="offcanvasNavbarLabel">HotPot</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                    </div>
                    <div className="offcanvas-body">
                        <ul className="navbar-nav justify-content-center flex-grow-1 pe-3">
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/'>Home</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/restaurants'>Restaurants</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" aria-current="page" to='/about'>About</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/contact'>Contact</NavLink>
                            </li>
                        </ul>
                    </div>
                </div>
                <NavLink className="login-button" to='/customer-login'>LogIn</NavLink>
                <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
                    <span className="navbar-toggler-icon"></span>
                </button>
            </div>
        </nav>
    )
}

export default RestaurantNav;