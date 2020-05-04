<template>
    <div class="row">
        <div class="col-6">
            <div class="card container py-4">
                <div class="card-header justify-content-between">
                    <div><h5>{{ form.name }}</h5></div>
                    <div>
                        <a :href="`/bid/${form.id}/pdf`" class="btn btn-info">
                        <i class="fas fa-download"></i> PDF
                    </a>
                    <button @click="back" class="btn btn-secondary mr-2">
                        See Project
                    </button>
                    </div>
                    
                </div>
                <br>
                <div class="row container">
                    <div class="col-12">
                        <h5 class="sub-header">Description</h5>
                        <div>{{form.description}}</div>
                    </div>
                </div>
                <br>
                <div class="row container">
                    <div class="col-6">
                        <h5 class="sub-header">Bid Total</h5>
                        <h4 class="price">{{ form.price | formatPrice }}</h4>
                    </div>
                    <div class="col-6">
                        <h5 class="sub-header">Won</h5>
                        <h5 v-if="form.won">Yes</h5>
                        <h5 v-else>No</h5>
                    </div>
                </div>
                <div class="row container py-4">
                    <div class="col-6">
                        <button @click="unmarkWon" v-if="form.won" class="btn btn-secondary">Unmark bid as won</button>
                        <button @click="markWon" v-else class='btn btn-warning'>Mark bid as won</button>
                    </div>
                    <!-- <div class="col-6">
                        <button @click="deleteBid" class="btn btn-danger">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </div> -->
                </div>
                <div class="row">
                    <div class="col">
                        <h5 class="sub-header">Additional Charges/Discounts</h5>
                        <table class="table" id="extrachargestable">
                            <tbody v-if="bid.extra_charges.length > 0">
                                <tr v-for="e in bid.extra_charges" v-bind:key="e.id">
                                    <td>{{e.description}}</td>
                                    <td v-if="e.amount > 0" class="price">{{ e.amount | formatPrice }}</td>
                                    <td v-else class="price-neg">{{e.amount | formatPrice}}</td>
                                </tr>
                            </tbody>
                            <tbody v-else>
                                <tr>
                                    <td>No additional charges/discounts</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-6">
            <div class="card container py-4">
                <div class="card-header">
                    <h5>Materials</h5>
                </div>
                <br>
                <div class="row">
                    <div class="col">
                        <table class="table table-hover" id="bid-material-table">
                            <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Qty</th>
                                <th>Price</th>
                                <th>Total</th>
                            </tr>
                            </thead>
                            <tbody>
                            <tr v-for="m in form.materials" :key="m.id" :tempid="m">
                                <td>{{ m.name }}</td>
                                <td>{{ m.description | strLimit(25) }}</td>
                                <td>{{ m.quantity }}</td>
                                <td class="price">{{ m.price | formatPrice }}</td>
                                <td class="price">{{ calculateTotal(m) | formatPrice }}</td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "bid-show",
        props: ['bid'],
        data() {
            return {
                form: this.initForm(this.bid),
                tableLoaded: false,
            };
        },
        updated(){
            if(!this.tableLoaded){
            $("#bid-material-table").dtable({
                pageLimit: 5
            });
            $("#extrachargestable").dtable({pageLimit: 5, header: false, filter: false})
            this.tableLoaded = true
            }
        },
        methods: {
            initForm(bid) {
                return {
                    id: bid.id,
                    name: bid.name,
                    description: bid.description,
                    price: bid.price,
                    project_id: bid.project.id,
                    materials: bid.materials,
                    address: bid.address,
                    city: bid.city,
                    state: bid.state,
                    zip: bid.zip,
                    contact_name: bid.contact_name,
                    contact_phone: bid.contact_phone,
                    contact_email: bid.contact_email,
                    sq_foot: bid.sq_foot,
                    labor_hours: bid.labor_hours,
                    labor_rate: bid.labor_rate,
                    percentage_on_top: bid.percentage_on_top,
                    extra_charges: bid.extra_charges,
                    won: bid.won,
                };
            },
            calculateTotal(m) {
                return m.price * m.quantity;
            },
            back() {
                window.location = '/project/' + this.bid.project.id;
            },
            markWon(){
                 let self = this;
                self.form.won = 1;
                axios.put(`/api/bid/${this.bid.id}`,this.form)
                    .then(res => {
                        self.$parent.$refs.mytoast.s(res.data.message);
                        self.initForm(res.data.bid);
                    })
                    .catch(function (err) {
                        $.each(err.response.data.errors, function (index, error) {
                            self.$parent.$refs.mytoast.e(error[0]);
                        });
                    });
            },
            unmarkWon() {
                let self = this;
                self.form.won = 0;
                axios.put(`/api/bid/${this.bid.id}`,this.form)
                    .then(res => {
                        self.$parent.$refs.mytoast.s(res.data.message);
                        self.initForm(res.data.bid);
                    })
                    .catch(function (err) {
                        $.each(err.response.data.errors, function (index, error) {
                            self.$parent.$refs.mytoast.e(error[0]);
                        });
                    });
            },
            // deleteBid(){
            //     axios.delete(`/api/bid/${this.bid.id}`).then(data => {
            //         if (data.data.status === "success") {
            //             window.location = "/project/"+this.bid.project.id;
            //         } else {
            //             Swal.fire({
            //                 title: "Error",
            //                 text: data.data.message,
            //                 type: "error",
            //                 confirmButtonText: "OK"
            //             });
            //         }
            //     });
            //}
        },
    }
</script>
