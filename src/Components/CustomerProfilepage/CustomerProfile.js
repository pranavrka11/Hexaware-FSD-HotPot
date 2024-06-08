import React, { useEffect, useState } from 'react';
import './customerprofile.css'; // Importing the CSS file
import axios from 'axios';
import { Card, Col, Image, Row } from 'react-bootstrap';
import { useAuth } from '../../Context/AuthContext';
import StarRating from './StarRating';

const Profile = () => {
    //Fetching customer id from session storage
    const customerId = sessionStorage.getItem('UserId');
    const customerToken = sessionStorage.getItem('Token');

    // const { isLoggedIn, handleLogin } = useAuth();

    const [cities, setCities] = useState([]);

    //Object to store customer details
    var [customerDetails, setCustomerDetails] = useState({
        "id": 0,
        "name": "",
        "email": "",
        "phone": "",
        "userName": ""
    });

    //Object to store customer address
    var [customerAddress, setCustomerAddress] = useState({
        "addressId": 0,
        "customerId": 0,
        "houseNumber": "",
        "buildingName": "",
        "locality": "",
        "cityId": 0,
        "landMark": "",
        "city": {
            "cityId": 0,
            "name": "",
            "stateId": 0,
            "state": null
        }
    });

    var addressId = customerAddress.addressId;

    //Object to store customer order history
    var [orderHistory, setOrderHistory] = useState([{
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

    const [deliveryPartner, setDeliveryPartner] = useState({
        "partnerId": 0,
        "name": "r",
        "phone": "",
        "email": "",
        "cityId": 0,
        "userName": ""
    })

    //fetching city names
    useEffect(() => {
        axios.get('http://localhost:5249/api/Customer/GetAllCities')
            .then(function (response) {
                setCities(response.data);
            })
            .catch(function (error) {
                console.log(error);
            })
    }, []);


    //To handle changes in customer detail fields and setting them to customerProfile object
    const handleProfileChange = (fieldName, value) => {
        setCustomerDetails(prevState => ({
            ...prevState,
            [fieldName]: value
        }));
    };

    //To handle changes in customer address fields and setting them to customerAddressobject
    const handleAddressChange = (fieldName, value) => {
        setCustomerAddress(prevState => ({
            ...prevState,
            [fieldName]: value
        }));
    };

    // To fetch customer profile details
    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };

    useEffect(() => {
        const fetchCustomerDetails = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/Customer/GetCustomerDetails?customerId=${customerId}`, requestOptions);
                setCustomerDetails(response.data);
            } catch (error) {
                console.log(error);
            }
        };

        fetchCustomerDetails();
    }, []);

    // To fetch customer address details
    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };

    useEffect(() => {
        const fetchCustomerAddress = async () => {
            try {
                const response = await axios.get(`http://localhost:5249/api/Customer/address/${customerId}`, requestOptions);
                setCustomerAddress(response.data);
            } catch (error) {
                console.log(error);
            }
        };

        fetchCustomerAddress();
    }, []);

    //To fetch order history of a customer
    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };

    useEffect(() => {
        fetchOrderHistory();
    }, [orderHistory]);

    const fetchOrderHistory = () => {
        axios.get(`http://localhost:5249/api/Customer/ViewOrderHistoryForCustomer?customerId=${customerId}`, requestOptions)
            .then(function (response) {
                setOrderHistory(response.data);
            })
            .catch(function (error) {
                console.log(error);
            });
    };

    //Method to edit customer details
    const editCustomerDetails = () => {
        var requestOptions = {
            method: 'PUT',
            // headers: { 'Authorization': 'Bearer ' + customerToken },
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(customerDetails)
        };

        fetch("http://localhost:5249/api/Customer/UpdateCustomerDetails", requestOptions)
            .then(r => r.json())
            .then(alert("Profile details updated successfully"))
            .catch(e => console.log(e));
    };

    //method to edit customer address
    const editCustomerAddress = () => {
        var requestOptions = {
            method: 'PUT',
            // headers: { 'Authorization': 'Bearer ' + customerToken },
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(customerAddress)
        };

        fetch(`http://localhost:5249/api/Customer/address/${addressId}`, requestOptions)
            .then(r => r.json())
            .then(alert("Address updated successfully"))
            .catch(e => console.log(e));
    };

    //method to cancel an order
    const cancelOrder = (orderId) => {
        //http://localhost:5249/api/Customer/CancelOrderFromCustomer?orderId=5
        // console.log("Order id: " + oId);
        // console.log(orderHistory);
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            headers: { 'Authorization': 'Bearer ' + customerToken }
        };

        fetch(`http://localhost:5249/api/Customer/CancelOrderFromCustomer?orderId=${orderId}`, requestOptions)
            .catch(e => console.log(e));
    };

    //fetching delivery partner details
    const fetchPartnerDetails = (partnerId) => {
        // console.log("Partner id: "+partnerId);
        axios.get(`http://localhost:5249/api/DeliveryPartner/GetDetails?partnerId=${partnerId}`)
            .then(function (response) {
                setDeliveryPartner(response.data);
            })
            .catch(function (error) {
                console.log(error);
            });
    };

    const [selectedRating, setSelectedRating] = useState(0);

    const handleRatingChange = (value) => {
        setSelectedRating(value);
    };

    const convertStarRatingToInt = (rating) => {
        switch (rating) {
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return 4;
            case 5:
                return 5;
            default:
                return 0;
        }
    };

    const [feedback, setFeedback] = useState('');

    const changeFeedback = (e) => {
        setFeedback(e.target.value);
    };

    const submitFeedback = (restaurantId) => {
        const rating = convertStarRatingToInt(selectedRating);
        // console.log("Restaurant id: " + restaurantId + " rating: " + rating + " feedback: " + feedback);
        const customerReview = {
            "customerId": customerId,
            "restaurantId": restaurantId,
            "rating": rating,
            "textReview": feedback,
        }
        // console.log("Review object ready");
        // console.log(customerReview);
        var requestOptions = {
            method: 'POST',
            // headers: { 'Authorization': 'Bearer ' + customerToken },
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(customerReview)
        };
        fetch('http://localhost:5249/api/Customer/review', requestOptions)
            .then(r => r.json)
            .catch(e => console.log(e));
    };

    return (
        <>
            <br /><br /><br /><br /><br />
            <div className="page-container">
                <div className="cards-container">
                    <div className="first-card">
                        <div className="imgbox">
                            {/* You can include user avatar here if available */}
                            <img
                                src="/UserProfile.jpg"
                                alt="User Avatar"
                            />
                        </div>
                        <div className="content">
                            <h2>Profile</h2>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Name:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerDetails.name} onChange={(e) => handleProfileChange('name', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Email:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerDetails.email} onChange={(e) => handleProfileChange('email', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Phone:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerDetails.phone} onChange={(e) => handleProfileChange('phone', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>UserName:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerDetails.userName} disabled />
                            </div>
                        </div>
                        <button className="edit-button" onClick={editCustomerDetails}>Edit</button> {/* Edit button */}

                    </div>

                    <div className="first-card">
                        <div className="imgbox">
                            {/* You can include address image here if available */}
                            <img
                                src="/addressLogo.jpg"
                                alt="Address Image"
                            />
                        </div>
                        <div className="content">
                            <h2>Address</h2>
                            <div className='detail-field'>
                                <label htmlFor='Name'>House Number:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerAddress.houseNumber} onChange={(e) => handleAddressChange('houseNumber', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Building Name:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerAddress.buildingName} onChange={(e) => handleAddressChange('buildingName', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Locality:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerAddress.locality} onChange={(e) => handleAddressChange('locality', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>Landmark:&nbsp;&nbsp;</label>
                                <input className='first-text' type='text' value={customerAddress.landMark} onChange={(e) => handleAddressChange('landMark', e.target.value)} />
                            </div>
                            <div className='detail-field'>
                                <label htmlFor='Name'>City:&nbsp;&nbsp;</label>
                                {/* <input className='first-text' type='text' value={customerAddress.city.name} onChange={(e) => handleAddressChange('cityId', e.target.value)} /> */}
                                <select value={customerAddress.city.name} onChange={(e) => {
                                    const selectedCityOption = e.target.value;
                                    console.log("Selected option: " + selectedCityOption);
                                    const selectedCity = cities.find(city => city.name === selectedCityOption);
                                    const selectedCityId = selectedCity.cityId;
                                    // setCustomerAddress(prevState => ({
                                    //     ...prevState,
                                    //     cityId: selectedCityId
                                    // }));
                                    handleAddressChange('cityId', selectedCityId);
                                    handleAddressChange('city', selectedCity);
                                    console.log("New city: " + selectedCity + " Id: " + selectedCityId);
                                    console.log(customerAddress);
                                }}>
                                    {/* <option value={customerAddress.city.name}>{customerAddress.city.name}</option> */}
                                    {Array.isArray(cities) && cities.map((city, index) => {
                                        // if (city.name === customerAddress.city.name) {
                                        //     return null; // Skip iteration for Pune
                                        // }

                                        return <option key={index} value={city.name}>{city.name}</option>;
                                    })}
                                </select>
                            </div>
                        </div>
                        <button className="edit-button" onClick={editCustomerAddress}>Edit</button> {/* Edit button */}

                    </div>
                </div>

                {/* Horizontal wide card for order history */}
                {/* <div className="horizontal-card ">

                    <div className="content">
                        <h2>Order History</h2>
                        <ul>
                            {orderHistory1.map((order, index) => (
                                <li key={index}>{order}</li>
                            ))}
                        </ul>
                    </div>
                </div> */}
                <div className='order-hostory container'>
                    <div className='history-title'>
                        <h1>Your Orders</h1>
                    </div>
                    <div className='order-cards'>
                        {Array.isArray(orderHistory) && orderHistory.slice(0).reverse().map((order) => (
                            <Card key={order.orderId} className="mb-3 order-card">
                                <Card.Body>
                                    <Row>
                                        <Col xs={3}>
                                            <Image src={`/${order.restaurantImage}`} height={100} width={100} fluid />
                                            {order.status !== 'delivered' && order.status !== 'on the way' && order.status !== 'cancelled' ?
                                                (<div className='cancel-button'>
                                                    <button className='btn btn-danger' data-bs-toggle="modal" data-bs-target={`#exampleModal-${order.orderId}`}>Cancel</button>

                                                    <div className="modal fade" id={`exampleModal-${order.orderId}`} tabIndex="-1" aria-labelledby={`exampleModalLabel-${order.orderId}`} aria-hidden="true">
                                                        <div className="modal-dialog">
                                                            <div className="modal-content cancel-order-modal">
                                                                <div className="modal-header">
                                                                    <h1 className="modal-title fs-5" id={`exampleModalLabel-${order.orderId}`}>Cancel Order</h1>
                                                                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                </div>
                                                                <div className="modal-body">
                                                                    Are you sure you want to cancel your order?
                                                                </div>
                                                                <div className="modal-footer">
                                                                    <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                                                    <button type="button" className="btn btn-danger" onClick={() => cancelOrder(order.orderId)} data-bs-dismiss="modal">Cancel</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>) : order.status === 'on the way' ?
                                                    (
                                                        <div className='delivery-partner-button'>
                                                            <button className='btn btn-primary' data-bs-toggle="modal" data-bs-target={`#exampleModal-${order.orderId}`} onClick={() => fetchPartnerDetails(order.partnerId)}>Delivery Partner</button>
                                                            <div className="modal fade" id={`exampleModal-${order.orderId}`} tabIndex="-1" aria-labelledby={`exampleModalLabel-${order.orderId}`} aria-hidden="true">
                                                                <div className="modal-dialog">
                                                                    <div className="modal-content cancel-order-modal">
                                                                        <div className="modal-header">
                                                                            <h1 className="modal-title fs-5" id={`exampleModalLabel-${order.orderId}`}>Delivery Partner Details</h1>
                                                                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                        </div>
                                                                        <div className="modal-body">
                                                                            <p>Name: {deliveryPartner.name}</p>
                                                                            <p>Phone: {deliveryPartner.email}</p>
                                                                            <p>Email: {deliveryPartner.phone}</p>
                                                                        </div>
                                                                        <div className="modal-footer">
                                                                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    ) : order.status === 'delivered' ?
                                                        (
                                                            <div className='rating'>
                                                                <button className='btn btn-success' data-bs-toggle="modal" data-bs-target={`#exampleModal-${order.orderId}`}>Give Rating</button>

                                                                <div className="modal fade" id={`exampleModal-${order.orderId}`} tabIndex="-1" aria-labelledby={`exampleModalLabel-${order.orderId}`} aria-hidden="true">
                                                                    <div className="modal-dialog">
                                                                        <div className="modal-content cancel-order-modal">
                                                                            <div className="modal-header">
                                                                                <h1 className="modal-title fs-5" id={`exampleModalLabel-${order.orderId}`}>How was your experience with {order.restaurantName}</h1>
                                                                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                            </div>
                                                                            <div className="modal-body">
                                                                                Give rating
                                                                                <StarRating onChange={handleRatingChange} /><br />
                                                                                Share feedback
                                                                                <input type='text-area' placeholder='(optional)' value={feedback} onChange={changeFeedback}></input>
                                                                            </div>
                                                                            <div className="modal-footer">
                                                                                <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                                                                <button type="button" className="btn btn-success" onClick={() => submitFeedback(order.restaurantId)} data-bs-dismiss="modal">Submit</button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        ) : null
                                            }
                                        </Col>

                                        <Col xs={6}>
                                            <h5><b>{order.restaurantName}</b></h5>
                                            <hr></hr>
                                            <div className='order-menu'>
                                                {order.menuName.map((menuItem) => (
                                                    <div key={menuItem.manuItemName}>
                                                        <p>{menuItem.manuItemName} - {menuItem.quantity}</p>
                                                    </div>
                                                ))}
                                            </div>
                                        </Col>
                                        <Col xs={3}>
                                            {order.status === 'delivered' || order.status === 'on the way' ?
                                                (<div className='order-status-delivered'>
                                                    <p className="text-right status-text">{order.status}</p>
                                                </div>) : order.status == 'cancelled' ?
                                                    (<div className='order-status-cancelled'>
                                                        <p className="text-right status-text">{order.status}</p>
                                                    </div>) :
                                                    (<div className='order-status-undelivered'>
                                                        <p className="text-right status-text">{order.status}</p>
                                                    </div>)
                                            }

                                            <hr />
                                            <div className='order-price'>
                                                <p className='text-right'>Order Price :</p>
                                                <p className="text-right">Rs. {order.price}</p>
                                            </div>
                                            <div className='order-date'>
                                                <p>{order.orderDate}</p>
                                            </div>
                                        </Col>
                                    </Row>
                                </Card.Body>
                            </Card>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
};

export default Profile;