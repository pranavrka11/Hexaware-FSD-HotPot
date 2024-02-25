import React, { useState } from 'react';
import './OwnerProfile.css'; // Importing the CSS file

const OwnerProfile = () => {
  const [activeTab, setActiveTab] = useState('orders');

  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

    // Sample order data
  const orders = [
    {
      id: 1,
      date: '2024-02-24',
      amount: 50.99,
      status: 'Pending',
      customer: 'John Doe',
      deliveryPartner: 'Delivery Express'
    },
    {
      id: 2,
      date: '2024-02-23',
      amount: 35.75,
      status: 'Delivered',
      customer: 'Jane Smith',
      deliveryPartner: 'Swift Delivery'
    },
    // Add more sample orders as needed
  ];
  
  const payments = [
    {
      id: 1,
      mode: 'Credit Card',
      amount: 50.99,
      status: 'Completed',
      date: '2024-02-24',
      orderId: 101
    },
    {
      id: 2,
      mode: 'PayPal',
      amount: 35.75,
      status: 'Pending',
      date: '2024-02-23',
      orderId: 102
    },
    // Add more sample payments as needed
  ];


  return (
    <div className="profile-container">
      <div className="tabs">
        <button
          className={activeTab === 'orders' ? 'active' : ''}
          onClick={() => handleTabChange('orders')}
        >
          View Orders
        </button>
        <button
          className={activeTab === 'payments' ? 'active' : ''}
          onClick={() => handleTabChange('payments')}
        >
          View Payments
        </button>
        <button
          className={activeTab === 'changeStatus' ? 'active' : ''}
          onClick={() => handleTabChange('changeStatus')}
        >
          Change Order Status
        </button>
      </div>

      <div className="tab-content">
      {activeTab === 'orders' && (
          <div className="orders-tab">
            <h2>View Orders</h2>
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Date</th>
                  <th>Amount</th>
                  <th>Status</th>
                  <th>Customer</th>
                  <th>Delivery Partner</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id}>
                    <td>{order.id}</td>
                    <td>{order.date}</td>
                    <td>${order.amount.toFixed(2)}</td>
                    <td>{order.status}</td>
                    <td>{order.customer}</td>
                    <td>{order.deliveryPartner}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
        {activeTab === 'payments' && (
          <div className="payments-tab">
            <h2>View Payments</h2>
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Mode</th>
                  <th>Amount</th>
                  <th>Status</th>
                  <th>Date</th>
                  <th>Order ID</th>
                </tr>
              </thead>
              <tbody>
                {payments.map((payment) => (
                  <tr key={payment.id}>
                    <td>{payment.id}</td>
                    <td>{payment.mode}</td>
                    <td>${payment.amount.toFixed(2)}</td>
                    <td>{payment.status}</td>
                    <td>{payment.date}</td>
                    <td>{payment.orderId}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
        {activeTab === 'changeStatus' && (
          <div className="change-status-tab">
            <h2>Change Order Status</h2>
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Date</th>
                  <th>Amount</th>
                  <th>Status</th>
                  <th>Customer</th>
                  <th>Change Status</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id}>
                    <td>{order.id}</td>
                    <td>{order.date}</td>
                    <td>${order.amount.toFixed(2)}</td>
                    <td>{order.status}</td>
                    <td>{order.customer}</td>
                    <td>
                      <select>
                        <option value="pending">Pending</option>
                        <option value="processing">Processing</option>
                        <option value="completed">Completed</option>
                        <option value="cancelled">Cancelled</option>
                      </select>
                      <button>   <i class='bx bx-edit'></i> Change Status</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default OwnerProfile;
