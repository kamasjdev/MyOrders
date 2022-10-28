import './styles/main.scss';
import Customers from './views/Customers';
import Home from './views/Home';
import NotFound from './views/NotFound';
import Products from './views/Products';
import ProductView from './views/ProductView';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import useRouter from './common/router';
import CustomerView from './views/CustomerView';
import ProductKindAdd from './views/ProductKindAdd';
import ProductKindEdit from './views/ProductKindEdit';
import ProductKinds from './views/ProductKinds';
import AddToCartForCustomer from './views/AddToCartForCustomer';
import AddToCart from './views/AddToCart';
import Cart from './views/Cart';
import CartForCustomer from './views/CartForCustomer';
import OrderView from './views/OrderView';
import OrdersForCustomer from './views/OrdersForCustomer';
import Orders from './views/Orders';
import CustomerAdd from './views/CustomerAdd';
import CustomerEdit from './views/CustomerEdit';

const routes = [
    { path: '/', view: Home },
    { path: '/customers', view: Customers },
    { path: '/products', view: Products },
    { path: '/products/:id', view: ProductView },
    { path: '/customers/add', view: CustomerAdd },
    { path: '/customers/edit/:id', view: CustomerEdit },
    { path: '/customers/:id', view: CustomerView },
    { path: '/product-kinds', view: ProductKinds },
    { path: '/product-kinds/add', view: ProductKindAdd },
    { path: '/product-kinds/edit/:id', view: ProductKindEdit },
    { path: '/add-to-cart', view: AddToCart },
    { path: '/add-to-cart/:id', view: AddToCartForCustomer },
    { path: '/cart', view: Cart },
    { path: '/cart/:id', view: CartForCustomer },
    { path: '/orders', view: Orders },
    { path: '/orders/view/:id', view: OrderView },
    { path: '/orders/:id', view: OrdersForCustomer },
    { path: '(.*)', view: NotFound } // every character will be matched
];

useRouter(routes);