import axios from "../axios-setup.js";
import loadingIcon from "../common/loadingIcon";
import { navigateTo } from "../common/router.js";
import AbstractView from "./AbstractView";

export default class ProductKinds extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Product Kinds');
        this.loading = true;
        this.productKinds = [];
    }

    deleteProductKind = (id) => {
        axios.delete(`api/product-kinds/${id}`)
        .then(_ => {
            navigateTo('/product-kinds');
        });
    }

    async created() {
        const productKinds = await this.fetchProductKinds();
        this.productKinds = productKinds;
        this.loading = false;
        document.addEventListener('click', (event) => {
            for (const productKind of productKinds) {
                if (event.target.matches(`#delete-product-kind-${productKind.id}`)) {
                    const dialogEl = document.querySelector('dialog');
                    event.preventDefault();
                    document.querySelector('#content').innerHTML = `Do you wish to delete ProductKind with id ${productKind.id}?`;
                    dialogEl.showModal();
                    const yesBtn = document.querySelector('.yes');
                    yesBtn.onclick = () => {
                        dialogEl.close();
                        this.deleteProductKind(productKind.id);
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
        const returnBodyHtml = (pk) => {
            let html = '';
            for (const productKind of pk) {
                html += '<tr>';
                
                for (const field in productKind) {
                    html += '<td>';
                    html += productKind[field]
                    html += '</td>';
                }
                html += `<td>
                            <a href="/product-kinds/edit/${productKind.id}" class="btn btn-warning me-2" data-link>Edit</a>
                            <button id="delete-product-kind-${productKind.id}" class="btn btn-danger" type="button">Delete</button>
                        </td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Product Kinds</h1>
                ${this.loading ? loadingIcon() : 
                `<div class="d-flex justify-content-start mb-2">
                    <a class="btn btn-success" href="/product-kinds/add" data-link>Add</a>
                </div>
                <table class="table table">
                    <thead>
                        <th>
                            Id
                        </th>
                        <th>
                            Product Kind Name
                        </th>
                        <th>
                            Action
                        </th>
                    </thead>
                    <tbody>
                        ${returnBodyHtml(this.productKinds)}
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

    async fetchProductKinds() {
        const response = await axios.get('api/product-kinds');
        return response.data;
    }
}