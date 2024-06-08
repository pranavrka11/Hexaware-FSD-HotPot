import { createSlice } from "@reduxjs/toolkit";

const token = sessionStorage.getItem('Token');

const authSlice = createSlice({
    name: 'auth',
    initialState:{
        isLoggedIn:!!token
    },
    reducers: {
        login(state) {
            state.isLoggedIn = true;
            // console.log("Login executed "+state.isLoggedIn);
        },
        logout(state) {
            state.isLoggedIn = false;
            // console.log("Logout executed "+state.isLoggedIn);
        },
    },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;