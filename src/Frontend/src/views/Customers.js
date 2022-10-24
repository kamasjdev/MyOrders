import AbstractView from "./AbstractView";

export default class Customer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Customers');
    }

    async getHtml() {
        return `
            <div class="containerBox">
                <h1>Customers</h1>
            </div>
        `;
    }
}