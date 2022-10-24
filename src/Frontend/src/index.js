import './styles/main.scss';
import Customers from './views/Customers';
import Home from './views/Home';
import NotFound from './views/NotFound';
import Products from './views/Products';
import ProductView from './views/ProductView';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import useRouter from './common/router';

const routes = [
    { path: '/', view: Home },
    { path: '/customers', view: Customers },
    { path: '/products', view: Products },
    { path: '/products/:id', view: ProductView },
    { path: '(.*)', view: NotFound } // every character will be matched
];

useRouter(routes);