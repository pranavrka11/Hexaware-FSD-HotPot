import { NavLink } from "react-router-dom";


export default function Footer(){
    return(
        <footer className="myfooter">
            <div className="footer-brand">
                <h1><a href="index.html">HotPot</a></h1>
            </div>
            <div className="footer-container container-fluid">
                <div className="social-icons">
                    <a href=""><i className="bi bi-facebook"></i></a>
                    <a href=""><i className="bi bi-messenger"></i></a>
                    <a href=""><i className="bi bi-instagram"></i></a>
                    <a href=""><i className="bi bi-twitter-x"></i></a>
                </div>
                <div className="footer-nav">
                    <ul>
                        <li><NavLink to='/'>Home</NavLink></li>
                        <li><NavLink to='/restaurants'>Restaurants</NavLink></li>
                        <li><NavLink to='/about'>About</NavLink></li>
                        <li><NavLink to='/contact'>Contact</NavLink></li>
                    </ul>
                </div>
            </div>
            <div className="footer-copyright">
                <p>Copyright &copy; 2024; HotPot Pvt. Ltd </p>
            </div>
        </footer>
    )
}