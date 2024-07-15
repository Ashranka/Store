import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
import {useEffect, useState} from "react";


export default function Catalog() {

    const [products, setProducts] = useState<Product[]>([]);

    useEffect(() => {
        fetch('https://localhost:7005/api/Products')
            .then(response => response.json())
            .then(data => setProducts(data))
    },[])



    return (
        <>
            <ProductList products={products} />
        </>
    );
}
