<template>
    <div>
        <div class="card container">
            <div class="card-header">
                <h5>Materials List</h5>
                <a class="btn btn-primary" href="/material/create" tour-step="3">Create</a>
            </div>
            <div class="card-body">
                <h4 v-if="materials.length <=0 " class="text-center">No materials added yet</h4>
                <table v-else id="material-table" class="table table-hover py-4">
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="material in materials" v-bind:key="material.id" v-bind:tempid="material">
                        <td>{{material.name}}</td>
                        <td>{{material.description | strLimit(50)}}</td>
                        <td class="price">{{material.price | formatPrice}}</td>
                        <td>
                            <a class="btn btn-primary btn-sm" :href="`/material/${material.id}`">
                                <i class="fas fa-eye"></i>
                            </a>
                        </td>
                    </tr>
                    </tbody>
                </table>
            </div>
            <div class="card-footer">
                <p>Please format your file to upload in the following way:</p>
                <table id="fileFormatExample" class="table">
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th>Sq Foot Coverage</th>
                        <th></th>
                    </tr>
                    </thead>

                    <tr>
                        <th>Material1</th>
                        <th>This is the first Material</th>
                        <th>1.00</th>
                        <th>5</th>
                    </tr>
                    <tr>
                        <th>Material2</th>
                        <th>This is the second Material</th>
                        <th>2.22</th>
                        <th>10</th>
                    </tr>
                    <tr>
                        <th>Material3</th>
                        <th>This is the third Material</th>
                        <th>0.30</th>
                        <th>10</th>
                    </tr>
                    <tbody>
                    </tbody>
                </table>
                <button class="btn btn-primary" onclick="document.getElementById('fileInput').click()">
                    <span>{{ fileName }}</span>
                    <input @change="prepareFile" type="file" id="fileInput"
                           accept=".csv" hidden>
                </button>
                <button class="btn btn-primary" name="load" id="load" @click="create" :disabled="!fileLoaded">
                    Load
                </button>
                <a href="/material/1/export" class="btn btn-outline-secondary">Export to CSV</a>
            </div>
        </div>
        <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
    </div>
</template>

<script>
    export default {

        name: "material-index",
        data() {
            return {
                materials: null,
                tableLoaded: false,
                materialImportForm: new FormData(),
                fileLoaded: false,
                fileName: "Choose a File",
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
                        target: '[tour-step="3"]',
                        content: 'Click on "Create" to add a new Material'
                    },
                    
                ]
            }
        },
        mounted() {
            this.getMaterials();
            
        },
        updated() {
            if(!this.checkTour()){
              this.$tours['myTour'].start()
            }

            if (!this.tableLoaded) {
                $("#material-table").dtable({
                    pageLimit: 10
                });
                this.tableLoaded = true;
            }

        },
        methods: {
            getMaterials() {
                axios.get('/api/material')
                    .then(response => {
                        this.materials = response.data.data;
                    });
            },
            prepareFile(e) {
                let files = e.target.files || e.dataTransfer.files;
                if (!files.length)
                    return;
                this.materialImportForm.append("file", files[0]);
                this.fileLoaded = true;
                this.fileName = files[0].name;
            },
            create() {
                if (!this.fileLoaded) {
                    this.$parent.$refs.mytoast.w('Please select a file first!');
                    return;
                }

                let self = this;
                axios.post('/api/material/import', this.materialImportForm)
                    .then((data) => {
                        if (data.data.status === 'success') {
                            self.$parent.$refs.mytoast.s(data.data.message);
                            self.materials = data.data.materials;
                        } else {
                            self.$parent.$refs.mytoast.e(data.data.message);
                        }
                        self.materialImportForm = new FormData();
                        document.getElementById("fileInput").value = "";
                        self.fileName = "Choose a File";
                        self.fileLoaded = false;
                    });
            },
        }
    }
</script>



