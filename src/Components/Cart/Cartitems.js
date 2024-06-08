import axios from "axios";
import { useState, useEffect } from "react";
import './cart.css';
import { useNavigate, useParams } from "react-router-dom";

export default function Cartitems(){
    var [carts, setCarts] = useState([{
        "cartId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "menuItemId": 0,
        "menuTitle": "string",
        "quantity": 0,
        "price": 0,
        "menuImage":"string"
    }]);

    var [isNotEmpty, setIsNotEmpty] = useState(false);
    const {userId} = useParams();
    const customerToken=sessionStorage.getItem('Token');
    var requestOptions={
        headers:{'Authorization':'Bearer '+customerToken}
    };
    const url = `http://localhost:5249/api/Customer/ViewCart?userId=${userId}`;

    useEffect(() => {
        var data = axios.get(url, requestOptions)
            .then(function (response) {
                setCarts(response.data);
                setIsNotEmpty(response.data.length > 0); // Update isNotEmpty based on response data
            })
            .catch(function (error) {
                console.log(error);
                setIsNotEmpty(false);
            })
    },[carts]);

    var increaseCartItemQuantity = (cartID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            headers:{'Authorization':'Bearer '+customerToken}
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
            headers: { 'Content-Type': 'application/json' },
            headers:{'Authorization':'Bearer '+customerToken}
        };

        const decreaseCart = `http://localhost:5249/api/Customer/DecreaseCartItemQuantity?cartId=${cartID}`;
        fetch(decreaseCart, requestOptions)
            .catch(e => console.log(e));
    };

    var deleteCartItem = (cartID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            headers:{'Authorization':'Bearer '+customerToken}
        };
        const deleteCart = `http://localhost:5249/api/Customer/DeleteCartItem?cartId=${cartID}`;
        fetch(deleteCart, requestOptions)
            .catch(e => console.log(e));
    };

    var emptyCart = (customerID) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            headers:{'Authorization':'Bearer '+customerToken}
        };

        const emptyCartUrl = `http://localhost:5249/api/Customer/EmptyCart?customerId=${customerID}`;
        fetch(emptyCartUrl, requestOptions)
            .catch(e => console.log(e));
    };

    var purchaseCartItem = (cartID) => { };

    const navigate=useNavigate();
    var purchaseAllItems = () => {
        navigate('/check-out-page');
     };

    return (
        <div className="container-1">
            
            {isNotEmpty == true ?
            
                <div className="alert">
                    <br/><br/><br/>
                    <h1 className="my-4">My carts</h1>
                    <button className="btn btn-danger" onClick={() => emptyCart(userId)}>Empty</button>
                    <button className="btn btn-warning" onClick={purchaseAllItems}>Purchase</button>
                </div> : 
                <div className="empty-cart">
                    <br/><br/><br/><br/><br/><br/>
                    <h1>The cart is empty</h1>
                    <img src="/emptycart.jpg" height={500} width={700} />
                    <br/><br/><br/><br/><br/><br/><br/><br/><br/>
                </div>
            }
            {Array.isArray(carts) && carts.map((cart) =>
                <div key={cart.cartId} className="cart-item-card">
                    <div className="row border-bottom py-3 card-inside">
                        <div className="col-3">
                            <img src={`/${cart.menuImage}`} height={100} width={100} alt={cart.menuTitle} className="img-fluid" />
                        </div>
                        <div className="col-6">
                            <h5>{cart.menuTitle}</h5>
                            <p>Quantity: {cart.quantity}</p>
                            <p>Price: Rs. {cart.price.toFixed(2)}</p>
                        </div>
                        <div className="col-3 d-flex flex-column justify-content-between">
                            <button className="btn btn-primary" onClick={() => increaseCartItemQuantity(cart.cartId)}>+</button>
                            <button className="btn btn-primary" onClick={() => decreaseCartItemQuantity(cart.cartId)}>-</button>
                            <button className="btn btn-danger" onClick={() => deleteCartItem(cart.cartId)}>Delete</button>
                            {/* <button className="btn btn-warning mt-2" onClick={() => purchaseCartItem(cart.cartId)}>Purchase</button> */}
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}