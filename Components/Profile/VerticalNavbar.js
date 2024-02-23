// VerticalNavbar.js

import React from 'react';
import './Profile.css'; // Import the CSS file

const VerticalNavbar = () => {
  return (
    <div className="vertical-navbar">
      <ul>
        <li>User</li>
        <li>Address</li>
        <li>Order History</li>
        <li>Payment History</li>
      </ul>
    </div>
  );
};

export default VerticalNavbar;
