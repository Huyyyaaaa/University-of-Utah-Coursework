<template>
    <div class="row">
        <div class="col">
            <div class="card container py-4" >
                <div class="card-header">
                    <h5>Create Material</h5>
                </div>
                <br>
                <form class="container">
                    <div class="form-group">
                        <label for="name">Name</label>
                        <input type="text" id="name" name="name" class="form-control" v-model="form.name">
                    </div>
                    <div class="form-group">
                        <label for="description">Description</label>
                        <textarea id="description" name="description" class="form-control"
                                  v-model="form.description"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="price">Price</label>
                        <money id="price" class="form-control" v-model="form.price"></money>
                    </div>
                    <div class="form-group">
                        <label for="sq_foot_coverage">Square Feet Coverage</label>
                        <input type="text" id="sq_foot_coverage" name="sq_foot_coverage" class="form-control" v-model="form.sq_foot_coverage">
                    </div>
                    <div class="form-group">
                        <button class="btn btn-primary" @click.prevent="create" tour-step="1">Create</button>
                        <a href="/material" class="btn btn-secondary">Cancel</a>
                    </div>
                </form>
            </div>
        </div>
         <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
    </div>
</template>

<script>
 import {Money} from 'v-money'
     export default {
        name: "material-create",
        components: {Money},
        data() {
            return {
                form: {
                    name: null,
                    description: null,
                    price: null,
                    sq_foot_coverage: null,
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
                        content: 'Fill the form above and click "Create" to save the Material.'
                    },
                    
                ]
            }
        },
        mounted(){
            if(!this.checkTour()){
              this.$tours['myTour'].start()
            }
        },
        methods: {
            create() {
    
                let self = this;
                axios.post(`/api/material`, this.form)
                    .then(res => {
                        window.location = '/material/' + res.data.material.id
                    })
                    .catch(function (err) {
                        $.each(err.response.data.errors, function (index, error) {
                            self.$parent.$refs.mytoast.e(error[0]);
                        });
                    });
            },
        }
    }
</script>
