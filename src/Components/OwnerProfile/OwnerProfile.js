import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './OwnerProfile.css'; // Importing the CSS file

const OwnerProfile = () => {
  const [activeTab, setActiveTab] = useState('orders');
  const [orders, setOrders] = useState([]);
  const [payments, setPaymnets] = useState([]);
  const [orderStatuses, setOrderStatuses] = useState([]);


  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

    useEffect(() => {
      const fetchOrders = async () => {
        try {
          const response = await axios.get('http://localhost:5272/api/Customer/orders');
          setOrders(response.data);
          setOrderStatuses(response.data.map(order => ({ orderId: order.orderId, status: order.status })));

        } catch (error) {
          console.error('Error fetching orders:', error);
      }
    };
    
    fetchOrders();
  }, []);

  useEffect(() => {
    const fetchPayments = async  () => {
      try {
        const response = await axios.get('http://localhost:5272/api/Customer/Payments');
        setPaymnets(response.data);
      } catch (error) {
        console.error('Error getting payments:', error);
      }
    };

    fetchPayments();
  }, []);

  const handleChangestatus = async (orderId, newStatus) => {
    try {
      await axios.put(`http://localhost:5272/api/Customer/${orderId}/status/${newStatus}`);
      const updatedOrders = orders.map(order => {
        if (order.orderId === orderId) {
          return { ...order, status: newStatus };
        }
        return order;
      });
      setOrders(updatedOrders);
      // Update the status for the selected order in orderStatuses
      setOrderStatuses(prevStatuses => prevStatuses.map(status => {
        if (status.orderId === orderId) {
          return { ...status, status: newStatus };
        }
        return status;
      }));
    } catch (error) {
      console.error('Error changing order status:', error);
    }
  };


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
                  <tr key={order.orderId}>
                    <td>{order.orderId}</td>
                    <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                    <td>${order.amount.toFixed(2)}</td>
                    <td>{order.status}</td>
                    <td>{order.customer.name}</td>
                    <td>{order.deliveryPartner.name}</td>
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
                  <tr key={payment.paymentId}>
                    <td>{payment.paymentId}</td>
                    <td>{payment.paymentMode}</td>
                    <td>${payment.amount.toFixed(2)}</td>
                    <td>{payment.status}</td>
                    <td>{new Date(payment.date).toLocaleDateString()}</td>
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
                {orders.map((order, index) => (
                  <tr key={order.orderId}>
                    <td>{order.orderId}</td>
                    <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                    <td>${order.amount.toFixed(2)}</td>
                    <td>{order.status}</td>
                    <td>{order.customer.name}</td>
                    <td>
                    <select value={orderStatuses[index].status} onChange={(e) => handleChangestatus(order.orderId, e.target.value)}>
                        <option value="pending">Pending</option>
                        <option value="processing">Processing</option>
                        <option value="completed">Completed</option>
                        <option value="cancelled">Cancelled</option>
                      </select>
                      <button onClick={() => handleChangestatus(order.orderId, orderStatuses[index].status)}>
                        <i className="bx bx-edit"></i> Change Status
                      </button>
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
