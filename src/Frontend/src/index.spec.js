describe('render html', () => {
    beforeEach(() => {
    })

    it('should render html', () => {
        const h3 = document.querySelector('h3');

        expect(document).not.toBeNull();
        expect(document.body.innerHTML.length).toBeGreaterThan(0);
        expect(h3).not.toBeNull();
        expect(h3.innerHTML).toBe('Generated using template.html');
    })
})