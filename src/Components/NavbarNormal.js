import { useEffect, useState } from 'react'
import './StaticComponentStyles.css'
import { NavLink, useLocation, useNavigate } from 'react-router-dom'
import axios from 'axios';
import { useDispatch, useSelector } from 'react-redux';
import { changeCity } from './Store/city-slice';
import { changeCategory } from './Store/category-slice';

export default function NavbarNormal() {
    // const city = 'pune'
    // const [selectedCity, setSelectedCity] = useState(() => {
    //     return sessionStorage.getItem('selectedCity') || 'pune';
    // });
    const [cities, setCities] = useState([]);

    const selectedCity=useSelector((state)=>state.city.selectedCity);

    const dispatch=useDispatch();
    const navigate=useNavigate();
    const location=useLocation();

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
        // setSelectedCity(cityName);
        dispatch(changeCity(cityName));
        //sessionStorage.setItem('selectedCity', cityName);
        // window.location.reload();
    }

    // useEffect(() => {
    //     const storedCity = sessionStorage.getItem('selectedCity');
    //     if (storedCity && storedCity !== selectedCity) {
    //         setSelectedCity(storedCity);
    //     }
    // }, []);

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
                                <NavLink className="nav-link mx-lg-2" to='/'>Home</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link mx-lg-2" to={`/restaurants`} onClick={ChangeCategory}>Restaurants</NavLink>
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
                {/* <div class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                        City
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">Action</a></li>
                        <li><a class="dropdown-item" href="#">Another action</a></li>
                        <li><hr class="dropdown-divider" /></li>
                        <li><a class="dropdown-item" href="#">Something else here</a></li>
                    </ul>
                </div> */}
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
                <NavLink className="login-button" to='/customer-login'>LogIn</NavLink>
                <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
                    <span className="navbar-toggler-icon"></span>
                </button>
            </div>
        </nav>
    )
}