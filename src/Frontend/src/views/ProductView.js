import AbstractView from "./AbstractView";

export default class ProductView extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Products');
    }

    async getHtml() {
        return `
            <div class="containerBox">
                <h1>Product ${this.params.id}</h1>
                <a href="/" class="btn btn-primary" data-link>Back to Home</a>
            </div>
        `;
    }
}