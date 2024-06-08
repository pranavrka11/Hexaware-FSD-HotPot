// Header.js
import React from 'react';
import './Menu.css';

function MenuHeader() {
    return (
        <header className="home-header">
            <div className="container">
                <div className="logo">
                    <a href="index.html">HotPot</a>
                </div>
                <nav>
                    <ul>
                        <li className="link">
                            <a href="index.html">Home</a>
                        </li>
                        <li className="link">
                            <a href="Contact.html">Contact us</a>
                        </li>
                        <li className="link">
                            <a href="Registration.html">Registration</a>
                        </li>
                    </ul>
                </nav>
            </div>
            <input type="text" id="search" placeholder="Search" />
        </header>
    );
}

export default MenuHeader;