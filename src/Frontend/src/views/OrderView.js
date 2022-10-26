import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon.js";
import { navigateTo } from "../common/router.js";
import AbstractView from "./AbstractView";

export default class OrderView extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Order View');
        this.order = null;
        this.loading = true;
    }

    async created() {
        document.body.addEventListener('click', e => {
            if (e.target.matches("#back-to-orders")) {
                e.preventDefault();
                navigateTo('/orders');
            }
        });
        const response = await axios.get(`api/orders/${this.params.id}`);
        this.order = response.data;
        this.loading = false;
    }

    async getHtml() {
        const showOrderItems = () => {
            let html = '';
            for (const orderItem of this.order.orderItems) {
                html += `<div class="d-flex justify-content-center mt-2">
                            <div class="me-4">
                                Product Name:
                            </div>
                            <div>
                                ${orderItem.product.productName}
                            </div>
                        </div>
                        <div class="d-flex justify-content-center">
                            <div class="me-4">
                                Price:
                            </div>
                            <div>
                                ${orderItem.product.price}
                            </div>
                        </div>`
            }

            return html;
        }

        return `
            <div class="containerBox">
                <h1>Order ${this.params.id}</h1>
                ${this.loading ? loadingIcon() : `
                <div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Order Number:
                        </div>
                        <div>
                            ${this.order.orderNumber}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Price:
                        </div>
                        <div>
                            ${this.order.price} $
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Created:
                        </div>
                        <div>
                            ${this.order.created}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Modified:
                        </div>
                        <div>
                            ${this.order.modified}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            First Name:
                        </div>
                        <div>
                            ${this.order.customer.firstName}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Last Name:
                        </div>
                        <div>
                            ${this.order.customer.lastName}
                        </div>
                    </div>
                    <h4 class="mt-4">Order Items</h4>
                    ${this.order.orderItems ? showOrderItems() : ''}
                </div>`}
                <button id="back-to-orders" class="mt-4 btn btn-primary">Back to Orders</button>
            </div>
        `;
    }
}