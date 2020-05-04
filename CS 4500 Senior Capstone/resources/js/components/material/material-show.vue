<template>
    <div>
        <div class="row">
            <div class="col-7">
                <div class="card container py-4" tour-step="1">
                    <div v-if="!materialEdit">
                        <div class="card-header">
                            <h5>{{form.name}}</h5>
                            <button class="btn btn-warning" @click="editMaterial">
                                <i class="fas fa-edit"></i>
                            </button>
                        </div>
                        <div class="container pt-4">
                            <h5 class="sub-header">Description</h5>
                            <div>{{form.description}}</div>
                        </div>
                        <div class="container pt-4">
                            <h5 class="sub-header">Price</h5>
                            <div class="price">{{form.price | formatPrice}}</div>
                        </div>
                        <div class="container pt-4">
                            <h5 class="sub-header">Square Feet Coverage</h5>
                            <div>{{form.sq_foot_coverage}} sq. ft</div>
                        </div>
                    </div>
                    <div v-else>
                        <div class="card-header">
                            <h5>Edit Material</h5>
                            <button class="btn btn-success" @click="saveMaterial">
                                <i class="fas fa-save"></i>
                            </button>
                        </div>
                        <div class="container">
                            <form>
                                <div class="form-group">
                                    <label for="name">Name</label>
                                    <input id="name" type="text" class="form-control" v-model="form.name">
                                </div>
                                <div class="form-group">
                                    <label for="description">Description</label>
                                    <textarea id="description" class="form-control"
                                              v-model="form.description"></textarea>
                                </div>
                                <div class="form-group">
                                    <label for="price">Price</label>
                                    <money id="price" class="form-control" v-model="form.price"></money>
                                </div>
                                <div class="form-group">
                                    <label for="sq_feet_coverage">Square Feet Covered</label>
                                    <input type="text" id="sq_feet_coverage" class="form-control"
                                           v-model="form.sq_foot_coverage">
                                </div>
                            </form>
                            <button class="btn btn-danger" @click="deleteMaterial">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-5">
                <div class="card container py-4">
                    <h5 class="card-header">Templates Using {{material.name}}</h5>
                    <div>
                        <h5 v-if="form.project_temp <= 0" class="text-center my-5">No templates using {{material.name}}</h5>
                        <table v-else id="template-table" class="table table-hover py-4">
                            <thead>
                            <tr>
                                <th>Name</th>
                                <th></th>
                            </tr>
                            </thead>
                            <tbody>

                            <tr v-for="(t, index) in form.project_temp" v-bind:key="index">
                                <td>{{t.name}}</td>
                                <td>
                                    <a class="btn btn-primary btn-sm" :href="`/project-template/${t.id}`">
                                        <i class="fas fa-eye"></i>
                                    </a>
                                </td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
    </div>
</template>

<script>
    import {Money} from 'v-money'

    export default {
        name: "material-show",
        components: {Money},
        props: ['material'],
        data() {
            return {
                componentName: 'material-show',
                materialEdit: false,
                tableLoaded: false,
                form: {
                    name: this.material.name,
                    description: this.material.description,
                    price: this.material.price,
                    material_id: this.material.id,
                    sq_foot_coverage: this.material.sq_foot_coverage,
                    project_temp: this.material.project_templates
                },
                tourOptions: {
                        useKeyboardNavigation: false,
                        labels: {
                            buttonSkip: 'Skip',
                            buttonPrevious: 'Previous',
                            buttonNext: 'Next',
                            buttonStop: 'Ok'
                        },
                        
                },
                tourSteps:[
                    {
                        target: '[tour-step="1"]',
                        header:{
                            title: "Hooray!"
                        },
                        content: 'you have created a material.'
                    },
                    {
                        target: '[side-step="2"]',
                        content: 'Click on "Templates" to make a Project Template',
                        params:{
                            placement:"right"
                        }
                    }
                    
                ]

            }

        },
        mounted(){
            if(!this.checkTour()){
              this.$tours['myTour'].start()
            }
        },
        updated() {

            if (!this.tableLoaded) {
                $("#template-table").dtable({
                    pageLimit: 5
                });
                this.tableLoaded = true
            }
        },
        methods: {
            editMaterial() {
                this.temp = this.form.price;
                this.temp = "$" + ((this.temp / 100).toFixed(2));
                this.materialEdit = true;
            },
            saveMaterial() {
                this.materialEdit = false;

                let self = this;
                axios.put(`/api/material/${this.material.id}`, this.form)
                    .then(res => {
                        self.$parent.$refs.mytoast.s(res.data.message);
                    })
                    .catch(function (err) {
                        $.each(err.response.data.errors, function (index, error) {
                            self.$parent.$refs.mytoast.e(error[0]);
                        });
                    });
            },

            deleteMaterial() {
                axios.delete(`/api/material/${this.material.id}`)
                    .then((data) => {
                        if (data.data.status === 'success') {
                            window.location = '/material';
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: data.data.message,
                                type: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    });
            },
        },
    }
</script>
