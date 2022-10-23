import AbstractView from "./AbstractView";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Products');
    }

    async getHtml() {
        return `
            <div class="container">
                <h1>Products</h1>
            </div>
        `;
    }
}