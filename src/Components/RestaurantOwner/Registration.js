import React, { useState, useEffect } from 'react';
import axios from 'axios';
// import './RestaurantOwner.css';
import './Registration.css';

function Registration() {
  const [isLoginFormDisplayed, setIsLoginFormDisplayed] = useState(false);
  const [name, setName] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [restaurantId, setRestaurantId] = useState('');
  const [restaurantList, setRestaurantList] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchRestaurants = async () => {
      try {
        const response = await axios.get('http://localhost:5272/api/Customer/restaurants');
        setRestaurantList(response.data);
      } catch (error) {
        setError('Error fetching restaurants');
      }
    };

    fetchRestaurants();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('http://localhost:5272/api/RestaurantOwner', {
        Name: name,
        UserName: username,
        Password: password,
        Role: 'Restaurant Owner',
        RestaurantId: parseInt(restaurantId),
      });

      console.log('Registration successful:', response.data);
      setName('');
      setUsername('');
      setPassword('');
      setRestaurantId('');
    } catch (error) {
      console.error('Registration failed:', error.message);
    }
  };

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
  
    try {
      const response = await axios.post('http://localhost:5272/api/RestaurantOwner/LoginResto', {
        UserName: username,
        Password: password,
        Role: "",
        Token: "",
      });
  
      console.log('Login successful:', response.data);
  
      // Store token in session storage
      sessionStorage.setItem('token', response.data.token);
  
      // Clear username and password fields
      setUsername('');
      setPassword('');
    } catch (error) {
      console.error('Login failed:', error.message);
    }
  };
  
  const toggleForms = () => {
    setIsLoginFormDisplayed(!isLoginFormDisplayed);
  };

  return (
    <main>
      <section className="user-container">
        <div className="grid-two--column">
          <div className="form-text">
            {isLoginFormDisplayed ? (
              <>
                <h2>Hello, Friend!</h2>
                <p>End your Hunger Journey, with Registration </p>
                <button className="registration-btn" onClick={toggleForms}>Register Here</button>
              </>
            ) : (
              <>
                <h2>Welcome Back!</h2>
                <p>Feeling Hungry!! <br /> Login with personal details </p>
                <button className="login-btn" onClick={toggleForms}>Login Here</button>
              </>
            )}
          </div>

          {isLoginFormDisplayed ? (
            <div className="login-form">
              <h2>Sign In</h2>
              <p>Access your account</p>
              <form onSubmit={handleLoginSubmit}>
                <div className="input-field">
                  <label htmlFor="Username">
                    Username:
                    <input type="text" name="username" id="Username" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" required />
                  </label>
                </div>
                <div className="input-field">
                  <label htmlFor="Password">
                    Password:
                    <input type="password" name="password" id="Password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
                  </label>
                </div>
                <div className="input-field">
                  <input type="submit" value="Login" />
                </div>
              </form>
              {error && <p className="error-message">{error}</p>}
            </div>
          ) : (
            <div className="registration-form">
              <h2>Create Account</h2>
              <p>Just a click away</p>
              <form onSubmit={handleSubmit}>
                <div className="input-field">
                  <label htmlFor="Name">
                    Restaurant Owner Name:
                    <input type="text" name="name" id="Name" value={name} onChange={(e) => setName(e.target.value)} placeholder="Name" required />
                  </label>
                </div>
                <div className="input-field">
                  <label htmlFor="Restaurant">
                    Restaurant:
                    <select name="restaurant" id="Restaurant" value={restaurantId} onChange={(e) => setRestaurantId(e.target.value)} required>
                      <option value="">Select Restaurant</option>
                      {restaurantList.map((restaurant) => (
                        <option key={restaurant.restaurantId} value={restaurant.restaurantId}>{restaurant.restaurantName}</option>
                      ))}
                    </select>
                  </label>
                </div>
                <div className="input-field">
                  <label htmlFor="Username">
                    Username:
                    <input type="text" name="username" id="Username" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" required />
                  </label>
                </div>
                <div className="input-field">
                  <label htmlFor="Password">
                    Password:
                    <input type="password" name="password" id="Password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
                  </label>
                </div>
                <div className="input-field">
                  <input type="submit" value="Register" />
                </div>
              </form>
            </div>
          )}
        </div>
      </section>
    </main>
  );
}

export default Registration;
