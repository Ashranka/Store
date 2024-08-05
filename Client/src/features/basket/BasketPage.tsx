import React from 'react';
import { Box, Button, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
import { Add, Delete, Remove } from '@mui/icons-material';
import { LoadingButton } from '@mui/lab';
import BasketSummary from './BasketSummary';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { addBasketItemAsync, removeBasketItemAsync } from './basketSlice';

export default function BasketPage() {
    const dispatch = useAppDispatch();
    const { basket, status } = useAppSelector(state => state.basket);

    if (!basket) return <Typography variant="h3">Your basket is empty</Typography>;

    return (
        <>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Product</TableCell>
                            <TableCell align="right">Price</TableCell>
                            <TableCell align="center">Quantity</TableCell>
                            <TableCell align="right">Subtotal</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {basket.items.map((item) => (
                            <TableRow
                                key={item.productId}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    <Box display='flex' alignItems='center'>
                                        <img style={{ height: 50, marginRight: 20 }} src={item.pictureUrl} alt={item.name} />
                                        <span>{item.name}</span>
                                    </Box>
                                </TableCell>
                                <TableCell align="right">${(item.price / 100).toFixed(2)}</TableCell>
                                <TableCell align="center">
                                    <LoadingButton
                                        color='error'
                                        loading={status === `pendingRemoveItem${item.productId}rem`}
                                        onClick={() => dispatch(removeBasketItemAsync({
                                            productId: item.productId, quantity: 1, name: 'rem'
                                        }))}
                                    >
                                        <Remove />
                                    </LoadingButton>
                                    {item.quantity}
                                    <LoadingButton
                                        loading={status === `pendingAddItem${item.productId}`}
                                        onClick={() => dispatch(addBasketItemAsync({ productId: item.productId }))}
                                        color='secondary'
                                    >
                                        <Add />
                                    </LoadingButton>
                                </TableCell>
                                <TableCell align="right">${((item.price / 100) * item.quantity).toFixed(2)}</TableCell>
                                <TableCell align="right">
                                    <LoadingButton
                                        loading={status === `pendingRemoveItem${item.productId}del`}
                                        onClick={() => dispatch(removeBasketItemAsync({
                                            productId: item.productId, quantity: item.quantity, name: 'del'
                                        }))}
                                        color='error'
                                    >
                                        <Delete />
                                    </LoadingButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Grid container>
                <Grid item xs={6}></Grid>
                <Grid item xs={6}>
                    <BasketSummary />
                    <Button
                        component={Link}
                        to={'/checkout'}
                        variant='contained'
                        size='large'
                        fullWidth
                    >
                        Checkout
                    </Button>
                </Grid>
            </Grid>
        </>
    );
}
