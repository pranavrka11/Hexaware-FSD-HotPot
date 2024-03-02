import React from 'react';
import { useState, useEffect } from 'react';
import axios from "axios";
import './Checkout.css'; // Import your CSS file

const Checkout = () => {
  const [orderSummary, setOrderSummary] = useState(null);
  const [address, setAddress] = useState(null);
  const [review, setReview] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Fetch order summary
    const fetchOrderSummary = async () => {
      try {
        const orderSummaryResponse = await axios.get('http://localhost:5272/api/Customer/cart/1');
        setOrderSummary(orderSummaryResponse.data);
      } catch (error) {
        setError(error.message);
      }
    };

    // Fetch customer address
    const fetchAddress = async () => {
      try {
        const addressResponse = await axios.get('http://localhost:5272/api/Customer/address/1');
        setAddress(addressResponse.data);
      } catch (error) {
        setError(error.message);
      }
    };

    const fetchReview = async () => {
      try {
        const reviewResponse = await axios.get('http://localhost:5272/api/Customer/review/3');
        setReview(reviewResponse.data);
      } catch (error) {
        setError(error.message);
      }
    };

    Promise.all([fetchOrderSummary(), fetchAddress(), fetchReview()])
      .then(() => setLoading(false))
      .catch(error => {
        setError(error.message);
        setLoading(false);
      });
  }, []);


  const handlePlaceOrder = async () => {
    try {
      // Make an HTTP POST request to trigger the controller
      await axios.post('http://localhost:5272/api/Customer/create-orders/1');
      // Optionally, you can add logic here to handle the response or perform any additional actions
    } catch (error) {
      setError(error.message);
    }
  };
  
    if (loading) {
      return <div>Loading...</div>;
    }

    if (error) {
      return <div>Error: {error}</div>;
    }
 
    const totalAmount = orderSummary.reduce((total, item) => total + (item.quantity * item.menu.price), 0);

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

  

  return (
    <div className="checkout-container">
    <div className="card order-summary">
        <h2>Order Summary</h2>
        <ul>
          {orderSummary.map(item => (
            <li key={item.id}>
              <p>Item: {item.menu.name}</p>
              <p>Quantity: {item.quantity}</p>
              <p>Price: {item.menu.price}</p>
              <p>Amount: Rs.{item.quantity * item.menu.price}</p>
              <p>Restaurant: {item.restaurant.restaurantName}</p>
            </li>
          ))}
        </ul>
        <p>Total: Rs.{totalAmount}</p>
      </div>
      <button className="add-more-item-btn">Add More Item</button>


      {address && (
        <div className="card address">
          <h2>Shipping Details</h2>
          <p>House Number: {address.houseNumber}</p>
          <p>Building Namer: {address.buildingName}</p>
          <p>Locality: {address.locality}</p>
          <p>City: {address.city.name}</p>
          <p>LandMark: {address.landMark}</p>


          <button className="edit-button-address">Edit</button> 

        </div>
      )}


      {review && (
        <div className="card review">
          <h2>Customer Review</h2>
          <p>Rating: {renderStarRating(review.rating)}</p>
          <p>Review: {review.textReview}</p>
          <p>Restaurant: {review.restaurant.restaurantName}</p>
        </div>
      )}


      <div className="place-order-btn">
        <button onClick={handlePlaceOrder}>Place Order</button>
      </div>
    </div>
  );
};

export default Checkout;
