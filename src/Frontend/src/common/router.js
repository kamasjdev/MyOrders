import AbstractView from '../views/AbstractView';

const pathToRegex = (path) => new RegExp('^' + path.replace(/\//g, "\\/").replace(/:\w+/g, '(.+)') + "$");

const matchRoute = (path, route) => {
    const result = path.match(pathToRegex(route));

    if (!result) {
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
    const potentialMatches = validatedRoutes.map(route => {
        return {
            route,
            result: matchRoute(location.pathname, route.path)
        };
    });

    let match = potentialMatches.find(m => m.result !== null);

    if (!match) {
        window.location.href = '/'; // default on home page
        return;
    }

    const view = new match.route.view(getParams(match));
    document.querySelector('#app').innerHTML = await view.getHtml();
    dispatchEvent(new CustomEvent('pageLoaded', {
        detail: { page: view.constructor.name }
    }))
};

const validRoutes = (routes) => {
    if (!routes) {
        throw new Error('Routes cannot be null or underfined');
    }

    if (!(routes instanceof Array)) {
        throw new Error('Routes should be array');
    }

    if (routes.length === 0) {
        throw new Error('Routes cannot be empty');
    }

    let errors = '';
    for (const route of routes) {
        errors += validRoute(route);
    }

    if (errors) {
        throw new Error(errors);
    }
}

const validRoute = (route) => {
    if (!route) {
        return 'Element of route cannot be null or undefined. ';
    }

    if (!(route instanceof Object)) {
        return 'Element of route should be of type Object. ';
    }

    let errors = ''
    errors += validPath(route.path);
    errors += validView(route.view);

    return errors;
}

const validPath = (path) => {
    if (!path) {
        return `Element of route should contain property string 'path'. `;
    }

    if (!(typeof(path) === 'string')) {
        return `Element of route should contain property string 'path'. `;
    }

    return '';
}

const validView = (view) => {
    if (!view) {
        return `Element of route should contain property 'view' which extends ${AbstractView.name}. `;
    }

    if (!(view.prototype instanceof AbstractView)) {
        return `Element of route should contain property 'view' which extends ${AbstractView.name}. `;
    }

    return '';
}

let validatedRoutes = {};

export default function useRouter(routes) {
    validRoutes(routes);
    validatedRoutes = routes;

    window.addEventListener('popstate', () => { 
        router();
     }); // event is fired when the active history entry changes

    document.addEventListener('DOMContentLoaded', () => {
        document.body.addEventListener('click', e => {
            if (e.target.matches("[data-link]")) { // every element with attribute data-link will be matched
                e.preventDefault();
                navigateTo(e.target.href);
            }
        });

        router();
    });
}