import Errors from "./errors";

export default class Form {

    constructor(data) {
        this.originalData = data;

        for (let field in data) {
            this[field] = data[field];
        }

        this.errors = new Errors();
        this.successful = false;
    }

    data() {
        let data = {};
        for (let property in this.originalData) {
            data[property] = this[property];
        }
        return data;
    }

    reset() {
        for (let field in this.originalData) {
            if (Array.isArray(this[field])) {
                this[field] = [];
            } else {
                this[field] = '';
            }
        }
        this.errors.clear();
    }

    post(url) {
        return this.submit('post', url);
    }

    put(url) {
        return this.submit('put', url);
    }

    delete(url) {
        return this.submit('delete', url);
    }

    submit(requestType, url) {
        return new Promise((resolve, reject) => {
            window.axios[requestType](url, this.data())
                .then(response => {
                    this.onSuccess(response.data);

                    resolve(response.data);
                })
                .catch(error => {
                    this.onFail(error.response);

                    reject(error.response.data);
                });
        });
    }

    onSuccess(response) {
        this.successful = true;
    }

    onFail(response) {
        this.errors.record(response);
    }
}
