import './styles/main.scss';
import Customers from './views/Customers';
import Home from './views/Home';
import NotFound from './views/NotFound';
import Products from './views/Products';

const navigateTo = (url) => {
    history.pushState(null, null, url);
    router();
}

const router = async () => {
    const routes = [
        { path: '/', view: Home },
        { path: '/customers', view: Customers },
        { path: '/products', view: Products },
    ];

    const potentialMatches = routes.map(route => {
        return {
            route,
            isMatch: location.pathname === route.path
        };
    });

    let match = potentialMatches.find(m => m.isMatch);

    if (!match) {
        match = {
            route: { path: '/not-found', view: NotFound },
            isMatch: true
        };
    }

    const view = new match.route.view();
    const html = await view.getHtml();
    console.log(html);
    document.querySelector('#app').innerHTML = html;
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