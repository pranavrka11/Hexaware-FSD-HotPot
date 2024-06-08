import {  NavLink, Outlet, useNavigate } from "react-router-dom";
import './StaticComponentStyles.css'
import { useEffect, useState } from "react";
import axios from "axios";
import { useDispatch, useSelector } from "react-redux";
import { logout } from "./Store/auth-slice";
import { changeCity } from "./Store/city-slice";
import { changeCategory } from "./Store/category-slice";

export default function NavbarLogin() {
    const customerId = sessionStorage.getItem("UserId");
    const navigate=useNavigate();
    const dispatch=useDispatch();

    const logoutUser=()=>{
        sessionStorage.clear();
        dispatch(logout());
        navigate('/');
        // window.location.reload();
    }

    // const [selectedCity, setSelectedCity] = useState(() => {
    //     return sessionStorage.getItem('selectedCity') || 'pune';
    // });
    const [cities, setCities] = useState([]);

    const selectedCity=useSelector((state)=>state.city.selectedCity);

    //fetching city names
    useEffect(() => {
        axios.get('http://localhost:5249/api/Customer/GetAllCities')
            .then(function (response) {
                setCities(response.data);
            })
            .catch(function (error) {
                console.log(error);
            })
    }, []);

    const handleCityChange = (e) => {
        const cityName = e.target.value;
        dispatch(changeCity(cityName));
        // setSelectedCity(cityName);
        // sessionStorage.setItem('selectedCity', cityName);
        // window.location.reload();
    };

    const ChangeCategory=()=>{
        dispatch(changeCategory('all'));
    };

    return (
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
                                <NavLink className="nav-link mx-lg-2" aria-current="page" to='/'>Home</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/restaurants' onClick={ChangeCategory}>Restaurants</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/about'>About</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to='/contact'>Contact</NavLink>
                            </li>
                        </ul>
                    </div>
                </div>
                {/* <Link className="login-button" to='/login'>Profile</Link> */}
                <div className='city'>
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
                </div>
                <div className="login-button-2">
                    <NavLink to={`/customer-profile/${customerId}`}><i className="bi bi-person login-icons"></i></NavLink>
                    <NavLink to={`/carts/${customerId}`}><i className="bi bi-cart3 login-icons"></i></NavLink>
                </div>
                <button onClick={logoutUser} className="logout-button" href="#">Logout</button>
                <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
                    <span className="navbar-toggler-icon"></span>
                </button>
            </div>
            <Outlet />
        </nav>
    );
}
