import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon.js";
import { navigateTo } from "../common/router";
import AbstractView from "./AbstractView";

export default class ProductView extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Product View');
        this.product = null;
        this.loading = true;
    }

    backToProducts = (e) => {
        if (e.target.matches("#back-to-products")) { // every element with attribute data-link will be matched
            e.preventDefault();
            navigateTo('/products');
        }
    }

    onDestroy() {
        document.body.removeEventListener('click', this.backToProducts, false);
    }

    async created() {
        document.body.addEventListener('click', this.backToProducts, false);
        const response = await axios.get(`api/products/${this.params.id}`);
        this.product = response.data;
        this.loading = false;
    }

    async getHtml() {
        return `
            <div class="containerBox">
                <h1>Product ${this.params.id}</h1>
                ${this.loading ? loadingIcon() : `
                <div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Product Name:
                        </div>
                        <div>
                            ${this.product.productName}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Product Kind:
                        </div>
                        <div>
                            ${this.product.productKind.productKindName}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Price:
                        </div>
                        <div>
                            ${this.product.price} $
                        </div>
                    </div>
                </div>`}
                <button id="back-to-products" class="mt-4 btn btn-primary">Back to Home</button>
            </div>
        `;
    }
}