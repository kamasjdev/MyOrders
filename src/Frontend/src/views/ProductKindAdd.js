import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router";
import AbstractView from "./AbstractView";

let _object = null;

export default class ProductKindAdd extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Add Product Kind');
        this.form = {
            productKindName: {
                value: '',
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Product Kind Name is required',
                    v => v.length >= 3 || 'Product Kind Name should have at least 3 characters',
                    v => !/^\s+$/.test(v) || 'Product Kind Name cannot contain whitespaces'
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
        await axios.post('api/product-kinds', {
            productKindName: this.form.productKindName.value
        });
        this.loading = false;
        navigateTo('/product-kinds');
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
        console.log(this);
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

    assignInput(e) {
        if (e.target.matches("#productKindName")) {
            e.preventDefault();
            console.log(_object);
            _object.onChangeInput(e.target.value, 'productKindName');
        }
    }
    
    onDestroy() {
        document.removeEventListener('click', this.assignSendForm, false);
        document.removeEventListener('change', this.assignInput, false);
        _object = null;
    }

    async created() {
        _object = this;
        document.addEventListener('click', this.assignSendForm, false);
        document.addEventListener('change', this.assignInput, false);
    }

    async getHtml() {
        return `
            <div class="containerBox">
                <h1>Add Product Kind</h1>
                <div class="mt-2">
                    <form>
                        <div class="form-group">
                            <label for="productKindName">Product Kind Name</label>
                            <input type="text" class="form-control" id="productKindName" aria-describedby="Product Kind Name" placeholder="Enter product kind name"
                                value="${this.form.productKindName.value}">
                            ${this.form.productKindName.error && this.form.productKindName.showError ?
                                `<div class="invalid-feedback d-flex text-start">
                                    ${this.form.productKindName.error}
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
                    <a class="btn btn-secondary" type="button" href="/product-kinds">Cancel</a>
                </div>
            </div>
        `;
    }
}