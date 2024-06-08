import { createSlice } from "@reduxjs/toolkit";


const partnerToken = sessionStorage.getItem('DeliveryPartnerToken');
const ownerToken = sessionStorage.getItem('RestaurantOwnerToken');


const siteauthoritySlice=createSlice({
    name: 'siteauth',
    initialState:{authLoggedIn: partnerToken || ownerToken},
    reducers:{
        siteAuthLogin(state){
            state.authLoggedIn=true;
        },
        siteAuthLogout(state){
            state.authLoggedIn=false;
        },
    },
});

export const {siteAuthLogin, siteAuthLogout} =siteauthoritySlice.actions;
export default siteauthoritySlice.reducer;

