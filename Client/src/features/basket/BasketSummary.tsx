import React from 'react';
import { Typography, List, ListItem, ListItemText } from '@mui/material';
import { useAppSelector } from '../../app/store/configureStore';

const BasketSummary = () => {
    const { basket } = useAppSelector(state => state.basket);

    const subtotal = basket?.items.reduce((sum, item) => sum + (item.price * item.quantity), 0) || 0;

    return (
        <>
            <Typography variant="h6">Order Summary</Typography>
            <List>
                <ListItem>
                    <ListItemText primary="Subtotal" />
                    <Typography variant="body2">${(subtotal / 100).toFixed(2)}</Typography>
                </ListItem>
                {/* Add other summary details as needed */}
            </List>
        </>
    );
};

export default BasketSummary;
