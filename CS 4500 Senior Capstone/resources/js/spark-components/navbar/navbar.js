var base = require('navbar/navbar');

Vue.component('spark-navbar', {
    mixins: [base],
    created() {
        let self = this;
        Bus.$on('taskCreated', function () {
            self.getIncompleteTaskCount();
        });
        Bus.$on('taskUpdated', function () {
            self.getIncompleteTaskCount();
        });
    },
    mounted() {
        this.getIncompleteTaskCount();
    },
    data() {
        return {
            incompleteTaskCount: null,
        }
    },
    methods: {
        getIncompleteTaskCount() {
            let self = this;
            axios.get('/api/task/incomplete/count')
                .then((response) => {
                    self.incompleteTaskCount = response.data;
                });
        },
    },
});
