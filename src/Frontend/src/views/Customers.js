import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon";
import AbstractView from "./AbstractView";

export default class Customer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Customers');
        this.loading = true;
        this.customers = [];
    }

    async created() {
        const customers = await this.fetchCustomers();
        this.customers = customers;
        this.loading = false;
    }

    async getHtml() {
        const returnBodyHtml = (c) => {
            let html = '';
            for (const customer of c) {
                html += '<tr>';
                
                for (const field in customer) {
                    html += '<td>';
                    html += customer[field]
                    html += '</td>';
                }
                html += `<td><a href="/customers/${customer.id}" class="btn btn-primary" data-link>Show Details</a></td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Customers</h1>
                ${this.loading ? loadingIcon() : 
                `<table class="table table">
                    <thead>
                        <th>
                            Id
                        </th>
                        <th>
                            Price
                        </th>
                        <th>
                            Product Name
                        </th>
                        <th>
                            Action
                        </th>
                    </thead>
                    <tbody>
                        ${returnBodyHtml(this.customers)}
                    </tbody>
                </table>`}
            </div>
        `;
    }

    async fetchCustomers() {
        const response = await axios.get('api/customers');
        return response.data;
    }
}