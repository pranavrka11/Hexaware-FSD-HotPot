import React from 'react';
import '../RestaurantOwner/RestaurantOwner.css';


function RegistrationFooter() {
  return (
    <footer className="myfooter">
      <div className="footer-brand">
        <h1><a href="">HotPot</a></h1>
      </div> 
      <div className="footer-container container-fliud">
        <div className="social-icons">
          <a href=""><i className='bx bxl-facebook'></i></a>
          <a href=""><i className='bx bxl-twitter'></i></a>
          <a href=""><i className="bx bxl-youtube"></i></a>
          <a href=""><i className="bx bxl-instagram"></i></a>
        </div>
        <div className="footer-nav">
          <ul>
            <li><a href="">Home</a></li>
            <li><a href="">Restaurants</a></li>
            <li><a href="">About</a></li>
            <li><a href="">Contact</a></li>
          </ul>
        </div>
      </div>
      <div className="footer-copyright">
        <p>Copyright &copy; 2024; HotPot Pvt. Ltd </p>
      </div>
    </footer>
  );
}

export default RegistrationFooter;
