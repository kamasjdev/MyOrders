import Home from '../Home';

describe('Home view', () => {
    let home = null;

    beforeEach(() => {
        home = new Home();
    })

    it('should render html', async () => {
        const app = document.querySelector('div#app');

        app.innerHTML = await home.getHtml();

        expect(app).not.toBeNull();
        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.querySelector('.containerBox').textContent).toContain('Welcome to the MyOrders App');
    })
})