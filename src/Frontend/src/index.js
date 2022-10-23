import './styles/main.scss';
import Customers from './views/Customers';
import Home from './views/Home';
import NotFound from './views/NotFound';
import Products from './views/Products';
import ProductView from './views/ProductView';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";

const pathToRegex = (path) => new RegExp('^' + path.replace(/\//g, "\\/").replace(/:\w+/g, '(.+)') + "$");

const matchRoute = (path, route) => {
    const result = path.match(pathToRegex(route));

    if (!result) {
        return null;
    }

    const values = result.slice(1);
    // block not included params included in url
    if (values.some(v => v.includes('/'))) {
        return null;
    }

    return result;
}

const getParams = (match) => {
    const result = match.result;

    if (!result) {
        return {};
    }

    const values = match.result.slice(1); // gets all start index 1
    const keys = Array.from(match.route.path.matchAll(/:(\w+)/g)).map(result => result[1]);

    return Object.fromEntries(keys.map((key, i) => {
        return [key, values[i]]
    }));
};

const navigateTo = (url) => {
    history.pushState(null, null, url);
    router();
}

const router = async () => {
    const routes = [
        { path: '/', view: Home },
        { path: '/customers', view: Customers },
        { path: '/products', view: Products },
        { path: '/products/:id', view: ProductView },
        { path: '/not-found', view: NotFound }
    ];
    
    const potentialMatches = routes.map(route => {
        return {
            route,
            result: matchRoute(location.pathname, route.path)
        };
    });

    let match = potentialMatches.find(m => m.result !== null);

    if (!match) {
        match = {
            route: routes[routes.length - 1],
            result: location.pathname
        };
    }

    const view = new match.route.view(getParams(match));
    document.querySelector('#app').innerHTML = await view.getHtml();
};

window.addEventListener('popstate', router); // event is fired when the active history entry changes

document.addEventListener('DOMContentLoaded', () => {
    document.body.addEventListener('click', e => {
        if (e.target.matches("[data-link]")) { // every element with attribute data-link will be matched
            e.preventDefault();
            navigateTo(e.target.href);
        }
    })

    router();
});