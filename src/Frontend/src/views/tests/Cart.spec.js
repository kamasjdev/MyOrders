import axios from '../../axios-setup';
import Cart from '../Cart';

describe('Cart view', () => {
    let cart = null;
    const customersMock = [{ id: 1, firstName: 'Customer#1First', lastName: 'Customer#1Last' }, { id: 2, firstName: 'Customer#2First', lastName: 'Customer#2Last' }];

    beforeEach(() => {
        cart = new Cart();
    })

    it('should render html', async () => {
        const app = document.querySelector('div#app');
        app.innerHTML = await cart.getHtml();
        
        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Choose customer for check cart');
        expect(document.body.querySelector('#loading')).toBeTruthy();
    })

    it('should render Cart with customers', async () => {
        spyOn(axios, 'get').and.returnValue({ data: customersMock });
        await cart.created();

        const app = document.querySelector('div#app');
        app.innerHTML = await cart.getHtml();

        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Choose customer for check cart');
        expect(document.body.querySelector('#loading')).not.toBeTruthy();
        expect(document.body.innerHTML).toContain('table');
        expect(document.body.innerHTML).toContain(customersMock[0].firstName);
    })
});