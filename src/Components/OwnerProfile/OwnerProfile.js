import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './OwnerProfile.css'; // Importing the CSS file

const OwnerProfile = () => {
  const [activeTab, setActiveTab] = useState('orders');
  const [orders, setOrders] = useState([]);
  const [payments, setPaymnets] = useState([]);
  const [orderStatuses, setOrderStatuses] = useState([]);
  const [newMenu, setNewMenu] = useState({
    name: '',
    type: 'Veg',
    price: '',
    description: '',
    cuisine: 'Main Course',
    cookingTime: '',
    tasteInfo: '',
    menuImage: '',
    nutritionId: '',
    restaurantId: '1', // Set restaurantId statically for now
  });
  const [menus, setMenus] = useState([]);
  const [selectedMenuId, setSelectedMenuId] = useState('');
  const [error, setError] = useState(null);


  useEffect(() => {
    const fetchMenus = async () => {
      try {
        const response = await axios.get('http://localhost:5272/api/Customer/menu');
        setMenus(response.data);
      } catch (error) {
        console.error('Error fetching menus:', error);
        setError('Error fetching menus. Please try again.');
      }
    };
    fetchMenus();
  }, []);

  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

  const handleMenuSubmit = async (e) => {
    e.preventDefault();
    try {
      // Call API to add menu
      const response = await axios.post('http://localhost:5272/api/Customer/menu/add', newMenu);
      console.log('Menu added:', response.data);
      // Reset form fields
      setNewMenu({
        name: '',
        type: 'Veg',
        price: '',
        description: '',
        cuisine: 'Main Course',
        cookingTime: '',
        tasteInfo: '',
        menuImage: '',
        nutritionId: '',
        restaurantId: '123', // Set restaurantId statically for now
      });
      setError(null); // Clear any previous errors
    } catch (error) {
      console.error('Error adding menu:', error);
      setError('Error adding menu. Please try again.'); // Set error message
    }
  };

  const handleDeleteMenu = async () => {
    try {
      await axios.delete(`http://localhost:5272/api/Customer/menu/delete?id=${selectedMenuId}`);
      // After successfully deleting the menu, remove it from the dropdown list
      setMenus(menus.filter(menu => menu.menuId !== selectedMenuId));
      setSelectedMenuId('');
      setError(null); // Clear any previous errors
    } catch (error) {
      console.error('Error deleting menu:', error);
      setError('Error deleting menu. Please try again.');
    }
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
        <button
          className={activeTab === 'addMenu' ? 'active' : ''}
          onClick={() => handleTabChange('addMenu')}
        >
          Add Menu
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
                    <td>Rs. {payment.amount.toFixed(2)}</td>
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
        {activeTab === 'addMenu' && (
        <div className="add-menu-tab">
          <h2>Add Menu</h2>
          <form onSubmit={handleMenuSubmit}>
          <div className="form-group">
                <label>Name:</label>
                <input
                  type="text"
                  value={newMenu.name}
                  onChange={(e) => setNewMenu({ ...newMenu, name: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Type:</label>
                <select
                  value={newMenu.type}
                  onChange={(e) => setNewMenu({ ...newMenu, type: e.target.value })}
                >
                  <option value="Veg">Veg</option>
                  <option value="Non-Veg">Non-Veg</option>
                </select>
              </div>
              <div className="form-group">
                <label>Price:</label>
                <input
                  type="number"
                  value={newMenu.price}
                  onChange={(e) => setNewMenu({ ...newMenu, price: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Description:</label>
                <input
                  type="text"
                  value={newMenu.description}
                  onChange={(e) => setNewMenu({ ...newMenu, description: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Cuisine:</label>
                <select
                  value={newMenu.cuisine}
                  onChange={(e) => setNewMenu({ ...newMenu, cuisine: e.target.value })}
                >
                  <option value="Main Course">Main Course</option>
                  <option value="Break Fast">Break Fast</option>
                  <option value="Icecream">Icecream</option>
                </select>
              </div>
              <div className="form-group">
                <label>Cooking Time:</label>
                <input
                  type="text"
                  value={newMenu.cookingTime}
                  onChange={(e) => setNewMenu({ ...newMenu, cookingTime: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Taste Info:</label>
                <input
                  type="text"
                  value={newMenu.tasteInfo}
                  onChange={(e) => setNewMenu({ ...newMenu, tasteInfo: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Menu Image:</label>
                <input
                  type="text"
                  value={newMenu.menuImage}
                  onChange={(e) => setNewMenu({ ...newMenu, menuImage: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Nutrition ID:</label>
                <input
                  type="number"
                  value={newMenu.nutritionId}
                  onChange={(e) => setNewMenu({ ...newMenu, nutritionId: e.target.value })}
                />
              </div>
             
              {error && <div className="error">{error}</div>}
              <button className='addMenu-button' type="submit">Add Menu</button>
            </form>
            
            <div className="form-group">
            <label>Select Menu to Delete:</label>
            <select
              value={selectedMenuId}
              onChange={(e) => setSelectedMenuId(e.target.value)}
            >
              <option value="">Select Menu</option>
              {menus.map(menu => (
                <option key={menu.menuId} value={menu.menuId}>{menu.name}</option>
              ))}
            </select>
          </div>
          {error && <div className="error">{error}</div>}
          <button className='delete-menu-button' onClick={handleDeleteMenu}>Delete Menu</button>
        </div>
      )}
      </div>
    </div>
  );
};

export default OwnerProfile;
