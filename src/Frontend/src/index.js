import './styles/main.scss'

console.log('test');
console.log(history);

const navigateTo = (url) => {
    history.pushState(null, null, url);
    router();
}

const router = async () => {
    const routes = [
        { path: '/', view: () => console.log('Viewing Home Page') },
        { path: '/customers', view: () => console.log('Viewing Customers Page') },
        { path: '/products', view: () => console.log('Viewing Products Page') },
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
            route: routes[0],
            isMatch: true
        };
    }

    console.log(match.route.view());
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