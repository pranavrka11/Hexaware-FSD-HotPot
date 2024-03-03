import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Confirmation.css'; // Import your CSS file

const Confirmation = () => {
  const [orders, setOrders] = useState([]);
  const [address, setAddress] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const ordersResponse = await axios.get('http://localhost:5272/api/Customer/orders/1');
        const addressResponse = await axios.get('http://localhost:5272/api/Customer/address/1');
        setOrders(ordersResponse.data);
        setAddress(addressResponse.data);
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  
  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="confirmation-container">
     {orders.map(order => (
        <div key={order.orderId} className="card order-details">
          <h2>Order Details</h2>
          <p>Order ID: {order.orderId}</p>
          <p>Order Date: {new Date(order.orderDate).toLocaleDateString()}</p>
          <p>Amount: ${order.amount}</p>
          <p>Status: {order.status}</p>
          <p>Customer: {order.customer.name}</p>
          <p>Restaurant: {order.restaurant.restaurantName}</p>
          <p>Delivery Partner: {order.deliveryPartner.name}</p>
          {/* You can add more details as needed */}
        </div>
      ))}

      {address && (
        <div className="card shipping-detail">
          <h2>Shipping Detail</h2>
          <p>House Number: {address.houseNumber}</p>
          <p>Building Name: {address.buildingName}</p>
          <p>Locality: {address.locality}</p>
          <p>City: {address.city.name}</p>
          <p>Landmark: {address.landMark}</p>
        </div>
      )}

      <div className="thank-you-note">
        <h2>Thank You for Ordering using HotPot !!</h2>
      </div>
    </div>
  );
};

export default Confirmation;
