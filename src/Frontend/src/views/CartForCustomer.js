import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router";
import AbstractView from "./AbstractView";

export default class CartForCustomer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Cart');
        this.loading = true;
        this.orderItems = [];
    }

    async created() {
        const orderItems = await this.fetchOrderItems();
        this.orderItems = orderItems;
        this.loading = false;
        document.addEventListener('click', async (event) => {
            for (const orderItem of this.orderItems) {
                if (event.target.matches(`#cart-${orderItem.id}`)) {
                    await this.deleteFromCart(orderItem);
                }
            }

            if (event.target.matches('#accept-order')) {
                await this.createOrder();
            }
        })
    }

    async deleteFromCart(orderItem) {
        await axios.delete(`api/order-items/${orderItem.id}`);
        location.reload();
    }

    async createOrder() {
        const response = await axios.post('api/orders', {
            customerId: this.params.id,
            orderItemIds: this.orderItems.map(oi => oi.id)
        });
        navigateTo(`/orders/view/${response.data.id}`);
    }

    calculateTotalPrice() {
        let total = 0;
        for (const order of this.orderItems) {
            total += order.product.price;
        }
        return total;
    }

    async getHtml() {
        const returnBodyHtml = (oi) => {
            let html = '';
            for (const orderItem of oi) {
                html += '<tr>';
                html += '<td>';
                html += orderItem.id
                html += '</td>';
                html += '<td>';
                html += orderItem.product.productName
                html += '</td>';
                html += '<td>';
                html += orderItem.product.price
                html += '</td>';
                html += '<td>';
                html += orderItem.created
                html += '</td>';
                html += `<td><button id="cart-${orderItem.id}" type="button" class="btn btn-danger" data-link>Delete</button></td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Cart for customer ${this.params.id}</h1>
                ${this.loading ? LoadingIcon() : 
                `<table class="table table">
                    <thead>
                        <th>
                            Id
                        </th>
                        <th>
                            Product Name
                        </th>
                        <th>
                            Price
                        </th>
                        <th>
                            Created
                        </th>
                        <th>
                            Action
                        </th>
                    </thead>
                    <tbody>
                        ${returnBodyHtml(this.orderItems)}
                    </tbody>
                </table>
                <div>
                    <div>
                        Summary Price:
                    </div>
                    <div>
                        ${this.calculateTotalPrice()}
                    </div>
                </div>
                <div>
                    <button id="accept-order" class="btn btn-success mt-2">Accept Order</button>
                </div>
                `}
            </div>
        `;
    }

    async fetchOrderItems() {
        const response = await axios.get(`api/order-items/not-ordered?customerId=${this.params.id}`);
        return response.data;
    }
}