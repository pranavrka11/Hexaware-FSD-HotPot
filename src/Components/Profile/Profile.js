import React from 'react';
import './Profile.css'; // Importing the CSS file

const Profile = () => {
  // Static user details
  const userDetails = {
    name: 'John Doe',
    email: 'john@example.com',
    phone: '123-456-7890',
    userName: 'johndoe'
  };

  // Static user address
  const userAddress = {
    houseNumber: '123',
    city: 'Anytown',
    buildingName: 'The Building',
    locality: 'Example Locality',
    landMark: 'Near the Park'
  };

  // Static order history
  const orderHistory = ['Order 1', 'Order 2', 'Order 3'];

  return (
    <div className="page-container">
      <div className="card">
        <div className="imgbox">
          {/* You can include user avatar here if available */}
          <img
            src="UserProfile.jpg"
            alt="User Avatar"
            style={{ maxWidth: '100%', maxHeight: '130%' }} // Added maxWidth and maxHeight properties
          />
        </div>
        <div className="content">
          <h2>{userDetails.name}</h2>
          <p>Email: {userDetails.email}</p>
          <p>Phone: {userDetails.phone}</p>
          <p>Username: {userDetails.userName}</p>
        </div>
      </div>

      <div className="card">
        <div className="imgbox">
          {/* You can include address image here if available */}
          <img
            src="addressLogo.jpg"
            alt="Address Image"
            style={{ maxWidth: '100%', maxHeight: '130%' }} // Added maxWidth and maxHeight properties
          />
        </div>
        <div className="content">
          <h2>Address</h2>
          <p>House Number: {userAddress.houseNumber}</p>
          <p>City: {userAddress.city}</p>
          <p>Building Name: {userAddress.buildingName}</p>
          <p>Locality: {userAddress.locality}</p>
          <p>Landmark: {userAddress.landMark}</p>
        </div>
      </div>

      <div className="card">
        <div className="imgbox">
          {/* You can include order image here if available */}
          <img
            src="orderHistory.png"
            alt="Order Image"
            style={{ maxWidth: '100%', maxHeight: '130%' }} // Added maxWidth and maxHeight properties

          />
        </div>
        <div className="content">
          <h2>Order History</h2>
          <ul>
            {orderHistory.map((order, index) => (
              <li key={index}>{order}</li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Profile;
