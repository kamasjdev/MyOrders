import AbstractView from "./AbstractView";

export default class Home extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Home');
    }

    async getHtml() {
        return `
            <div class="containerBox">
                <h1>Welcome to the MyOrders App</h1>
                <h2>Are this links below are rendered?</h2>
                <h3>
                    <a href="/" data-link>
                        Home
                    </a>
                </h3>
                <h3>
                    <a href="/customers" data-link>
                        Customers
                    </a>
                </h3>
                <h3>
                    <a href="/products" data-link>
                        Products
                    </a>
                </h3>
            </div>
        `;
    }
}