import './Aboutstyles.css'

export default function Pageheader(){
    return(
        <div id="carouselExampleSlidesOnly" className="carousel slide" data-bs-ride="carousel">
            <div className="carousel-inner">
                <div className="carousel-item active">
                    <img src="fimg6.jpg" height="700vh" className="d-block w-100" alt="..." />
                    <div className="carousel-content">
                        <h1>About Us</h1>
                    </div>
                </div>
            </div>
        </div>
    )
}