import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Menu.css'

function MenuCategories() {
    const [menuItems, setMenuItems] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState('all');

    useEffect(() => {
        // Fetch menu items from the backend API
        axios.get('http://localhost:5249/api/Customer/GetMenuByRestaurant?restaurantId=1')
            .then(response => {
                setMenuItems(response.data);
            })
            .catch(error => {
                console.error('Error fetching menu items:', error);
            });
    }, []);

    const handleCategoryClick = (category) => {
        setSelectedCategory(category);
    };

    return (
        <section id="menu-categories">
            <br /><br /><br /><br />
            <h2>Menu Categories</h2>
            <div className="categories">
                <button className="category-btn" onClick={() => handleCategoryClick('all')}>All</button>
                <button className="category-btn" onClick={() => handleCategoryClick('main course')}>Main Course</button>
                <button className="category-btn" onClick={() => handleCategoryClick('breakfast')}>Breakfast</button>
                <button className="category-btn" onClick={() => handleCategoryClick('ice cream')}>Ice Cream</button>
            </div>
            <div className="menu-listings">
                {menuItems.map(item => (
                    (selectedCategory === 'all' || item.cuisine.toLowerCase() === selectedCategory) && (
                        <div key={item.menuId} className="product-card">
                            <img src={item.menuImage} alt={item.name} />
                            <h3>{item.name}</h3>
                            <p>{item.description}</p>
                        </div>
                    )
                ))}
            </div>
        </section>
    );
}

export default MenuCategories;