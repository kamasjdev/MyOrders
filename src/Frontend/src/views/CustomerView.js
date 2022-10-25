import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon.js";
import { navigateTo } from "../common/router.js";
import AbstractView from "./AbstractView";

export default class CustomerView extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Customer View');
        this.customer = null;
        this.loading = true;
    }

    async created() {
        document.body.addEventListener('click', e => {
            if (e.target.matches("#back-to-customers")) {
                e.preventDefault();
                navigateTo('/customers');
            }
        });
        const response = await axios.get(`api/customers/${this.params.id}`);
        this.customer = response.data;
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
                            First Name:
                        </div>
                        <div>
                            ${this.customer.fistName}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Last Name:
                        </div>
                        <div>
                            ${this.customer.lastName}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Email:
                        </div>
                        <div>
                            ${this.customer.contactData.email}
                        </div>
                    </div>
                    <div class="mt-2 d-flex justify-content-center">
                        <div class="me-4">
                            Phone Number:
                        </div>
                        <div>
                            ${ `${this.customer.contactData.countryCode} ${this.customer.contactData.phoneNumber}`}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Country:
                        </div>
                        <div>
                            ${this.customer.address.countryName}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            City:
                        </div>
                        <div>
                            ${this.customer.address.cityName}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Zip Code:
                        </div>
                        <div>
                            ${this.customer.address.zipCode}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Steet:
                        </div>
                        <div>
                            ${this.customer.address.streetName}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Building Number:
                        </div>
                        <div>
                            ${this.customer.address.buildingNumber}
                        </div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="me-4">
                            Flat Number:
                        </div>
                        <div>
                            ${this.customer.address.flatNumber ? this.customer.address.flatNumber : ''}
                        </div>
                    </div>
                </div>`}
                <button id="back-to-customers" class="mt-4 btn btn-primary">Back to Customers</button>
            </div>
        `;
    }
}