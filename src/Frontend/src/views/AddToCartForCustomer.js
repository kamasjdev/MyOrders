import axios from "../axios-setup";
import LoadingIcon from "../common/loadingIcon";
import AbstractView from "./AbstractView";

export default class AddToCartForCustomer extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle('Add To Cart');
        this.loading = true;
        this.products = [];
    }

    async created() {
        const products = await this.fetchProducts();
        this.products = products;
        this.loading = false;
    }

    afterCreateView() {
        for (const product of this.products) {
            document.querySelector(`#add-to-cart-${product.id}`).onclick = async () => await this.addToCart(product);
        }
    }

    async addToCart(product) {
        await axios.post(`api/order-items`, {
            productId: product.id,
            customerId: this.params.id
        });
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
                html += `<td><button id="add-to-cart-${product.id}" type="button" class="btn btn-primary">Add To Cart</button></td>`;

                html += '</tr>';
            }
            return html;
        }
        return `
            <div class="containerBox">
                <h1>Add to cart for customer ${this.params.id}</h1>
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
                </table>`}
            </div>
        `;
    }

    async fetchProducts() {
        const response = await axios.get('api/products');
        return response.data;
    }
}