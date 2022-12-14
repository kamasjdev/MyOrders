export default class AbstractView {
    constructor(params) {
        this.params = params;
    }

    setTitle(title) {
        document.title = title;
    }

    async getHtml() {
        return ""; // returns html template per view
    }

    // invoked when view is created using router
    async created() {
        
    }

    beforeCreateView() {

    }

    afterCreateView() {

    }

    forceUpdateView() {
        dispatchEvent(new CustomEvent('pageChanged', {}));
    }

    onDestroy() {
    }
}