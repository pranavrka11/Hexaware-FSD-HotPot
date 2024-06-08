import React, { useEffect, useState } from 'react';
import './Admin.css'; // Importing the CSS file
import axios from 'axios';
import { useSelector } from 'react-redux';

const Admin = () => {
    const authLoggedIn = useSelector((state) => state.siteauth.authLoggedIn);
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

    const [orderHistory, setOrderHistory] = useState([{
        "orderId": 1,
        "orderDate": "2024-02-11T17:53:58.0382584",
        "amount": 500,
        "status": "delivered",
        "customerId": 1,
        "customer": {
            "id": 1,
            "name": "Pranav Karlekar",
        },
        "restaurantId": 1,
        "restaurant": {
            "restaurantId": 1,
        },
        "partnerId": 1,
        "deliveryPartner": {
            "partnerId": 1,
            "name": "Vishal Kumar",
        }
    }]);

    const [paymentHistory, setPaymentHistory] = useState([{
        "paymentId": 1,
        "paymentMode": "online",
        "amount": 500,
        "status": "successful",
        "date": "2024-02-11T17:53:58.0398164",
        "orderId": 1
    }]);

    const [restaurantData, setRestaurantData] = useState({
        "restaurantId": 0,
        "restaurantName": "",
        "phone": "",
        "email": "",
        "cityId": 0,
        "restaurantImage": ""
    });

    const [restaurantAdminData, setRestaurantAdminData] = useState({
        "name": "",
        "restaurantId": 0,
        "userName": "",
        "password": "",
    })

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/Restaurant/GetAllOrders`);
                setOrderHistory(response.data);
                // setOrderStatuses(response.data.map(order => ({ orderId: order.orderId, status: order.status })));

            } catch (error) {
                console.error('Error fetching orders:', error);
            }
        };

        fetchOrders();
    }, []);

    useEffect(() => {
        const fetchPayments = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/Restaurant/GetAllPayments`);
                setPaymentHistory(response.data);
            } catch (error) {
                console.error('Error getting payments:', error);
            }
        };

        fetchPayments();
    }, []);

    const handleInputChangeResto = (e) => {
        const { name, value } = e.target;
        setRestaurantData({ ...restaurantData, [name]: value });
    };

    const handleInputChangeRestoAdmin = (e) => {
        const { name, value } = e.target;
        setRestaurantAdminData({ ...restaurantAdminData, [name]: value });
    };

    const handleRestaurantSubmit = async (e) => {
        e.preventDefault();
        // console.log('Adding restaurant:', restaurantData);
        // console.log('Adding admin', restaurantAdminData);
        // API call to add restaurant will be added here
        //http://localhost:5249/api/api/Restaurant/AddRestaurant
        var requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(restaurantData)
        };
        fetch('http://localhost:5249/api/Restaurant/AddRestaurant', requestOptions)
            .then(r => r.json())
            .then(r => {
                setRestaurantData(r);
                restaurantAdminData.restaurantId = restaurantData.restaurantId;
                var requestOptions = {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(restaurantAdminData)
                };
                fetch('http://localhost:5249/api/Restaurant/Register', requestOptions)
                    .then(r => r.json)
                    .catch(r => console.log(e));
            })
            .catch(e => console.log(e));
    };

    return (
        <>
        {authLoggedIn? (
            <div className="admin-container">
                <br /><br /><br /><br />
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
                                value={restaurantData.restaurantName}
                                onChange={handleInputChangeResto}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>Phone:</label>
                            <input
                                type="tel"
                                name="phone"
                                value={restaurantData.phone}
                                onChange={handleInputChangeResto}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>Email:</label>
                            <input
                                type="email"
                                name="email"
                                value={restaurantData.email}
                                onChange={handleInputChangeResto}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>City ID:</label>
                            <input
                                type="number"
                                name="cityId"
                                value={restaurantData.cityId}
                                onChange={handleInputChangeResto}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>Restaurant Image:</label>
                            <input
                                type="text"
                                name="restaurantImage"
                                value={restaurantData.restaurantImage}
                                onChange={handleInputChangeResto}
                            />
                        </div>
                        <div className="form-group">
                            <label>Owner Name:</label>
                            <input
                                type="text"
                                name="name"
                                value={restaurantAdminData.name}
                                onChange={handleInputChangeRestoAdmin}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>Username:</label>
                            <input
                                type="text"
                                name="userName"
                                value={restaurantAdminData.userName}
                                onChange={handleInputChangeRestoAdmin}
                                required
                            />
                        </div>
                        <div className="form-group">
                            <label>Password:</label>
                            <input
                                type="password"
                                name="password"
                                value={restaurantAdminData.password}
                                onChange={handleInputChangeRestoAdmin}
                                required
                            />
                        </div>
                        <button className="add-restaurant-button" type="submit">Add Restaurant</button>
                    </form>
                )}
                {activeTab === 'orderHistory' && (
                    <div className="order-history">
                        <div className="table-container">
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
                        <div className="table-container">
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
        ): (null)}
        </>
    );
};



export default Admin;