import React, { useState } from 'react';
import axios from 'axios';
import './Customer.css';

function CustomerRegistration() {
  const [isLoginFormDisplayed, setIsLoginFormDisplayed] = useState(false);
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('http://localhost:5272/api/CustomerLogin/Register', {
        name: name,
        email: email,
        phone: phone,
        userName: username,
        password: password,
        role: 'Customer',
      });

      console.log('Registration successful:', response.data);
      // Clear form fields after successful registration
      setName('');
      setEmail('');
      setPhone('');
      setUsername('');
      setPassword('');
    } catch (error) {
      console.error('Registration failed:', error.message);
      setError('Registration failed. Please try again.');
    }
  };

  const handleLoginSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('http://localhost:5272/api/CustomerLogin/LogIn', {
        userName: username,
        password: password,
        role: '',
        token: '',
      });

      console.log('Login successful:', response.data);
      // Store token in session storage
      sessionStorage.setItem('token', response.data.token);
      // Clear username and password fields
      setUsername('');
      setPassword('');
    } catch (error) {
      console.error('Login failed:', error.message);
      setError('Login failed. Please check your username and password.');
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
                  <label htmlFor="Username">Username:</label>
                  <input type="text" name="username" id="Username" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" required />
                </div>
                <div className="input-field">
                  <label htmlFor="Password">Password:</label>
                  <input type="password" name="password" id="Password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
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
                  <label htmlFor="Name">Name:</label>
                  <input type="text" name="name" id="Name" value={name} onChange={(e) => setName(e.target.value)} placeholder="Name" required />
                </div>
                <div className="input-field">
                  <label htmlFor="Email">Email:</label>
                  <input type="email" name="email" id="Email" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />
                </div>
                <div className="input-field">
                  <label htmlFor="Phone">Phone:</label>
                  <input type="text" name="phone" id="Phone" value={phone} onChange={(e) => setPhone(e.target.value)} placeholder="Phone" required />
                </div>
                <div className="input-field">
                  <label htmlFor="Username">Username:</label>
                  <input type="text" name="username" id="Username" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" required />
                </div>
                <div className="input-field">
                  <label htmlFor="Password">Password:</label>
                  <input type="password" name="password" id="Password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
                </div>
                <div className="input-field">
                  <input type="submit" value="Register" />
                </div>
              </form>
              {error && <p className="error-message">{error}</p>}
            </div>
          )}
        </div>
      </section>
    </main>
  );
}

export default CustomerRegistration;
