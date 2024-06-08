import React, { useState, useEffect, useRef } from 'react';
import axios from 'axios';
import './Menu.css'
import Slider from 'react-slick';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
// import { useAuth } from '../../Context/AuthContext';
import { useSelector } from 'react-redux';

function Menu() {
    const [menuItems, setMenuItems] = useState([]);
    const [filteredMenuItems, setFilteredMenuItems] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState('all');
    const [searchQuery, setSearchQuery] = useState('');
    const [searchedItems, setSearchedItems] = useState([]);
    const sliderRef = useRef();
    const { restaurantId } = useParams();
    const userId = sessionStorage.getItem('UserId');
    const [carts, setCarts] = useState([]);
    // const { isLoggedIn, handleLogin } = useAuth();
    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn);
    const navigate = useNavigate();
    const customerToken = sessionStorage.getItem('Token');
    const customerId = sessionStorage.getItem('UserId');
    const location = useLocation();
    

    // var [isNotEmpty, setIsNotEmpty] = useState(false);
    const url = `http://localhost:5249/api/Customer/ViewCart?userId=${userId}`;
    var requestOptions = {
        headers: { 'Authorization': 'Bearer ' + customerToken }
    };

    useEffect(() => {
        var data = axios.get(url, requestOptions)
            .then(function (response) {
                setCarts(response.data);
                // setIsNotEmpty(response.data.length > 0); // Update isNotEmpty based on response data
            })
            .catch(function (error) {
                console.log(error);
                // setIsNotEmpty(false);
            })
    });

    useEffect(() => {
        // Fetch menu items from the backend API
        axios.get(`http://localhost:5249/api/Customer/GetMenuByRestaurant?restaurantId=${restaurantId}`)
            .then(response => {
                setMenuItems(response.data);
                setFilteredMenuItems(response.data); // Initially set filtered items to all items
            })
            .catch(error => {
                console.error('Error fetching menu items:', error);
            });
    }, [setMenuItems, setFilteredMenuItems, setCarts]);

    useEffect(() => {
        // Filter menu items based on selected category
        if (selectedCategory === 'all') {
            setFilteredMenuItems(menuItems);
        } else {
            const filteredItems = menuItems.filter(item => item.cuisine.toLowerCase() === selectedCategory.toLowerCase());
            setFilteredMenuItems(filteredItems);
        }
    }, [selectedCategory, menuItems, carts]);

    useEffect(() => {
        // Search menu items based on search query
        if (searchQuery.trim() !== '') {
            const searchResults = menuItems.filter(item =>
                item.name.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setSearchedItems(searchResults);
        } else {
            setSearchedItems([]);
        }
    }, [searchQuery, menuItems]);

    const settings = {
        dots: true,
        infinite: true,
        speed: 500,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                },
            },
        ],
    };

    const handleNext = () => {
        sliderRef.current.slickNext();
    };

    const handlePrev = () => {
        sliderRef.current.slickPrev();
    };

    const handleCategoryFilter = (e) => {
        setSelectedCategory(e.target.value);
    };

    const handleSearch = async () => {
        try {
            const response = await axios.get(`http://localhost:5249/api/Customer/menu/search?restaurantId=${restaurantId}&query=${searchQuery}`);
            setSearchedItems(response.data);
        } catch (error) {
            console.error('Error searching menu items:', error);
        }
    };

    const handleInputChange = (event) => {
        setSearchQuery(event.target.value);
    };

    const handleKeyPress = (event) => {
        if (event.key === 'Enter') {
            handleSearch();
        }
    };

    const handleAddToCart = (menuItemId) => {
        // Implement your logic to add the item to the cart
        if (isLoggedIn) {
            var requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                headers: { 'Authorization': 'Bearer ' + customerToken }
            };
            fetch(`http://localhost:5249/api/Customer/AddToCart?userId=${customerId}&menuItem=${menuItemId}`, requestOptions)
                .then(r => r.json())
                .then(r => {
                    console.log(r);
                    setCarts([...carts, r]);
                })
                .catch(e => console.log(e));
        }
        else {
            // navigate('/customer-login');
            navigate('/customer-login', { state: { from: location } });
        }

        // {carts.length>=0?window.location.reload():null}
    };

    const handleRemoveFromCart = (CartId) => {
        var requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            headers: { 'Authorization': 'Bearer ' + customerToken }
        };
        const deleteCart = `http://localhost:5249/api/Customer/DeleteCartItem?cartId=${CartId}`;
        fetch(deleteCart, requestOptions)
            .then(r => {
                if (r.ok) {
                    setCarts(prevCarts => prevCarts.filter(cart => cart.cartId !== CartId));
                }
            })
            .catch(e => console.log(e));
    }


    return (
        <div>
            <br /><br /><br /><br /><br /><br />
            <h2 className='resto-name-class'>Menu Items</h2>
            {/* <div className="search-bar-container">
                <input
                    type="text"
                    placeholder="Search..."
                    value={searchQuery}
                    onChange={handleInputChange}
                    onKeyPress={handleKeyPress}
                />
                <button onClick={handleSearch}>Search</button>
            </div> */}
            <div className='restaurant-controller'>
                <nav className="navbar bg-body-tertiary controller-navbar">
                    <div className="container-fluid controller-content">
                        <h3>Filter Menu</h3>
                        <div className='cuisine-filter'>
                            <h4>Select Cuisine: </h4>
                            <select className='select-cuisine' value={selectedCategory} onChange={handleCategoryFilter}>
                                <option value="all">All</option>
                                <option value="Indian">Indian</option>
                                <option value="South Indian">South Indian</option>
                                <option value="Italian">Italian</option>
                                <option value="Punjabi">Punjabi</option>
                                <option value="American">American</option>
                            </select>
                        </div>
                        <form className="d-flex" role="search">
                            <input className="form-control me-2" type="search" placeholder="Search Menu Items" aria-label="Search" value={searchQuery} onChange={handleInputChange} />
                        </form>
                    </div>
                </nav>
            </div>
            {searchedItems.length > 0 && (
                <div className="searched-items-container">
                    {searchedItems.map(item => (
                        <div key={item.menuId} className="menu-item">
                            <img src={`/${item.itemImage}`} alt={item.name} />
                            <h3>{item.name}</h3>
                            <p>Price: Rs.{item.price.toFixed(2)}</p>
                            <p>{item.description}</p>
                            {/* <button className="add-cart" onClick={() => handleAddToCart(item.menuId)}>Add to Cart</button> */}
                            {Array.isArray(carts) && carts.some((cart) => cart.menuItemId === item.menuId) ?
                                (<button className="added-cart" onClick={() => {
                                    const cartItem = carts.find((cart) => cart.menuItemId === item.menuId);
                                    if (cartItem) { handleRemoveFromCart(cartItem.cartId) }
                                }}>Added to Cart</button>) :
                                (<button className="add-cart" onClick={() => handleAddToCart(item.menuId)}>Add to Cart</button>)
                            }
                        </div>
                    ))}
                </div>
            )}
            {searchedItems.length === 0 && (
                <div>
                    {/* <div className="categories">
                        <button className={`category-btn ${selectedCategory === 'all' ? 'active' : ''}`} onClick={() => handleCategoryFilter('all')}>All</button>
                        <button className={`category-btn ${selectedCategory === 'main course' ? 'active' : ''}`} onClick={() => handleCategoryFilter('main course')}>Main Course</button>
                        <button className={`category-btn ${selectedCategory === 'break fast' ? 'active' : ''}`} onClick={() => handleCategoryFilter('break fast')}>Breakfast</button>
                        <button className={`category-btn ${selectedCategory === 'icecream' ? 'active' : ''}`} onClick={() => handleCategoryFilter('icecream')}>Ice Cream</button>
                    </div> */}
                    <Slider ref={sliderRef} {...settings}>
                        {filteredMenuItems.map(item => (
                            <div key={item.menuId} className="menu-item">
                                <img src={`/${item.itemImage}`} height={100} width={100} alt={item.name} />
                                <h3>{item.name}</h3>
                                <p>Price: Rs. {item.price.toFixed(2)}</p>
                                <p>{item.description}</p>
                                {/* <button className="add-cart" onClick={() => handleAddToCart(item.menuId)}>
                                    {Array.isArray(carts) && carts.some((cart)=>cart.menuItemId===item.menuId)?'Added to cart':'Add to cart'}
                                    </button> */}

                                {Array.isArray(carts) && carts.some((cart) => cart.menuItemId === item.menuId) ?
                                    (<button className="added-cart" onClick={() => {
                                        const cartItem = carts.find((cart) => cart.menuItemId === item.menuId);
                                        if (cartItem) { handleRemoveFromCart(cartItem.cartId) }
                                    }}>Added to Cart</button>) :
                                    (<button className="add-cart" onClick={() => handleAddToCart(item.menuId)}>Add to Cart</button>)
                                }
                            </div>
                        ))}
                    </Slider>
                    <div className="carousel-controls">
                        <button className="control-btn prev" onClick={handlePrev}>Prev</button>
                        <button className="control-btn next" onClick={handleNext}>Next</button>
                    </div>
                </div>
            )}
        </div>
    );
}

export default Menu;