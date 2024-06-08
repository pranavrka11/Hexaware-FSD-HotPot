import './Aboutstyles.css'

export default function Storycomponent() {
    return (
        <div className="container story-container">
            <div className="story-title">
                <h1>Our Story</h1>
            </div>
            <div className="story-main">
                <div className="story-image">
                    <img src="fimg1.jpg" width="500px" alt="" />
                </div>
                <div className="story-content">
                    <h3>Where innovation meets taste</h3>
                    <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Totam, saepe ducimus eius veritatis
                        asperiores, minima iure temporibus tempora aperiam impedit incidunt expedita quas voluptas ex
                        eveniet! Eius sequi aliquam ut?</p>
                    <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis quis amet vero consequuntur
                        asperiores nesciunt atque veniam id at quam repellat, enim in voluptatum perferendis, laudantium ut
                        omnis quisquam error.</p>
                </div>
            </div>
        </div>
    )
}