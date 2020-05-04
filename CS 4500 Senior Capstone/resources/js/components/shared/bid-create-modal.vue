<template>
    <div id="bidModal" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header card-header">
                    <h5>New Bid</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="bidName">Name</label>
                        <input id="bidName" type="text" class="form-control" v-model="form.name">
                    </div>
                    <div class="form-group">
                        <label for="bidDesc">Description</label>
                        <textarea id="bidDesc" class="form-control" v-model="form.description"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" @click="create">Create</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "bid-create-modal",
        props: ['data', 'adjustments'],
        data() {
            return {
                form: {
                    name: null,
                    description: null,
                    project_id: this.data.project_id,
                    materials: this.data.materials,
                    address: this.data.address,
                    city: this.data.city,
                    state: this.data.state,
                    zip: this.data.zip,
                    contact_name: this.data.contact_name,
                    contact_phone: this.data.contact_phone,
                    contact_email: this.data.contact_email,
                    sq_foot: this.data.sq_foot,
                    labor_hours: this.data.labor_hours,
                    labor_rate: this.data.labor_rate,
                    percentage_on_top: this.data.percentage_on_top,
                    won: 0,
                    adjustments: this.adjustments,
                }
            }
        },
        methods: {
            create() {
                let self = this;
                axios.post('/api/bid', this.form)
                    .then(res => {
                        window.location = '/bid/' + res.data.bid.id;
                    })
                    .catch(function (err) {
                        $.each(err.response.data.errors, function (index, error) {
                            self.$parent.$parent.$refs.mytoast.e(error[0]);
                        });
                    });
            }
        }
    }
</script>
