import axios from '../../axios-setup';
import ProductEdit from '../ProductEdit';

describe('ProductEdit view', () => {
    let productEdit = null;
    const productId = 1;
    const productKindsMock = [{ id: 1, productKindName: 'ProductKind#1' }, { id: 2, productKindName: 'ProductKind#2' }];
    const productToEditMock = { id: productId, productName: 'Product#1', price: 10.20, productKind: productKindsMock[0] };
    const hisoryMock = {
        pushState(data, unused, url) {}
    }
    global.history = hisoryMock;
    const app = document.querySelector('div#app');

    const invokeAllHooks = async () => {
        productEdit.beforeCreateView();
        app.innerHTML = await productEdit.getHtml();
        await productEdit.created();
        app.innerHTML = await productEdit.getHtml();
        productEdit.afterCreateView();
        app.innerHTML = await productEdit.getHtml();
    }

    const renderHtml = async () => {
        app.innerHTML = await productEdit.getHtml();
    }

    beforeEach(() => {
        productEdit = new ProductEdit({ id: productId });
    })

    it('should render html', async () => {
        await renderHtml();
        
        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Edit Product');
    })

    it('should render ProductEdit view', async () => {
        spyOn(axios, 'get').withArgs('api/product-kinds').and.returnValue({ data: productKindsMock })
                           .withArgs(`api/products/${productEdit.params.id}`).and.returnValue({ data: productToEditMock });
        await invokeAllHooks();

        await renderHtml();

        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Edit Product');
        expect(productKindsMock.map(pk => pk.productKindName).every(str => app.textContent.includes(str))).toBeTruthy();
        expect([productToEditMock.price.toString(), productToEditMock.productName].every(str => getInputValuesFromInputsAsStringArray().includes(str))).toBeTruthy();
    })

    it('fill with form invalid values should show errors', async () => {
        spyOn(axios, 'get').withArgs('api/product-kinds').and.returnValue({ data: productKindsMock })
                           .withArgs(`api/products/${productEdit.params.id}`).and.returnValue({ data: productToEditMock });
        await invokeAllHooks();
        
        const assignedValues = await fillFormWithValues('', -1, 2);
        
        expect(getErrors().every(str => app.textContent.includes(str))).toBeTruthy();
        const valuesFromInputs = getInputValuesFromInputs();
        expect(valuesFromInputs.productName).toBe(assignedValues.productName);
        expect(valuesFromInputs.price).toBe(assignedValues.price);
        expect(valuesFromInputs.productKind).toBe(assignedValues.productKind);
    })

    it('should send form to backend', async () => {
        spyOn(axios, 'get').withArgs('api/product-kinds').and.returnValue({ data: productKindsMock })
                           .withArgs(`api/products/${productEdit.params.id}`).and.returnValue({ data: productToEditMock });
        spyOn(axios, 'put').and.returnValue({ });
        await invokeAllHooks();
        const assignedValues = await fillFormWithValues('Abc#1', 1000.50, 2);

        triggerSendDataToBackend();
        
        expect(axios.put).toHaveBeenCalledTimes(1);
        const valuesFromInputs = getInputValuesFromInputs();
        expect(valuesFromInputs.productName).toBe(assignedValues.productName);
        expect(valuesFromInputs.price).toBe(assignedValues.price);
        expect(valuesFromInputs.productKind).toBe(assignedValues.productKind);
        expect(getErrors().length).toBe(0);
    })

    const triggerSendDataToBackend = () => {
        document.querySelector('#send-form').dispatchEvent(new window.Event('click', { bubbles: true }));
    }

    const fillFormWithValues = async (productName, price, productKind) => {
        productEdit.form.productName.value = productName;
        productEdit.form.price.value = price;
        productEdit.form.productKind.value = productKind;
        await renderHtml();
        triggerOnChangeOnEveryInput();
        await renderHtml();
        return { productName: productName, price: price, productKind: productKind };
    }

    const triggerOnChangeOnEveryInput = () => {
        document.querySelectorAll('.form-control').forEach(input => input.dispatchEvent(new window.Event('change', { bubbles: true })));
    }

    const getInputValuesFromInputs = () => {
        const productName = app.querySelector('#input-productName').value;
        const price = Number(app.querySelector('#input-price').value);
        const productKind = Number(app.querySelector('#input-productKind').value);
        return { productName, price, productKind };
    }

    const getInputValuesFromInputsAsStringArray = () => {
        const productName = document.querySelector('#input-productName').value;
        const price = Number(document.querySelector('#input-price').value);
        const productKind = Number(document.querySelector('#input-productKind').value);
        return [ productName, price.toString(), productKind.toString() ];
    }

    const getErrors = () => {
        const invalidFeedback = document.querySelectorAll('.invalid-feedback');
        const errors = [];
        invalidFeedback.forEach(feedback => errors.push(feedback.textContent));
        return errors;
    }
})