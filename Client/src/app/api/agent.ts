// src/app/api/agent.ts
import axios, {AxiosError, AxiosResponse} from 'axios';
import {toast} from "react-toastify";

// Configuración predeterminada de axios
axios.defaults.baseURL = 'https://localhost:7005/api/';

// Función para extraer el cuerpo de la respuesta
const responseBody = (response: AxiosResponse) => response.data;
axios.defaults.withCredentials = true;

axios.interceptors.response.use(response =>{
    return response
}, (error: AxiosError) => {
    const {data, status} = error.response as AxiosResponse;
    switch (status){
        case 400:
            toast.error(data.title);
            break;

        case 401:
            toast.error(data.title);
            break;

        case 500:
            toast.error(data.title);
            break;
        default:
            break;
    }
})

// Objeto request para manejar diferentes tipos de solicitudes HTTP
const requests = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    del: (url: string) => axios.delete(url).then(responseBody),
};

const Catalog = {
    list: () => requests.get('Products'),
    details: (id: number) => requests.get(`Products/${id}`)
};

const TestErrors = {
    get400Error: () => requests.get('buggy/bad-request'),
    get401Error: () => requests.get('buggy/unauthorised'),
    get404Error: () => requests.get('buggy/not-found'),
    get500Error: () => requests.get('buggy/server-error'),
    getValidationError: () => requests.get('buggy/validation-error')
};


const Basket = {
    get: () => requests.get('basket'),
    addItem: (productId: number, quantity = 1) => requests.post(`basket?productId=${productId}&quantity=${quantity}`, {}),
    removeItem: (productId: number, quantity = 1) => requests.del(`basket?productId=${productId}&quantity=${quantity}`)
}
const agent = {
    Catalog,
    TestErrors,
    Basket
};

export default agent;
