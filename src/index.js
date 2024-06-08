import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import RestaurantNav from './Components/Homepage/RestaurantNav';
import RestaurantCarousel from './Components/Homepage/RestaurantCarousel';
import Carousel2 from './Components/Homepage/Carousel2';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './Components/Store';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Provider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
