// import logo from './logo.svg';
import './App.css';
import HomePage from './Components/Homepage/HomePage';
import { Route, Routes } from 'react-router-dom';
import Navbar from './Components/Navbar';
import RestaurantPage from './Components/RestaurantsPage/RestaurantPage';
import AboutPage from './Components/Aboutpage/AboutPage';
import Footer from './Components/Footer';
import Cartitems from './Components/Cart/Cartitems';
import { useEffect, useState } from 'react';
import { AuthProvider } from './Context/AuthContext';
import CustomerRegistration from './Components/Loginpage/CustomerRegistration';
import MenuPage from './Components/Menupage/MenuPage';
import CustomerProfile from './Components/CustomerProfilepage/CustomerProfile';
import OwnerProfile from './Components/RestaurantOwnerProfilepage/OwnerProfile';
import PartnerProfile from './Components/DeliveryPartnerProfilepage/PartnerProfile';
import Registration from './Components/Loginpage/RestaurantLoginPage';
import DeliveryPartnerLogin from './Components/Loginpage/DeliveryPartnerLoginPage';
import Checkout from './Components/Checkoutpage/CheckOutPage';
import Confirmation from './Components/Confirmationpage/Confirmation';
import AdminLogin from './Components/Loginpage/AdminLoginPage';
import Admin from './Components/AdminPage/Admin';
import Contact from './Components/Contactpage/Contact';



function App() {
  var [isLoggedIn, setIsLoggedIn] = useState(false);
  // sessionStorage.setItem('selectedCity', 'Pune');

  useEffect(() => {
    const token = sessionStorage.getItem('Token');
    setIsLoggedIn(!!token);
  }, []);

  var handleLogin = () => {
    setIsLoggedIn(true);
  };

  const userId = sessionStorage.getItem('UserId');

  return (
    <>
      <AuthProvider>
        <Navbar />
        <Routes>
          <Route path='/' element={<HomePage />} />
          <Route path='/restaurants' element={<RestaurantPage />} />
          {/* <Route path='/restaurants' element={<RestaurantPage />} /> */}
          <Route path='/about' element={<AboutPage />} />
          <Route path='/contact' element={<Contact />} />
          <Route path='/carts'>
            <Route path=':userId' element={<Cartitems />} />
          </Route>
          {/* <Route path={'/carts/:userId'} element={<Cartitems />} /> */}
          <Route path='/customer-login' element={<CustomerRegistration />} />
          <Route path='/menus'>
            <Route path=':restaurantId' element={<MenuPage />} />
          </Route>
          <Route path='/customer-profile'>
            <Route path=':customerId' element={<CustomerProfile />} />
          </Route>
          {/* <Route path='/customer-login' element={<Login />} /> */}
          <Route path='/restaurant-login' element={<Registration />} />
          <Route path='/restaurant-profile' element={<OwnerProfile />} />
          <Route path='/delivery-partner-login' element={<DeliveryPartnerLogin />} />
          <Route path='/delivery-partner-profile' element={<PartnerProfile />} />
          <Route path='/check-out-page' element={<Checkout />} />
          <Route path='/order-confirmation' element={<Confirmation />} />
          <Route path='/admin-login' element={<AdminLogin />} />
          <Route path='/admin-profile' element={<Admin />} />
        </Routes>
        <Footer />
      </AuthProvider>
    </>
  );
}

export default App;
