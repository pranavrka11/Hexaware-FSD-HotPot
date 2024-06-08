import { useSelector } from 'react-redux';
import { useAuth } from '../Context/AuthContext'
import NavbarLogin from './NavbarLogin';
import NavbarNormal from './NavbarNormal';
import './StaticComponentStyles.css'
import { NavLink } from 'react-router-dom'
import NavbarAuthority from './NavbarAuthority';

export default function Navbar() {
    // const { isLoggedIn, handleLogin } = useAuth();
    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn);
    const authLoggedIn = useSelector((state) => state.siteauth.authLoggedIn);
    return (
        <>
            {!authLoggedIn ? (
                isLoggedIn ? <NavbarLogin /> : <NavbarNormal />
            ) : (
                <NavbarAuthority />
            )}

        </>
    );
}