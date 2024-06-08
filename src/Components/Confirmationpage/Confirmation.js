import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Confirmation.css'; // Import your CSS file
import { useNavigate } from 'react-router-dom';

const Confirmation = () => {
    const [orders, setOrders] = useState([{
        "orderId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "restaurantName": "",
        "restaurantImage": "",
        "menuName": [
            {
                "manuItemName": "",
                "quantity": 0
            }
        ],
        "price": 0,
        "status": "",
        "orderDate": "",
        "partnerId": 0
    }]);
    const [address, setAddress] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const customerId = sessionStorage.getItem('UserId');
    const customerToken = sessionStorage.getItem('Token');
    const navigate=useNavigate();

    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const ordersResponse = await axios.get(`http://localhost:5249/api/Customer/ViewOrderHistoryForCustomer?customerId=${customerId}`, requestOptions);
                const addressResponse = await axios.get(`http://localhost:5249/api/Customer/address/${customerId}`, requestOptions);
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

    const highestOrder = orders.reduce((prev, current) =>
        prev.orderId > current.orderId ? prev : current
    );

    const toProfile=()=>{
        navigate(`/customer-profile/${customerId}`);
    }

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="confirmation-container">
            <br/><br/><br/><br/>
            <div className="card card-2 order-details">
                <h2>Order Details</h2>
                {/* <p>Order ID: {highestOrder.orderId}</p> */}
                <p>Order Date: {new Date(highestOrder.orderDate).toLocaleDateString()}</p>
                <p>Menu:</p>
                {highestOrder.menuName.map(order =>
                    <p className='orderpage-menu' key={order.manuItemName}>{order.manuItemName} - {order.quantity}</p>
                )}
                <p>Amount: ${highestOrder.price}</p>
                <p>Status: {highestOrder.status}</p>
                <p>Restaurant: {highestOrder.restaurantName}</p>
                {/* <p>Delivery Partner: {order.deliveryPartner.name}</p> */}
                {/* You can add more details as needed */}
            </div>

            {address && (
                <div className="card shipping-detail">
                    <h2>Shipping Details</h2>
                    <p>House Number: {address.houseNumber}</p>
                    <p>Building Namer: {address.buildingName}</p>
                    <p>Locality: {address.locality}</p>
                    <p>City: {address.city.name}</p>
                    <p>LandMark: {address.landMark}</p>
                </div>
            )}

            <div className='to-profile-btn'>
                <button className='btn btn-primary' onClick={toProfile}>Go To Profile</button>
            </div>

            <div className="thank-you-note">
                <h2>Thank You for Ordering using HotPot !!</h2>
            </div>
        </div>
    );
};

export default Confirmation;