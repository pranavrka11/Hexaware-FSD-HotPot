import { configureStore } from "@reduxjs/toolkit";
import authReducer from './auth-slice';
import cityReducer from './city-slice'
import categoryReducer from './category-slice'
import siteauthorityReducer from './siteauthority-slice'

const store = configureStore({
    reducer: {
        auth: authReducer,
        city: cityReducer,
        category: categoryReducer,
        siteauth: siteauthorityReducer
    }
});

export default store;