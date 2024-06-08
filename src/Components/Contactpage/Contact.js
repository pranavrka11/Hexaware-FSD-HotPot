import React from 'react';
import './contact.css'; // Importing the CSS file

const Contact= () => {
  return (
    <div className="contact-us-container">
      <h1 className="contact-heading">Contact Us</h1>
      <div className="contact-info">
        <p className="info-item">
          <strong>Email:</strong> example@example.com
        </p>
        <p className="info-item">
          <strong>Phone:</strong> 123-456-7890
        </p>
        <p className="info-item">
          <strong>Address:</strong> 123 Main Street, City, Country
        </p>
      </div>
      <div className="social-icons">
        <a href="#" className="social-icon-link"><i className='bx bxl-facebook-square'></i></a>
        <a href="#" className="social-icon-link"><i className='bx bxl-twitter'></i></a>
        <a href="#" className="social-icon-link"><i className='bx bxl-instagram'></i></a>
        <a href="#" className="social-icon-link"><i className='bx bxl-linkedin'></i></a>
      </div>
    </div>
  );
};

export default Contact;