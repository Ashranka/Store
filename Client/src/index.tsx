import '@fontsource/roboto/700.css';
import { RouterProvider } from 'react-router-dom';
import { router } from './app/router/Routes';
import { StoreProvider } from './app/context/StoreContext';
import React from 'react';
import ReactDOM from 'react-dom/client';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <StoreProvider>
            <RouterProvider router={router} />
        </StoreProvider>
    </React.StrictMode>,
)