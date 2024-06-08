import axios from "axios";
import { useState, useEffect } from "react";
import './cart.css'

function Cart(props) {
    var [carts, setCarts] = useState([{
        "cartId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "menuItemId": 0,
        "menuTitle": "string",
        "quantity": 0,
        "price": 0
    }])

    var [isNotEmpty, setIsNotEmpty] = useState(false);
    //var [isLoading, setIsLoading]=useState(true);
    //const axios = require('axios');
    const userId = props.userid;
    const url = `http://localhost:5249/api/Customer/ViewCart?userId=${userId}`;

    useEffect(() => {
        var data = axios.get(url)
            .then(function (response) {
                setCarts(response.data);
                setIsNotEmpty(response.data.length > 0); // Update isNotEmpty based on response data
            })
            .catch(function (error) {
                console.log(error);
                setIsNotEmpty(false);
            })
    },[carts]);

    // const userId = 1;
    // const url = `http://localhost:5249/api/Customer/ViewCart?userId=${userId}`;
    // var data = axios.get(url)
    //     .then(function (response) {
    //         setCarts(response.data);
    //         console.log(carts);
    //         setIsNotEmpty(response.data.length > 0); // Update isNotEmpty based on response data
    //     })
    //     .catch(function (error) {
    //         console.log(error);
    //         setIsNotEmpty(false);
    //     })

    // useEffect(() => {
    //     const fetchData = async () => {
    //         try {
    //             const response = await axios.get(url);
    //             setCarts(response.data);
    //             setIsNotEmpty(response.data.length > 0);
    //             setIsLoading(false);
    //         } catch (error) {
    //             console.error(error);
    //             setIsNotEmpty(false);
    //             setIsLoading(false);
    //         }
    //     };

    //     fetchData();
    // }, [url]);

    // var data = fetch(url)
    //     .then(r => r.json())
    //     .then(r => {
    //         setCarts(r);
    //         setIsNotEmpty(r.l > 0);
    //     })
    //     .catch(e => {
    //         console.log(e)
    //         setIsNotEmpty(false);
    //     });

    var increaseCartItemQuantity = (cartID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' }
        };

        const increaseCart = `http://localhost:5249/api/Customer/IncreaseCartItemQuantity?cartId=${cartID}`;
        fetch(increaseCart, requestOptions)
            .then(r => r.json())
            .then(r => console.log(r))
            .catch(e => console.log(e));
    };

    var decreaseCartItemQuantity = (cartID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' }
        };

        const decreaseCart = `http://localhost:5249/api/Customer/DecreaseCartItemQuantity?cartId=${cartID}`;
        fetch(decreaseCart, requestOptions)
            .catch(e => console.log(e));
    };

    var deleteCartItem = (cartID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' }
        };
        const deleteCart = `http://localhost:5249/api/Customer/DeleteCartItem?cartId=${cartID}`;
        fetch(deleteCart, requestOptions)
            .catch(e => console.log(e));
    };

    var emptyCart = (customerID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' }
        };

        const emptyCartUrl = `http://localhost:5249/api/Customer/EmptyCart?customerId=${customerID}`;
        fetch(emptyCartUrl, requestOptions)
            .catch(e => console.log(e));
    };

    var purchaseCartItem = (cartID) => { };

    var purchaseAllItems = (customerID) => { };

    return (
        <div className="container">
            <h1 className="my-4">My carts</h1>
            {isNotEmpty == true ?
                <div className="alert">
                    <button className="btn btn-danger" onClick={() => emptyCart(userId)}>Empty</button>
                    <button className="btn btn-warning" onClick={() => purchaseAllItems(cart.customerId)}>Purchase</button>
                </div> : 
                <div>
                    <br/><br/><br/>
                    <h1 className="empty-cart">The cart is empty</h1>
                </div>
            }
            {Array.isArray(carts) && carts.map((cart) =>
                <div key={cart.cartId}>
                    <div className="row border-bottom py-3">
                        <div className="col-3">
                            <img src="fimg1.jpg" alt={cart.menuTitle} className="img-fluid" />
                        </div>
                        <div className="col-6">
                            <h5>{cart.menuTitle}</h5>
                            <p>Quantity: {cart.quantity}</p>
                            <p>Price: ${cart.price.toFixed(2)}</p>
                        </div>
                        <div className="col-3 d-flex flex-column justify-content-between">
                            <button className="btn btn-primary" onClick={() => increaseCartItemQuantity(cart.cartId)}>+</button>
                            <button className="btn btn-primary" onClick={() => decreaseCartItemQuantity(cart.cartId)}>-</button>
                            <button className="btn btn-danger" onClick={() => deleteCartItem(cart.cartId)}>Delete</button>
                            <button className="btn btn-warning mt-2" onClick={() => purchaseCartItem(cart.cartId)}>Purchase</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}

export default Cart;