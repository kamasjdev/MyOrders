import './styles/main.scss'

console.log('test');

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

document.addEventListener("DOMContentLoaded", () => {
    router();
});