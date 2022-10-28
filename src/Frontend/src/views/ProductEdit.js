import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router";
import AbstractView from "./AbstractView";

let _object = null;

export default class ProductEdit extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Edit Product');
        this.form = {
            productName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Product Name is required',
                    v => v.length >= 3 || 'Product Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Product Name cannot contain whitespaces'
                ]
            },
            price: {
                value: 0,
                showError: false,
                error: '',
                rules: [
                    v => v.toString() !== '' || 'Price is required',
                    v => v > 0 || 'Price cannot be negative'
                ]
            },
            productKind: {
                value: 0,
                rules: [
                    v => v !== null || 'Product Kind is required',
                    v => v > 0 || 'Product Kind is required',
                ]
            }
        }
        this.productKinds = [];
        this.product = null;
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
        await axios.put(`api/products/${this.params.id}`, {
            productName: this.form.productName.value,
            price: this.form.price.value,
            productKind: {
                id: this.form.productKind.value
            }
        })
        this.loading = false;
        navigateTo('/products');
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

    async created() {
        _object = this;
        document.addEventListener('click', this.assignSendForm, false);
        document.addEventListener('change', this.assignInputs, false);
        this.productKinds = await this.fetchProductKinds();
        this.form.productKind.value = this.productKinds.find(_ => true).id;
        this.product = await this.fetchProduct();
        
        this.form = {
            ...this.form,
            productName: {
                ...this.form.productName,
                value: this.product.productName
            },
            price: {
                ...this.form.price,
                value: this.product.price
            },
            productKind: {
                ...this.form.productKind,
                value: this.product.productKind.id
            }
        }
    }

    async fetchProduct() {
        const response = await axios.get(`api/products/${this.params.id}`);
        return response.data;
    }

    async fetchProductKinds() {
        const response = await axios.get('api/product-kinds');
        return response.data;
    }

    async getHtml() {
        const getProductKindsAsOptions = () => {
            let options = '';
            let i = -1;
            const indexOfValue = this.productKinds.map(pk => pk.id).indexOf(Number(this.form.productKind.value));
            
            for (const productKind of this.productKinds) {
                i++;
                if (i === indexOfValue) {
                    options += `<option value=${productKind.id} selected="selected">${productKind.productKindName}</option>`;
                    continue;
                }
                options += `<option value=${productKind.id}>${productKind.productKindName}</option>`;
            }

            return options;
        }
        return `
            <div class="containerBox">
                <h1>Add Customer</h1>
                <div class="mt-2">
                    <form>
                        <div class="form-group">
                            <label for="input-productName">Product Name</label>
                            <input type="text" class="form-control" id="input-productName" aria-describedby="Product Name" placeholder="Enter product name"
                                value="${this.form.productName.value}" />
                            ${this.form.productName.error && this.form.productName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.productName.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-price">Price</label>
                            <input type="number" class="form-control" id="input-price" aria-describedby="Price" placeholder="Enter price"
                                value="${this.form.price.value}" step="0.01" />
                            ${this.form.price.error && this.form.price.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.price.error}
                                </div>`
                                : '' }
                        </div>
                        <div class="form-group">
                            <label for="input-productKind">Product Kind</label>
                            <select type="text" class="form-control" id="input-productKind" aria-describedby="Product Kind"
                                value="${this.form.productKind.value}" >
                                ${getProductKindsAsOptions()}
                            </select>
                            ${this.form.productKind.error && this.form.productKind.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.productKind.error}
                                </div>`
                                : '' }
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