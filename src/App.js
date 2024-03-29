// import logo from './logo.svg';
import './App.css';
import RegistrationHeader from './Components/RestaurantOwner/RegistrationHeader';
import Registration from './Components/RestaurantOwner/Registration';
import RegistrationFooter from './Components/RestaurantOwner/RegistrationFooter';
import CustomerRegistration from './Components/Customer/CustomerRegistration';
import MenuHeader from './Components/Menu/MenuHeader';
import MenuCategories from './Components/Menu/MenuCategories';
import Menu from './Components/Menu/Menu';
import Profile from './Components/Profile/Profile';
import OwnerProfile from './Components/OwnerProfile/OwnerProfile';
import Checkout from './Components/Checkout/Checkout';
import Confirmation from './Components/Confirmation/Confirmation';
import Admin from './Components/Admin/Admin';




function App() {
  return (
    <div className="App">
      <Admin />
      
      {/* <RegistrationHeader />
      <Registration />
      <RegistrationFooter /> */}
      
      {/* <RegistrationHeader />
      <CustomerRegistration />
      <RegistrationFooter /> */}

      {/* <MenuHeader/> */}
      {/* <MenuCategories /> */}
      {/* <Menu /> */}
      {/* <RegistrationFooter /> */}

      {/* <Profile /> */}

      {/* <OwnerProfile /> */}

      {/* <Checkout /> */}

      {/* <Confirmation /> */}
    </div>
  );
}

export default App;
