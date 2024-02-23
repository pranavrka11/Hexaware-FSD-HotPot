import React from 'react';
import '../RestaurantOwner/RestaurantOwner.css';


function RegistrationHeader() {
  return (
    <header className="registration-header">
      <div className="container">
        <div className="logo">
          <a href="index.html">HotPot</a>
        </div>
        <nav>
          <ul>
            <li className="link">
              <a href="index.html">Home</a>
            </li>
            <li className="link">
              <a href="#">Home</a>
            </li>
            <li className="link">
              <a href="Contact.html">Contact us</a>
            </li>
            <li className="link">
              <a href="Registration.html">Registration</a>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
}

export default RegistrationHeader;
