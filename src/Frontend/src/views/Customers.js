import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon";
import AbstractView from "./AbstractView";

let _object = null;

export default class Customer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Customers');
        this.loading = true;
        this.customers = [];
    }

    assignDeleteButtons(event) {
        for (const customer of _object.customers) {
            if (event.target.matches(`#delete-customer-${customer.id}`)) {
                const dialogEl = document.querySelector('dialog');
                event.preventDefault();
                document.querySelector('#content').innerHTML = `Do you wish to delete Order with id ${customer.id}?`;
                dialogEl.showModal();
                const yesBtn = document.querySelector('.yes');
                yesBtn.onclick = async () => {
                    dialogEl.close();
                    _object.deleteCustomer(customer.id);
                };
                const noBtn = document.querySelector('.no');
                noBtn.onclick = () => {
                    dialogEl.close();
                };
            }
        }
    }

    afterCreateView() {
        document.addEventListener('click', this.assignDeleteButtons, false);
    }

    onDestroy() {
        document.removeEventListener('click', this.assignDeleteButtons, false);
        _object = null;
    }

    async deleteCustomer(id) {
        await axios.delete(`api/customers/${id}`);
        const customers = await this.fetchCustomers();
        this.customers = customers;
        this.forceUpdateView();
    }

    async created() {
        const customers = await this.fetchCustomers();
        this.customers = customers;
        this.loading = false;
        _object = this;
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
                html += `<td>
                            <a href="/customers/${customer.id}" class="btn btn-primary" data-link>Show Details</a>
                            <a href="/customers/edit/${customer.id}" class="btn btn-warning" data-link>Edit</a>
                            <button id="delete-customer-${customer.id}" class="btn btn-danger" type="button">Delete</button>
                        </td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Customers</h1>
                ${this.loading ? loadingIcon() : 
                `<div class="d-flex justify-content-start mb-2">
                    <a class="btn btn-success" href="/customers/add" data-link>Add</a>
                </div>
                <table class="table table">
                    <thead>
                        <th>
                            Id
                        </th>
                        <th>
                            First Name
                        </th>
                        <th>
                            Last Name
                        </th>
                        <th>
                            Action
                        </th>
                    </thead>
                    <tbody>
                        ${returnBodyHtml(this.customers)}
                    </tbody>
                </table>
                <dialog>
                    <div id="content">
                    </div>
                    <button class="yes btn btn-danger">Yes</button>
                    <button class="no btn btn-secondary">No</button>
                </dialog>`}
            </div>
        `;
    }

    async fetchCustomers() {
        const response = await axios.get('api/customers');
        return response.data;
    }
}