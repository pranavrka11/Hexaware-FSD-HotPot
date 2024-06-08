import { createSlice } from "@reduxjs/toolkit";


const citySlice=createSlice({
    name: 'city',
    initialState: {selectedCity: 'Shirdi'},
    reducers:{
        changeCity(state, action){
            state.selectedCity=action.payload;
        },
    },
});

export const { changeCity } = citySlice.actions;
export default citySlice.reducer;