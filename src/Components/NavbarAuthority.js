import { Link, NavLink, Outlet, useNavigate } from "react-router-dom";
import './StaticComponentStyles.css'
import { useEffect, useState } from "react";
import axios from "axios";
import { useDispatch, useSelector } from "react-redux";
import { logout } from "./Store/auth-slice";
import { changeCity } from "./Store/city-slice";
import { siteAuthLogout } from "./Store/siteauthority-slice";

export default function NavbarAuthority() {
    const navigate=useNavigate();
    const dispatch=useDispatch();

    const logoutUser=()=>{
        sessionStorage.clear();
        dispatch(siteAuthLogout());
        navigate('/');
        // window.location.reload();
    }

    return (
        <nav className="navbar navbar-expand-lg fixed-top " style={{ backgroundColor: '#EBEBEB' }}>
            <div className="container-fluid auth-navbar">
            <NavLink className="navbar-brand me-auto" to='/'>HotPot</NavLink>

                <div className="offcanvas offcanvas-end" tabIndex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">
                    <div className="offcanvas-header">
                        <h5 className="offcanvas-title" id="offcanvasNavbarLabel">HotPot</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
                    </div>
                    {/* <div className="offcanvas-body">
                        <ul className="navbar-nav justify-content-center flex-grow-1 pe-3">
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" aria-current="page" to='/'>Home</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/restaurants'>Restaurants</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/about'>About</NavLink>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link mx-lg-2" href="#">Contact</a>
                            </li>
                        </ul>
                    </div> */}
                </div>
                {/* <Link className="login-button" to='/login'>Profile</Link> */}
                {/* <div className='city'>
                    <div className='location-icon'>
                        <i className="bi bi-geo-alt"></i>
                    </div>
                    <div className='city-dropdown'>
                        <select className='city-select' value={selectedCity} onChange={handleCityChange}>
                            {Array.isArray(cities) && cities.map((city, index) => (
                                <option key={index} value={city.name}>{city.name}</option>
                            ))}
                        </select>
                    </div>
                </div> */}
                {/* <div className="login-button-2">
                    <NavLink to={`/customer-profile/${customerId}`}><i className="bi bi-person login-icons"></i></NavLink>
                    <NavLink to={`/carts/${customerId}`}><i className="bi bi-cart3 login-icons"></i></NavLink>
                </div> */}
                <button onClick={logoutUser} className="logout-button" href="#">Logout</button>
                <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
                    <span className="navbar-toggler-icon"></span>
                </button>
            </div>
            <Outlet />
        </nav>
    );
}
