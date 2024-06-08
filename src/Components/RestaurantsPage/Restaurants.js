import './Restaurantpagestyles.css'
import { useEffect, useState } from 'react';
import axios from "axios";
import { Link, useParams } from 'react-router-dom';
import { useSelector } from 'react-redux';

export default function Restaurants() {
    var [restaurants, setRestaurants] = useState([{
        "restaurantId": 0,
        "restaurantName": "",
        "email": "",
        "phone": "",
        "restaurantImage": ""
    }]);

    var [filteredRestaurants, setFilteredRestaurants] = useState([{
        "restaurantId": 0,
        "restaurantName": "",
        "email": "",
        "phone": "",
        "restaurantImage": ""
    }]);

    var [allReviews, setAllReviews] = useState([{
        "reviewId": 0,
        "customerId": 0,
        "restaurantId": 0,
        "rating": 0,
        "textReview": "",
    }]);

    // const customCuisine=useParams();
    const setCategory = useSelector((state) => state.category.setCategory);
    const [categories, setCategories] = useState([]);
    //http://localhost:5249/api/Restaurant/GetSpecialities

    var [searchedRestaurants, setSearchedRestaurants] = useState([]);

    const [searchQuery, setSearchQuery] = useState('');
    const [selectedCategory, setSelectedCategory] = useState(setCategory);


    const selectedCity = useSelector((state) => state.city.selectedCity);
    // const restoURL = `http://localhost:5249/api/Customer/GetRestaurantsByCity?city=${city}`
    // useEffect(() => {
    //     var data = axios.get(restoURL)
    //         .then(function (response) {
    //             setRestaurants(response.data);
    //         })
    //         .catch(function (error) {
    //             console.log(error);
    //         });
    // }, [restaurants]);
    useEffect(() => {
        const restoURL = `http://localhost:5249/api/Customer/GetRestaurantsByCity?city=${selectedCity}`;
        axios.get(restoURL)
            .then(function (response) {
                setRestaurants(response.data);
                setFilteredRestaurants(response.data);
                // console.log(restaurants);
            })
            .catch(function (error) {
                console.log(error);
            });
    }, [selectedCity, setRestaurants, setFilteredRestaurants]);

    //fetching categories
    useEffect(() => {
        axios.get('http://localhost:5249/api/Restaurant/GetSpecialities')
            .then(function (response) {
                setCategories(response.data);
            })
            .catch(function (error) {
                console.log(error);
            });
    }, []);

    //filtering restaurants based on search query
    useEffect(() => {
        if (searchQuery.trim() !== '') {
            const searchResults = restaurants.filter(restaurant => restaurant.restaurantName.toLowerCase().includes(searchQuery.toLowerCase()));
            setSearchedRestaurants(searchResults);
        }
        else {
            setSearchedRestaurants([]);
        }
    }, [searchQuery, restaurants]);

    //filtering restaurants based on cuisine/speciality
    useEffect(() => {
        if (selectedCategory === 'all') {
            setFilteredRestaurants(restaurants);
        }
        else {
            const specialityRestos = categories.filter(category => category.categoryName.toLowerCase() === selectedCategory.toLowerCase());
            const specialityRestoIds = specialityRestos.map(r => r.restaurantId);
            const restos = restaurants.filter(restaurant => specialityRestoIds.includes(restaurant.restaurantId));
            // console.log(restos);
            setFilteredRestaurants(restos);
            // console.log(filteredRestaurants);
            // const filteredItems=
        }
    }, [selectedCategory, restaurants]);

    // useEffect(() => {
    //     const interval = setInterval(() => {
    //         const storedCity = sessionStorage.getItem('selectedCity');
    //         if (storedCity !== selectedCity) {
    //             setSelectedCity(storedCity);
    //             console.log("Stored city "+storedCity);
    //             console.log("Selected city: "+selectedCity);
    //         }
    //     }, 1000); // Check every second

    //     return () => clearInterval(interval); // Clean up interval on unmount
    // }, []);

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value);
    };

    const handleCategoryChange = (e) => {
        setSelectedCategory(e.target.value);
        // console.log("New category: "+selectedCategory);
    };

    //fetching all the reviews
    useEffect(() => {
        axios.get('http://localhost:5249/api/Restaurant/GetAllReviews')
            .then(function (response) {
                setAllReviews(response.data);
            })
            .catch(function (error) {
                console.log(error)
            });
    }, []);

    // const [averageRating, setAverageRating] = useState(0);
    //caluclating rating for each restaurant

    const fetchAndCalculateRating = (restaurantId) => {
        const ratingsForRestaurant = allReviews.filter(review => review.restaurantId === restaurantId);
        const totalRating = ratingsForRestaurant.reduce((acc, curr) => acc + curr.rating, 0);
        const avgRating = totalRating / ratingsForRestaurant.length;
        return avgRating;
        // setAverageRating(avgRating);
    }

    const averageRatings = {};
    restaurants.forEach(restaurant => {
        averageRatings[restaurant.restaurantId] = fetchAndCalculateRating(restaurant.restaurantId);
    });


    return (
        <div className="container restaurant-cards">
            <div className="cards-title">
                <h1>Select from a wide variety of Restaurants and Cuisins</h1>
            </div>
            <div className='restaurant-controller'>
                <nav className="navbar bg-body-tertiary controller-navbar">
                    <div className="container-fluid controller-content">
                        <h3>Filter Restaurants</h3>
                        <div className='cuisine-filter'>
                            <h4>Select Cuisine: </h4>
                            <select className='select-cuisine' value={selectedCategory} onChange={handleCategoryChange}>
                                <option value="all">All</option>
                                <option value="Indian">Indian</option>
                                <option value="South Indian">South Indian</option>
                                <option value="Italian">Italian</option>
                                <option value="Punjabi">Punjabi</option>
                                <option value="American">American</option>
                            </select>
                        </div>
                        <form className="d-flex" role="search">
                            <input className="form-control me-2" type="search" placeholder="Search Restaurants" aria-label="Search" value={searchQuery} onChange={handleSearchChange} />
                        </form>
                    </div>
                </nav>
            </div>
            <div className="cards-display">
                {searchQuery !== '' ? (
                    <div className='row row-cols-1 row-cols-md-3 g-4'>
                        {searchedRestaurants.map((restaurant) =>
                            <div className="col" key={restaurant.restaurantId}>
                                <div className="card text-center h-100">
                                    <img src={`${process.env.PUBLIC_URL}/${restaurant.restaurantImage}`} height="200px" className="card-img-top" alt="..." />
                                    <div className="card-body">
                                        <h5 className="card-title">{restaurant.restaurantName}</h5>
                                        <p className="card-text"><h6>Categories:</h6>
                                            {categories.map((category) => {
                                                if (category.restaurantId === restaurant.restaurantId) {
                                                    return <p className='resto-category-label'>{category.categoryName}</p>
                                                }
                                            })}
                                        </p>
                                        <Link to={`/menus/${restaurant.restaurantId}`} className="btn btn-primary">View Menu</Link>
                                        <p className='rating-display'><i class="bi bi-star-fill"></i> {averageRatings[restaurant.restaurantId].toFixed(1)}/5</p>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                ) : (
                    // <div>
                    //     <h2>No Restaurants found</h2>
                    // </div>
                    null
                )}
                {searchQuery === '' && (
                    <div className="row row-cols-1 row-cols-md-3 g-4">
                        {Array.isArray(filteredRestaurants) && filteredRestaurants.map((restaurant) =>
                            <div className="col" key={restaurant.restaurantId}>
                                <div className="card text-center h-100">
                                    <img src={`${process.env.PUBLIC_URL}/${restaurant.restaurantImage}`} height="200px" className="card-img-top" alt="..." />
                                    <div className="card-body">
                                        <h4 className="card-title">{restaurant.restaurantName}</h4>
                                        <p className="card-text"><h6>Categories:</h6>
                                            {categories.map((category) => {
                                                if (category.restaurantId === restaurant.restaurantId) {
                                                    return <p className='resto-category-label'>{category.categoryName}</p>
                                                }
                                            })}</p>
                                        <Link to={`/menus/${restaurant.restaurantId}`} className="btn btn-primary">View Menu</Link>

                                        <p className='rating-display'><i class="bi bi-star-fill"></i> {averageRatings[restaurant.restaurantId].toFixed(1)}/5</p>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    )
}