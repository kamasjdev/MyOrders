import AbstractView from "./AbstractView";
import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";

export default class Products extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Products');
        this.loading = true;
        this.products = [];
    }

    beforeCreateView() {
        document.addEventListener('click', (event) => {
            for (const product of this.products) {
                if (event.target.matches(`#delete-product-${product.id}`)) {
                    const dialogEl = document.querySelector('dialog');
                    event.preventDefault();
                    document.querySelector('#content').innerHTML = `Do you wish to delete Product with id ${product.id}?`;
                    dialogEl.showModal();
                    const yesBtn = document.querySelector('.yes');
                    yesBtn.onclick = async () => {
                        dialogEl.close();
                        this.deleteProduct(product.id);
                    };
                    const noBtn = document.querySelector('.no');
                    noBtn.onclick = () => {
                        dialogEl.close();
                    };
                }
            }
        });
    }

    async deleteProduct(id) {
        await axios.delete(`api/products/${id}`);
        const products = await this.fetchProducts();
        this.products = products;
        this.forceUpdateView();
    }

    async created() {
        const products = await this.fetchProducts();
        this.products = products;
        this.loading = false;
    }

    async getHtml() {
        const returnBodyHtml = (p) => {
            let html = '';
            for (const product of p) {
                html += '<tr>';
                
                for (const field in product) {
                    html += '<td>';
                    html += product[field]
                    html += '</td>';
                }
                html += `<td>
                            <a href="/products/${product.id}" class="btn btn-primary" data-link>Show Details</a>
                            <button id="delete-product-${product.id}" class="btn btn-danger" type="button">Delete</button>
                        </td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Products</h1>
                ${this.loading ? LoadingIcon() : 
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
                        ${returnBodyHtml(this.products)}
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

    async fetchProducts() {
        const response = await axios.get('api/products');
        return response.data;
    }
}