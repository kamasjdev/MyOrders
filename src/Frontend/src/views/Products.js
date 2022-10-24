import AbstractView from "./AbstractView";
import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";

export default class Products extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Products');
        this.loading = true;
    }

    showProducts = () => {
        window.addEventListener("pageLoaded", (event) => {
            if (!this.constructor.name === event.detail.page) {
                return;
            }

            this.fetchProducts().then(products => {;
                const loadingIcon = document.querySelector('#loading');
                if (loadingIcon) {
                    loadingIcon.remove();
                }
                const returnBodyHtml = (p) => {
                    let html = '';
                    for (const product of p) {
                        html += '<tr>';
                        
                        for (const field in product) {
                            html += '<td>';
                            html += product[field]
                            html += '</td>';
                        }
                        html += `<td><a href="/products/${product.id}" class="btn btn-primary" data-link>Show Details</a></td>`;

                        html += '</tr>';
                    }
                    return html;
                }
                document.querySelector('#table').innerHTML = 
                `
                <table class="table table">
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
                        ${returnBodyHtml(products)}
                    </tbody>
                </table>
                `;
            });
        });
    }

    async getHtml() {
        this.showProducts();
        return `
            <div class="containerBox">
                <h1>Products</h1>
                ${LoadingIcon()}
                <div id="table"></div>
            </div>
        `;
    }

    async fetchProducts() {
        const response = await axios.get('api/products');
        return response.data;
    }
}