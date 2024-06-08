import React from 'react';
import { useState, useEffect } from 'react';
import axios from "axios";
import './Checkout.css'; // Import your CSS file
import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';

const Checkout = () => {
    const [orderSummary, setOrderSummary] = useState([{
        "cartId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "menuItemId": 0,
        "menuTitle": "",
        "quantity": 0,
        "price": 0,
        "menuImage": "",
        "restaurantName": "Cafe Express",
        "restaurantCityId": 1
    }]);
    const [address, setAddress] = useState(null);
    const [review, setReview] = useState([{
        "reviewId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "rating": 0,
        "textReview": ""
    }]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const customerId = sessionStorage.getItem('UserId');
    const customerToken = sessionStorage.getItem('Token');
    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn);
    const navigate = useNavigate();

    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };
    useEffect(() => {
        // Fetch order summary
        const fetchOrderSummary = async () => {
            try {
                const orderSummaryResponse = await axios.get(`http://localhost:5249/api/Customer/ViewCart?userId=${customerId}`, requestOptions);
                setOrderSummary(orderSummaryResponse.data);
            } catch (error) {
                // setError(error.message);
                console.log(error);
            }
        };

        // Fetch customer address
        const fetchAddress = async () => {
            try {
                const addressResponse = await axios.get(`http://localhost:5249/api/Customer/address/${customerId}`, requestOptions);
                setAddress(addressResponse.data);
            } catch (error) {
                // setError(error.message);
                console.log(error);
            }
        };

        const fetchReview = async () => {
            try {
                const reviewResponse = await axios.get('http://localhost:5249/api/Restaurant/GetAllReviews');
                setReview(reviewResponse.data);
            } catch (error) {
                // setError(error.message);
                console.log(error);
            }
        };

        Promise.all([fetchOrderSummary(), fetchAddress(), fetchReview()])
            .then(() => setLoading(false))
            .catch(error => {
                // setError(error.message);
                setLoading(false);
            });
    }, []);


    const handlePlaceOrder = async () => {
        // try {
        //     // Make an HTTP POST request to trigger the controller
        //     await axios.post('http://localhost:5272/api/Customer/create-orders/1');
        //     // Optionally, you can add logic here to handle the response or perform any additional actions
        // } catch (error) {
        //     //   setError(error.message);
        //     console.log(error);
        // }
        if (address.houseNumber === '' && address.buildingName === '' && address.locality === '') {
            alert("Please provide an address to place an order");
        }
        else if (orderSummary[0].restaurantCityId !== address.cityId) {
            alert("Can not deliver order to an address in a different city. Please update your address and try again");
        }
        else {
            const isConfirmed = window.confirm("Do you want to place the order?");
            if (isConfirmed) {
                var requestOptions = {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    headers: { 'Authorization': 'Bearer ' + customerToken }
                };
                fetch(`http://localhost:5249/api/Customer/PlaceOrderForAll?customerId=${customerId}&paymentMode=online`, requestOptions)
                    .then(r => r.json())
                    .then(r => {
                        alert("Order placed!");
                        // navigate(`/customer-profile/${customerId}`);
                        navigate('/order-confirmation');
                    })
                    .catch(e => console.log(e));
            }
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    const totalAmount = orderSummary.reduce((total, item) => total + item.price, 0);

    const renderStarRating = (rating) => {
        const stars = [];
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                stars.push(<span key={i} style={{ color: 'gold' }}>&#9733;</span>); // Filled star
            } else {
                stars.push(<span key={i} style={{ color: 'gold' }}>&#9734;</span>); // Empty star
            }
        }
        return stars;
    };

    // var navigate=useNavigate();
    const AddMoreItemsToCart = () => {
        navigate(`/menus/${orderSummary[0].restaurantId}`);
    }

    const toProfile = () => {
        navigate(`/customer-profile/${customerId}`);
    }

    return (
        <>
            {isLoggedIn ? (
                <div className="checkout-container">
                    <br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <div className="card card-1 order-summary">
                        <h2>Order Summary</h2>
                        <ul>
                            {orderSummary.map(item => (
                                <li key={item.cartId}>
                                    <p>Item: {item.menuTitle}</p>
                                    <p>Quantity: {item.quantity}</p>
                                    <p>Price: {item.price}</p>
                                </li>
                            ))}
                        </ul>
                        <p>Total: Rs.{totalAmount}</p>
                    </div>
                    <button className="add-more-item-btn" onClick={AddMoreItemsToCart}>Add More Item</button>


                    {address && (
                        <div className="card card-1 address">
                            <h2>Shipping Details</h2>
                            <p>House Number: {address.houseNumber}</p>
                            <p>Building Namer: {address.buildingName}</p>
                            <p>Locality: {address.locality}</p>
                            <p>City: {address.city.name}</p>
                            <p>LandMark: {address.landMark}</p>


                            <button className="edit-button-address" onClick={toProfile}>Edit</button>

                        </div>
                    )}

                    <div className='card card-1 payment-details'>
                        <h2>Payment Detals</h2>
                        <p>Card Number: </p>
                        <input type='text' placeholder='0000 0000 0000 0000'></input>
                        <br />
                        <p>Expiry: (mm/yyyy)</p>
                        <input type='text' placeholder='e.g. 04/24'></input>
                        <br />
                        <p>CVV: </p>
                        <input type='password'></input>
                    </div>


                    {/* {review && (
        <div className="card card-1 review">
          <h2>Customer Review</h2>
          <p>Rating: {renderStarRating(review.rating)}</p>
          <p>Review: {review.textReview}</p>
          <p>Restaurant: {review.restaurant.restaurantName}</p>
        </div>
      )} */}


                    <div className="place-order-btn">
                        <button onClick={handlePlaceOrder}>Place Order</button>
                    </div>
                </div>
            ) : null}
        </>
    );
};

export default Checkout;