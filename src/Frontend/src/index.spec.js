describe('render html', () => {
    beforeEach(() => {
    })

    it('should render html', () => {
        const app = document.querySelector('div#app');

        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(document.body.innerHTML).toContain('Home');
        expect(app).not.toBeNull();
    })
})