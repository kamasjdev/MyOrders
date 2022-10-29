import axios from '../../axios-setup';
import ProductAdd from '../ProductAdd';

describe('ProductAdd view', () => {
    let productAdd = null;
    const productKindsMock = [{ id: 1, productKindName: 'ProductKind#1' }, { id: 2, productKindName: 'ProductKind#2' }];
    const hisoryMock = {
        pushState(data, unused, url) {}
    }
    global.history = hisoryMock;
    const app = document.querySelector('div#app');

    beforeEach(() => {
        productAdd = new ProductAdd();
    })

    it('should render html', async () => {
        app.innerHTML = await productAdd.getHtml();
        
        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Add Product');
    })

    it('should render ProductApp view', async () => {
        spyOn(axios, 'get').and.returnValue({ data: productKindsMock });
        productAdd.beforeCreateView();
        await productAdd.created();
        productAdd.afterCreateView();

        app.innerHTML = await productAdd.getHtml();

        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Add Product');
        expect(productKindsMock.map(pk => pk.productKindName).every(str => app.textContent.includes(str))).toBeTruthy();
    })

    it('should fill ProductApp form with proper values', async () => {
        spyOn(axios, 'get').and.returnValue({ data: productKindsMock });
        productAdd.beforeCreateView();
        app.innerHTML = await productAdd.getHtml();
        await productAdd.created();
        app.innerHTML = await productAdd.getHtml();
        productAdd.afterCreateView();
        const valuesToAssign = assignInputs('Product#1', 100, productKindsMock[1].id);

        app.innerHTML = await productAdd.getHtml();

        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Add Product');
        const valuesAssigned = getInputValuesFromInputs();
        expect(valuesAssigned.productName).toBe(valuesToAssign.productName);
        expect(valuesAssigned.price).toBe(valuesToAssign.price);
        expect(valuesAssigned.productKind).toBe(valuesToAssign.productKind);
    })

    it('should fill ProductApp form with invalid values and show errors', async () => {
        spyOn(axios, 'get').and.returnValue({ data: productKindsMock });
        productAdd.beforeCreateView();
        app.innerHTML = await productAdd.getHtml();
        await productAdd.created();
        app.innerHTML = await productAdd.getHtml();
        productAdd.afterCreateView();
        const valuesToAssign = assignInputs('P', -100, productKindsMock[1].id);

        app.innerHTML = await productAdd.getHtml();

        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Add Product');
        const valuesAssigned = getInputValuesFromInputs();
        expect(valuesAssigned.productName).toBe(valuesToAssign.productName);
        expect(valuesAssigned.price).toBe(valuesToAssign.price);
        expect(valuesAssigned.productKind).toBe(valuesToAssign.productKind);
        expect(getErrors().every(str => app.textContent.includes(str))).toBeTruthy();
    })

    it('should fill ProductApp form with proper values and send values to backend', async () => {
        spyOn(axios, 'get').and.returnValue({ data: productKindsMock });
        spyOn(axios, 'post').and.returnValue({ });
        productAdd.beforeCreateView();
        app.innerHTML = await productAdd.getHtml();
        await productAdd.created();
        app.innerHTML = await productAdd.getHtml();
        productAdd.afterCreateView();
        const valuesToAssign = assignInputs('Product#1', 100, productKindsMock[1].id);
        app.innerHTML = await productAdd.getHtml();
        
        await productAdd.sendForm();
        app.innerHTML = await productAdd.getHtml();

        expect(axios.post).toHaveBeenCalledTimes(1);
        const valuesAssigned = getInputValuesFromInputs();
        expect(valuesAssigned.productName).toBe(valuesToAssign.productName);
        expect(valuesAssigned.price).toBe(valuesToAssign.price);
        expect(valuesAssigned.productKind).toBe(valuesToAssign.productKind);
    })

    const assignInputs = (productName, price, productKind) => {
        productAdd.onChangeInput(productName, 'productName');
        productAdd.onChangeInput(price, 'price');
        productAdd.onChangeInput(productKind, 'productKind');
        return { productName, price, productKind };
    }

    const getErrors = () => {
        const invalidFeedback = document.querySelectorAll('.invalid-feedback');
        const errors = [];
        invalidFeedback.forEach(feedback => errors.push(feedback.textContent));
        return errors;
    }

    const getInputValuesFromInputs = () => {
        const productName = document.querySelector('#input-productName').value;
        const price = Number(document.querySelector('#input-price').value);
        const productKind = Number(document.querySelector('#input-productKind').value);
        return { productName, price, productKind };
    }
});