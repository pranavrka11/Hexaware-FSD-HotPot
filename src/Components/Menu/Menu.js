import React, { useState, useEffect, useRef } from 'react';
import axios from 'axios';
import Slider from 'react-slick';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';

function Menu() {
    const [menuItems, setMenuItems] = useState([]);
    const [filteredMenuItems, setFilteredMenuItems] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState('all');
    const [searchQuery, setSearchQuery] = useState('');
    const [searchedItems, setSearchedItems] = useState([]);
    const sliderRef = useRef();

    useEffect(() => {
        // Fetch menu items from the backend API
        axios.get('http://localhost:5272/api/Customer/menu')
            .then(response => {
                setMenuItems(response.data);
                setFilteredMenuItems(response.data); // Initially set filtered items to all items
            })
            .catch(error => {
                console.error('Error fetching menu items:', error);
            });
    }, []);

    useEffect(() => {
        // Filter menu items based on selected category
        if (selectedCategory === 'all') {
            setFilteredMenuItems(menuItems);
        } else {
            const filteredItems = menuItems.filter(item => item.cuisine.toLowerCase() === selectedCategory.toLowerCase());
            setFilteredMenuItems(filteredItems);
        }
    }, [selectedCategory, menuItems]);

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

    const handleCategoryFilter = (category) => {
        setSelectedCategory(category);
    };

    const handleSearch = async () => {
        try {
            const response = await axios.get(`http://localhost:5272/api/Customer/menu/search?query=${searchQuery}`);
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

    const handleAddToCart = (item) => {
        // Implement your logic to add the item to the cart
        console.log('Added to cart:', item);
    };
    

    return (
        <div>
            <h2>Menu Items</h2>
            <div className="search-bar-container">
                <input
                    type="text"
                    placeholder="Search..."
                    value={searchQuery}
                    onChange={handleInputChange}
                    onKeyPress={handleKeyPress}
                />
                <button onClick={handleSearch}>Search</button>
            </div>
            {searchedItems.length > 0 && (
                <div className="searched-items-container">
                    <h3>Searched Items</h3>
                    {searchedItems.map(item => (
                        <div key={item.menuId} className="searched-item">
                            <img src={item.menuImage} alt={item.name} />
                            <h3>{item.name}</h3>
                            <p>Price: ${item.price.toFixed(2)}</p>
                            <p>{item.description}</p>
                            <button className="add-cart" onClick={() => handleAddToCart(item)}>Add to Cart</button>
                        </div>
                    ))}
                </div>
            )}
            {searchedItems.length === 0 && (
                <div>
                    <div className="categories">
                        <button className={`category-btn ${selectedCategory === 'all' ? 'active' : ''}`} onClick={() => handleCategoryFilter('all')}>All</button>
                        <button className={`category-btn ${selectedCategory === 'main course' ? 'active' : ''}`} onClick={() => handleCategoryFilter('main course')}>Main Course</button>
                        <button className={`category-btn ${selectedCategory === 'break fast' ? 'active' : ''}`} onClick={() => handleCategoryFilter('break fast')}>Breakfast</button>
                        <button className={`category-btn ${selectedCategory === 'icecream' ? 'active' : ''}`} onClick={() => handleCategoryFilter('icecream')}>Ice Cream</button>
                    </div>
                    <Slider ref={sliderRef} {...settings}>
                        {filteredMenuItems.map(item => (
                            <div key={item.menuId} className="menu-item">
                                <img src={item.menuImage} alt={item.name} />
                                <h3>{item.name}</h3>
                                <p>Price: ${item.price.toFixed(2)}</p>
                                <p>{item.description}</p>
                                <button className="add-cart" onClick={() => handleAddToCart(item)}>Add to Cart</button>
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
