import React, { useState } from 'react';
import './Admin.css'; // Importing the CSS file

const Admin = () => {
  const [activeTab, setActiveTab] = useState('addRestaurant');
  const [formData, setFormData] = useState({
    restaurantName: 'Example Restaurant',
    phone: '123-456-7890',
    email: 'example@example.com',
    cityId: '1',
    restaurantImage: 'https://via.placeholder.com/150',
    name: '',
    userName: '',
    password: ''
  });

  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

  const [orderHistory, setOrderHistory] = useState([
    { orderId: 1, orderDate: '2024-02-09', amount: 50.0, status: 'Delivered', customerId: 1, restaurantId: 1, partnerId: 1 },
    { orderId: 2, orderDate: '2024-02-08', amount: 45.0, status: 'In Progress', customerId: 2, restaurantId: 2, partnerId: 2 },
    { orderId: 3, orderDate: '2024-02-07', amount: 55.0, status: 'Cancelled', customerId: 3, restaurantId: 3, partnerId: 3 }
  ]);

  const [paymentHistory, setPaymentHistory] = useState([
    { paymentId: 1, paymentMode: 'Credit Card', amount: 50.0, status: 'Success', date: '2024-02-09', orderId: 1 },
    { paymentId: 2, paymentMode: 'Cash', amount: 45.0, status: 'Success', date: '2024-02-08', orderId: 2 },
    { paymentId: 3, paymentMode: 'Debit Card', amount: 55.0, status: 'Pending', date: '2024-02-07', orderId: 3 }
  ]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleRestaurantSubmit = async (e) => {
    e.preventDefault();
    console.log('Adding restaurant:', formData);
    // API call to add restaurant will be added here
  };

  return (
<div className="admin-container">
      <div className="tabs">
        <button
          className={activeTab === 'addRestaurant' ? 'active-tab' : ''}
          onClick={() => handleTabChange('addRestaurant')}
        >
          Add Restaurant
        </button>
        <button
          className={activeTab === 'orderHistory' ? 'active-tab' : ''}
          onClick={() => handleTabChange('orderHistory')}
        >
          Order History
        </button>
        <button
          className={activeTab === 'paymentHistory' ? 'active-tab' : ''}
          onClick={() => handleTabChange('paymentHistory')}
        >
          Payment History
        </button>
      </div>
      {activeTab === 'addRestaurant' && (
        <form className="add-restaurant-form" onSubmit={handleRestaurantSubmit}>
          <div className="form-group">
            <label>Restaurant Name:</label>
            <input
              type="text"
              name="restaurantName"
              value={formData.restaurantName}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Phone:</label>
            <input
              type="tel"
              name="phone"
              value={formData.phone}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Email:</label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>City ID:</label>
            <input
              type="number"
              name="cityId"
              value={formData.cityId}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Restaurant Image:</label>
            <input
              type="text"
              name="restaurantImage"
              value={formData.restaurantImage}
              onChange={handleInputChange}
            />
          </div>
          <div className="form-group">
            <label>Name:</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Username:</label>
            <input
              type="text"
              name="userName"
              value={formData.userName}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Password:</label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              required
            />
          </div>
          <button className="add-restaurant-button" type="submit">Add Restaurant</button>
        </form>
      )}
      {activeTab === 'orderHistory' && (
        <div className="order-history">
          <div className = "table-container">
            <h2>Order History</h2>
            <table>
              <thead>
                <tr>
                  <th>Order ID</th>
                  <th>Order Date</th>
                  <th>Amount</th>
                  <th>Status</th>
                  <th>Customer ID</th>
                  <th>Restaurant ID</th>
                  <th>Partner ID</th>
                </tr>
              </thead>
              <tbody>
                {orderHistory.map(order => (
                  <tr key={order.orderId}>
                    <td>{order.orderId}</td>
                    <td>{order.orderDate}</td>
                    <td>{order.amount}</td>
                    <td>{order.status}</td>
                    <td>{order.customerId}</td>
                    <td>{order.restaurantId}</td>
                    <td>{order.partnerId}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
      {activeTab === 'paymentHistory' && (
        <div className="payment-history">
          <div className = "table-container">
            <h2>Payment History</h2>
            <table>
              <thead>
                <tr>
                  <th>Payment ID</th>
                  <th>Payment Mode</th>
                  <th>Amount</th>
                  <th>Status</th>
                  <th>Date</th>
                  <th>Order ID</th>
                </tr>
              </thead>
              <tbody>
                {paymentHistory.map(payment => (
                  <tr key={payment.paymentId}>
                    <td>{payment.paymentId}</td>
                    <td>{payment.paymentMode}</td>
                    <td>{payment.amount}</td>
                    <td>{payment.status}</td>
                    <td>{payment.date}</td>
                    <td>{payment.orderId}</td>
                  </tr>
                ))}
              </tbody>
            </table>
           </div>
        </div>
      )}
    </div>
  );
};


    
export default Admin;
