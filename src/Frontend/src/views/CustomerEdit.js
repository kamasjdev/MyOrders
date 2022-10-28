import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router";
import AbstractView from "./AbstractView";

let _object = null;

export default class CustomerEdit extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Edit Customer');
        this.customer = null;
        this.form = {
            firstName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'First Name is required',
                    v => v.length >= 3 || 'First Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'First Name cannot contain whitespaces'
                ]
            },
            lastName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Last Name is required',
                    v => v.length >= 3 || 'Last Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Last Name cannot contain whitespaces'
                ]
            },
            countryName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Last Name is required',
                    v => v.length >= 3 || 'Last Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Last Name cannot contain whitespaces'
                ]
            },
            cityName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Last Name is required',
                    v => v.length >= 3 || 'Last Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Last Name cannot contain whitespaces'
                ]
            },
            streetName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Last Name is required',
                    v => v.length >= 3 || 'Last Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Last Name cannot contain whitespaces'
                ]
            },
            buildingNumber: {
                value: null,
                rules: [
                    v => v !== null || 'Building Number is required',
                    v => v > 0 || 'Building Number cannot be negative'
                ]
            },
            flatNumber: {
                value: null,
                showError: false,
                error: '',
                rules: [
                    v => !v || (v && v > 0) || 'Flat Number cannot be negative'
                ]
            },
            zipCode: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== '' || 'Zip Code is required',
                    v => !/^\s+$/.test(v) || 'Zip Code cannot contain whitespaces'
                ]
            },
            email: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Email is required',
                    v => v.length > 0 || 'Email is required',
                    v => /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(v) || 'Invalid email'
                ]
            },
            countryCode: {
                value: '+48',
                options: [ '+48', '+49', '+1', '+46' ],
                rules: [
                    v => v !== null || 'Country Code is required',
                    v => v.length > 0 || 'Country Code is required',
                ]
            },
            phoneNumber: {
                value: '',
                rules: [
                    v => v !== null || 'Phone Number is required',
                    v => v.length > 0 || 'Phone Number is required',
                ]
            }
        }
        this.loading = false;
    }

    async sendForm() {
        const errors = [];
        for (const field in this.form) {
            const error = this.validate(this.form[field].value, field);
            if (error.length > 0) {
                errors.push(error);
                this.form = {
                    ...this.form, 
                    [field]: {
                        ...this.form[field],
                        showError: error ? true : false,
                        error: error
                    }
                }
            }
        }

        if (errors.length > 0) {
            return;
        }

        this.loading = true;
        
        Promise.all([axios.put(`api/addresses/${this.customer.address.id}`, {
            countryName: this.form.countryName.value,
            cityName: this.form.cityName.value,
            streetName: this.form.streetName.value,
            buildingNumber: this.form.buildingNumber.value,
            flatNumber: this.form.flatNumber.value,
            zipCode: this.form.zipCode.value
        }),axios.put(`api/contact-datas/${this.customer.contactData.id}`, {
            email: this.form.email.value,
            countryCode: this.form.countryCode.value,
            phoneNumber: this.form.phoneNumber.value
        })])
            .then(response => {
                axios.put(`api/customers/${this.params.id}`, {
                    firstName: this.form.firstName.value,
                    lastName: this.form.lastName.value,
                    addressId: response[0].data.id,
                    contactDataId: response[1].data.id
                }).then(_ => {
                    this.loading = false;
                    navigateTo('/customers')
                });
            })
    }

    validate(value, fieldName) {
        const rules = this.form[fieldName].rules;
        
        for (const rule of rules) {
            const valid = rule(value);
            if (valid !== true) {
                return valid;
            }
        }

        return '';
    }

    onChangeInput(value, fieldName) {
        const error = this.validate(value, fieldName);
        this.form = {
            ...this.form, 
            [fieldName]: {
                ...this.form[fieldName],
                value,
                showError: error ? true : false,
                error: error
            }
        }
    }

    async assignSendForm(e) {
        if (e.target.matches("#send-form")) {
            e.preventDefault();
            await _object.sendForm()
        }
    }

    assignInputs(e) {
        for(const field in _object.form) {
            if (e.target.matches(`#input-${field}`)) {
                e.preventDefault();
                _object.onChangeInput(e.target.value, field);
            }
        }
    }
    
    onDestroy() {
        document.removeEventListener('click', this.assignSendForm, false);
        document.removeEventListener('change', this.assignInputs, false);
        _object = null;
    }

    afterCreateView() {
        document.addEventListener('click', this.assignSendForm, false);
        document.addEventListener('change', this.assignInputs, false);
    }

    async created() {
        _object = this;
        this.customer = await this.fetchCustomer();
        this.form = {
            ...this.form,
            firstName: {
                ...this.form.firstName,
                value: this.customer?.firstName ?? '',
            },
            lastName: {
                ...this.form.lastName,
                value: this.customer?.lastName ?? '',
            },
            countryName: {
                ...this.form.countryName,
                value: this.customer?.address?.countryName ?? '',
            },
            cityName: {
                ...this.form.cityName,
                value: this.customer?.address?.cityName ?? '',
            },
            streetName: {
                ...this.form.streetName,
                value: this.customer?.address?.streetName ?? '',
            },
            buildingNumber: {
                ...this.form.buildingNumber,
                value: this.customer?.address?.buildingNumber ?? null,
            },
            flatNumber: {
                ...this.form.flatNumber,
                value: this.customer?.address?.flatNumber ?? null,
            },
            zipCode: {
                ...this.form.zipCode,
                value: this.customer?.address?.zipCode ?? '',
            },
            email: {
                ...this.form.email,
                value: this.customer?.contactData?.email ?? '',
            },
            countryCode: {
                ...this.form.countryCode,
                value: this.customer?.contactData?.countryCode ?? '+48',
            },
            phoneNumber: {
                ...this.form.phoneNumber,
                value: this.customer?.contactData?.phoneNumber ?? '',
            }
        }
    }

    async fetchCustomer() {
        const response = await axios.get(`api/customers/${this.params.id}`);
        return response.data;
    }
    
    async getHtml() {
        const getCountryCodesAsOptions = () => {
            let options = '';
            let i = -1;
            const indexOfValue = this.form.countryCode.options.indexOf(this.form.countryCode.value);
            
            for (const option of this.form.countryCode.options) {
                i++;
                if (i === indexOfValue) {
                    options += `<option value=${option} selected="selected">${option}</option>`;
                    continue;
                }
                options += `<option value=${option}>${option}</option>`;
            }

            return options;
        }
        return `
            <div class="containerBox">
                <h1>Add Customer</h1>
                <div class="mt-2">
                    <form>
                        <div class="form-group">
                            <label for="input-firstName">First Name</label>
                            <input type="text" class="form-control" id="input-firstName" aria-describedby="First Name" placeholder="Enter first name"
                                value="${this.form.firstName.value}" />
                            ${this.form.firstName.error && this.form.firstName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.firstName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-lastName">Last Name</label>
                            <input type="text" class="form-control" id="input-lastName" aria-describedby="Last Name" placeholder="Enter last name"
                                value="${this.form.lastName.value}" />
                            ${this.form.lastName.error && this.form.lastName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.lastName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-countryName">Country Name</label>
                            <input type="text" class="form-control" id="input-countryName" aria-describedby="Country Name" placeholder="Enter country name"
                                value="${this.form.countryName.value}" />
                            ${this.form.countryName.error && this.form.countryName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.countryName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-cityName">City Name</label>
                            <input type="text" class="form-control" id="input-cityName" aria-describedby="City Name" placeholder="Enter city name"
                                value="${this.form.cityName.value}" />
                            ${this.form.cityName.error && this.form.cityName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.cityName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-streetName">Street Name</label>
                            <input type="text" class="form-control" id="input-streetName" aria-describedby="Street Name" placeholder="Enter street name"
                                value="${this.form.streetName.value}" />
                            ${this.form.streetName.error && this.form.streetName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.streetName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-buildingNumber">Building Number</label>
                            <input type="number" class="form-control" id="input-buildingNumber" aria-describedby="Building Number" placeholder="Enter building number"
                                value="${this.form.buildingNumber.value ? this.form.buildingNumber.value : ''}" />
                            ${this.form.buildingNumber.error && this.form.buildingNumber.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.buildingNumber.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-flatNumber">Flat Number</label>
                            <input type="number" class="form-control" id="input-flatNumber" aria-describedby="Flat Number" placeholder="Enter flat number"
                                value="${this.form.flatNumber.value ? this.form.flatNumber.value : ''}" />
                            ${this.form.flatNumber.error && this.form.flatNumber.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.flatNumber.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-zipCode">Zip Code</label>
                            <input type="text" class="form-control" id="input-zipCode" aria-describedby="Zip Code" placeholder="Enter zip code"
                                value="${this.form.zipCode.value ? this.form.zipCode.value : ''}" />
                            ${this.form.zipCode.error && this.form.zipCode.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.zipCode.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-email">Email</label>
                            <input type="text" class="form-control" id="input-email" aria-describedby="Email" placeholder="Enter email"
                                value="${this.form.email.value}" />
                            ${this.form.email.error && this.form.email.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.email.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="d-flex">
                            <div class="form-group w-15 me-2">
                                <label for="input-countryCode">Country code</label>
                                <select type="text" class="form-control" id="input-countryCode" aria-describedby="Country Code" placeholder="Enter country code"
                                    value="${this.form.countryCode.value}" >
                                    ${getCountryCodesAsOptions()}
                                </select>
                                ${this.form.countryCode.error && this.form.countryCode.showError ?
                                    `<div class="invalid-feedback d-flex text-start">
                                        ${this.form.countryCode.error}
                                    </div>`
                                    : '' }
                            </div>
                            <div class="form-group flex-grow-1">
                                <label for="input-phoneNumber">Phone Number</label>
                                <input type="number" class="form-control" id="input-phoneNumber" aria-describedby="Phone Number" placeholder="Enter phone number"
                                    value="${this.form.phoneNumber.value}" />
                                ${this.form.phoneNumber.error && this.form.phoneNumber.showError ?
                                    `<div class="invalid-feedback d-flex text-start">
                                        ${this.form.phoneNumber.error}
                                    </div>`
                                    : '' }
                            </div>
                        </div>
                    </form>
                </div>
                ${this.loading ? 
                    `<div class="mt-2">
                        ${LoadingIcon()}
                    </div>`
                    : ''}
                <div class="mt-2">
                    <button class="btn btn-success" id="send-form" type="button">Send</button>
                    <a class="btn btn-secondary" type="button" href="/customers">Cancel</a>
                </div>
            </div>
        `;
    }
}