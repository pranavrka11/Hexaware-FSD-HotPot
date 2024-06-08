import { useEffect, useRef, useState } from "react";
import axios from "axios";
import './Restaurant.css'
import Slider from "react-slick";
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";

function Carousel2() {
    var [restaurants, setRestaurants] = useState([{
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

    const [categories, setCategories] = useState([]);

    const sliderRef = useRef();

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

    // const [selectedCity, setSelectedCity] = useState(() => {
    //     const storedCity = sessionStorage.getItem('selectedCity');
    //     return storedCity || 'Pune';
    // });

    const selectedCity = useSelector((state) => state.city.selectedCity);

    // useEffect(() => {
    //     const handleStorageChange = (event) => {
    //         console.log('Storage event triggered:', event);
    //         if (event.key === 'selectedCity') {
    //             const newCity = sessionStorage.getItem('selectedCity') || 'pune';
    //             console.log('New city from storage:', newCity);
    //             setSelectedCity(newCity);
    //         }
    //     };

    //     window.addEventListener('storage', handleStorageChange);

    //     return () => {
    //         window.removeEventListener('storage', handleStorageChange);
    //     };
    // }, []);

    useEffect(() => {
        const restoURL = `http://localhost:5249/api/Customer/GetRestaurantsByCity?city=${selectedCity}`;
        axios.get(restoURL)
            .then(function (response) {
                setRestaurants(response.data);
                // console.log(restaurants);
            })
            .catch(function (error) {
                console.log(error);
            });
    }, [selectedCity, restaurants]); // Update when selectedCity changes

    // useEffect(() => {
    //     const storedCity = sessionStorage.getItem('selectedCity');
    //     if (storedCity && storedCity !== selectedCity) {
    //         setSelectedCity(storedCity);
    //     }
    // });

    // useEffect(() => {
    //     const interval = setInterval(() => {
    //         const storedCity = sessionStorage.getItem('selectedCity');
    //         if (storedCity !== selectedCity) {
    //             setSelectedCity(storedCity);
    //             // console.log("Stored city "+storedCity);
    //             // console.log("Selected city: "+selectedCity);
    //         }
    //     }, 1000); // Check every second

    //     return () => clearInterval(interval); // Clean up interval on unmount
    // }, []);

    // const [selectedCity, setSelectedCity] = useState(() => {
    //     const storedCity = sessionStorage.getItem('selectedCity');
    //     return storedCity || 'pune';
    // });

    // useEffect(() => {
    //     const fetchRestaurants = async () => {
    //         const restoURL = `http://localhost:5249/api/Customer/GetRestaurantsByCity?city=${selectedCity}`;
    //         try {
    //             const response = await axios.get(restoURL);
    //             setRestaurants(response.data);
    //         } catch (error) {
    //             console.log(error);
    //         }
    //     };

    //     fetchRestaurants();
    // }, [selectedCity]);

    // useEffect(() => {
    //     const storedCity = sessionStorage.getItem('selectedCity');
    //     if (storedCity && storedCity !== selectedCity) {
    //         setSelectedCity(storedCity);
    //     }
    // });    

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
        <>
            <div className="page-head">
                <h1>Restaurants Nearby</h1>
            </div>

            <Slider ref={sliderRef}{...settings}>

                {Array.isArray(restaurants) && restaurants.map((restaurant) =>
                    <div className="col">
                        <div className="card text-center mb-3 h-100 card-res" key={restaurant.restaurantId}>
                            {/* <div className="card text-center mb-3 h-100 card-res"> */}
                            <img src={restaurant.restaurantImage} height="200" className="card-img-top" alt="..." />
                            <div className="card-body">
                                <h5 className="card-title">{restaurant.restaurantName}</h5>
                                <div className="card-text"><h6>Categories:</h6>
                                    {categories.map((category) => {
                                        if (category.restaurantId === restaurant.restaurantId) {
                                            return <div className='resto-category-label'>{category.categoryName}</div>
                                        }
                                    })}</div>
                                <Link to={`/menus/${restaurant.restaurantId}`} className="btn btn-primary">View Menu</Link>
                                <p className='rating-display'><i className="bi bi-star-fill"></i> {averageRatings[restaurant.restaurantId].toFixed(1)}/5</p>
                            </div>
                            {/* </div> */}
                        </div>
                    </div>
                )}

            </Slider>
            {/* <div className="card text-center mb-3 h-100 card-res">
                <img src="resimg2.jpg" height="200" className="card-img-top" alt="..." />
                <div className="card-body">
                    <h5 className="card-title">Express Snack House</h5>
                    <p className="card-text">Some quick example text to build on the card title and make up
                        the bulk of
                        the
                        card's content.</p>
                    <a href="#" className="btn btn-primary">View Menu</a>
                </div>
            </div>

            <div className="card text-center mb-3 h-100 card-res">
                <img src="resimg3.jpg" height="200" className="card-img-top" alt="..." />
                <div className="card-body">
                    <h5 className="card-title">Shahi Biryani</h5>
                    <p className="card-text">Some quick example text to build on the card title and make up
                        the bulk of
                        the
                        card's content.</p>
                    <a href="#" className="btn btn-primary">View Menu</a>
                </div>
            </div> */}

            <div className="carousel-controls">
                <button className="control-btn prev" onClick={handlePrev}>Prev</button>
                <button className="control-btn next" onClick={handleNext}>Next</button>
            </div>

        </>
    )
}

export default Carousel2;