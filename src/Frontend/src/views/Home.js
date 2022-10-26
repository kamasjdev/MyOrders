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
            </div>
        `;
    }
}