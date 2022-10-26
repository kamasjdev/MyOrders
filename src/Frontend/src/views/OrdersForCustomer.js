import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router.js";
import AbstractView from "./AbstractView";

export default class OrdersForCustomer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Orders');
        this.loading = true;
        this.orders = [];
    }

    deleteProductKind = (id) => {
        axios.delete(`api/orders/${id}`)
        .then(_ => {
            navigateTo('/orders');
        });
    }

    async created() {
        const orders = await this.fetchOrders();
        this.orders = orders;
        this.loading = false;
        document.addEventListener('click', (event) => {
            for (const order of orders) {
                if (event.target.matches(`#delete-order-${order.id}`)) {
                    const dialogEl = document.querySelector('dialog');
                    event.preventDefault();
                    document.querySelector('#content').innerHTML = `Do you wish to delete Order with id ${order.id}?`;
                    dialogEl.showModal();
                    const yesBtn = document.querySelector('.yes');
                    yesBtn.onclick = () => {
                        dialogEl.close();
                        this.deleteProductKind(order.id);
                    };
                    const noBtn = document.querySelector('.no');
                    noBtn.onclick = () => {
                        dialogEl.close();
                    };
                }
            }
        });
    }

    async getHtml() {
        const returnBodyHtml = (o) => {
            let html = '';
            for (const order of o) {
                html += '<tr>';
                
                for (const field in order) {
                    html += '<td>';
                    html += order[field]
                    html += '</td>';
                }
                html += `<td>
                            <a href="/orders/view/${order.id}" class="btn btn-primary me-2" data-link>View</a>
                            <button id="delete-order-${order.id}" class="btn btn-danger" type="button">Delete</button>
                        </td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Orders</h1>
                ${this.loading ? loadingIcon() : 
                `<div class="d-flex justify-content-start mb-2">
                    <a class="btn btn-success" href="/orders/add" data-link>Add</a>
                </div>
                <table class="table table">
                    <thead>
                        <th>
                            Id
                        </th>
                        <th>
                            Order Number
                        </th>
                        <th>
                            Price
                        </th>
                        <th>
                            Created
                        </th>
                        <th>
                            Modified
                        </th>
                        <th>
                            Action
                        </th>
                    </thead>
                    <tbody>
                        ${returnBodyHtml(this.orders)}
                    </tbody>
                </table>`}
                <dialog>
                    <div id="content">
                    </div>
                    <button class="yes btn btn-danger">Yes</button>
                    <button class="no btn btn-secondary">No</button>
                </dialog>
            </div>
        `;
    }

    async fetchOrders() {
        const response = await axios.get(`api/orders/by-customer/${this.params.id}`);
        return response.data;
    }
}