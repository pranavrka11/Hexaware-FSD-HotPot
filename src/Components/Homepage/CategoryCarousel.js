import { useEffect, useRef, useState } from "react";
import axios from "axios";
import './Restaurant.css'
import Slider from "react-slick";
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { changeCategory } from "../Store/category-slice";

export default function CategoryCarousel() {
    const sliderRef = useRef();
    const navigate=useNavigate();
    const dispatch=useDispatch();

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

    const goToCategoryPage=(category)=>{
        // console.log("New category is: "+category);
        dispatch(changeCategory(category));
        navigate('/restaurants')
    }
    return (
        <>
            <div className="page-head">
                <h1>Choose from a wide variety of Cuisins</h1>
            </div>
            <Slider ref={sliderRef}{...settings}>
                <div className="col">
                    <div className="card text-center mb-3 h-100 card-res">
                        {/* <div className="card text-center mb-3 h-100 card-res"> */}
                        <img src="/indian.jpg" height="200" className="card-img-top" alt="Indian" />
                        <div className="card-body">
                            <h5 className="card-title">Indian</h5>
                            <p className="card-text">Known for its diverse flavors and spices, Indian cuisine offers a wide range of dishes from various regions, featuring aromatic spices, curries, and bread like naan and roti.</p>
                            <button className="btn btn-primary" onClick={()=>goToCategoryPage('Indian')}>Explore</button>
                        </div>
                        {/* </div> */}
                    </div>
                </div>
                <div className="col">
                    <div className="card text-center mb-3 h-100 card-res">
                        {/* <div className="card text-center mb-3 h-100 card-res"> */}
                        <img src="/italian.jpg" height="200" className="card-img-top" alt="Italian" />
                        <div className="card-body">
                            <h5 className="card-title">Italian</h5>
                            <p className="card-text">Characterized by its simplicity and variety, Italian cuisine emphasizes fresh, high-quality ingredients such as pasta, tomatoes, olive oil, and cheese, with dishes like pasta, pizza, and risotto</p>
                            <button className="btn btn-primary" onClick={()=>goToCategoryPage('Italian')}>Explore</button>
                        </div>
                        {/* </div> */}
                    </div>
                </div>
                <div className="col">
                    <div className="card text-center mb-3 h-100 card-res">
                        {/* <div className="card text-center mb-3 h-100 card-res"> */}
                        <img src="/southindian.jpg" height="200" className="card-img-top" alt="South Indian" />
                        <div className="card-body">
                            <h5 className="card-title">South Indian</h5>
                            <p className="card-text">Known for its use of rice, lentils, and coconut, South Indian cuisine includes dishes like dosa, idli, sambar, and rasam, offering a balance of flavors and textures</p>
                            <button className="btn btn-primary" onClick={()=>goToCategoryPage('South Indian')}>Explore</button>
                        </div>
                        {/* </div> */}
                    </div>
                </div>
                <div className="col">
                    <div className="card text-center mb-3 h-100 card-res">
                        {/* <div className="card text-center mb-3 h-100 card-res"> */}
                        <img src="/punjabi.jpg" height="200" className="card-img-top" alt="Indian" />
                        <div className="card-body">
                            <h5 className="card-title">Punjabi</h5>
                            <p className="card-text">Rich and robust, Punjabi cuisine is known for its use of dairy products, especially butter and cream, along with spices like cumin, coriander, and garam masala, with dishes like butter chicken, naan, and chole</p>
                            <button className="btn btn-primary" onClick={()=>goToCategoryPage('Punjabi')}>Explore</button>
                        </div>
                        {/* </div> */}
                    </div>
                </div>
                <div className="col">
                    <div className="card text-center mb-3 h-100 card-res">
                        {/* <div className="card text-center mb-3 h-100 card-res"> */}
                        <img src="/american.jpg" height="200" className="card-img-top" alt="Indian" />
                        <div className="card-body">
                            <h5 className="card-title">American</h5>
                            <p className="card-text">Reflecting a melting pot of cultures, American cuisine varies by region but often includes dishes like burgers, fried chicken, barbecue, and apple pie, known for its hearty and comforting flavors</p>
                            <button className="btn btn-primary" onClick={()=>goToCategoryPage('American')}>Explore</button>
                        </div>
                        {/* </div> */}
                    </div>
                </div>
            </Slider>
            <div className="carousel-controls">
                <button className="control-btn prev" onClick={handlePrev}>Prev</button>
                <button className="control-btn next" onClick={handleNext}>Next</button>
            </div>
        </>
    )
}