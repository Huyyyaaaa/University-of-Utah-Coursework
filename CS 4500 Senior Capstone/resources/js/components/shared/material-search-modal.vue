<template>
    <div id="materialModal" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header card-header">
                    <h5>Materials</h5>
                    <button @click="cancel" type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div>
                        <table id="material-modal-table" class="table table-hover">
                            <thead>
                            <tr>
                                <th>Name</th>
                                <th>Price</th>
                                <th></th>
                            </tr>
                            </thead>
                            <tbody>
                            <tr v-for="m in materials">
                                <td>{{m.name}}</td>
                                <td class="price">{{m.price | formatPrice}}</td>
                                <td>
                                    <button v-if="!materialAddedAlready(m)" class="btn btn-primary btn-sm"
                                            @click="addOrRemoveMaterial(m)">
                                        <i class="fas fa-plus"></i>
                                    </button>
                                    <button v-else class="btn btn-danger btn-sm" @click="addOrRemoveMaterial(m)">
                                        <i class="fas fa-minus"></i>
                                    </button>
                                </td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button @click="cancel" type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button @click="close" type="button" class="btn btn-primary" data-dismiss="modal">Add</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "material-search-modal",
        data() {
            return {
                materials: null,
                materialsExport: [],
                tableLoaded: false
            }
        },
        updated(){
            if(!this.tableLoaded){
                $("#material-modal-table").dtable({
                    pageLimit: 5
                })
                this.tableLoaded = true
            }

        },
        methods: {
            getMaterials() {
                axios.get('/api/material')
                    .then(res => {
                        this.materials = res.data.data;
                    })
            },
            addMaterial(material) {
                this.materialsExport.push(material);
            },
            removeMaterial(material) {
                this.materialsExport = this.materialsExport.filter(function (addedMaterial) {
                    return addedMaterial.id !== material.id;
                });
            },
            addOrRemoveMaterial(material) {
                this.materialAddedAlready(material) ? this.removeMaterial(material) : this.addMaterial(material);
            },
            materialAddedAlready(material) {
                return this.materialsExport.filter(_material => material.id === _material.id).length > 0;
            },
            close() {
                let result = [];
                $.each(this.materialsExport, function (index, material) {
                    //material.quantity = 1;
                    Vue.set(material, 'quantity', 1);
                    result.push(material);
                });
                Bus.$emit('materialsAdded', result);
                this.materialsExport = [];
            },
            cancel() {
                this.materialsExport = [];
            }
        }
    }
</script>
