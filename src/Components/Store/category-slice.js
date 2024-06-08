import { createSlice } from "@reduxjs/toolkit";


const categorySlice = createSlice({
    name: 'category',
    initialState: { setCategory: 'all' },
    reducers: {
        changeCategory(state, action) {
            state.setCategory = action.payload;
        },
    },
});

export const { changeCategory } = categorySlice.actions;

export default categorySlice.reducer;