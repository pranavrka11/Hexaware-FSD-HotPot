import Carousel2 from "./Carousel2";
import CategoryCarousel from "./CategoryCarousel";
import RestaurantCarousel from "./RestaurantCarousel";
import RestaurantNav from "./RestaurantNav";

export default function HomePage() {
    return (
        <>
            <RestaurantCarousel />
            <Carousel2 />
            <CategoryCarousel />
        </>
    )
}