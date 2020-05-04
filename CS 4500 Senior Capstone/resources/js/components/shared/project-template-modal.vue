<template>
    <div id="projectTemplateModal" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header card-header">
                    <h5>Project templates</h5>
                    <button @click="cancel" type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div>
                        <table id="project-template-modal-table" class="table table-hover">
                            <thead>
                            <tr>
                                <th>Name</th>
                                <th>Materials</th>
                                <th></th>
                            </tr>
                            </thead>
                            <tbody>
                            <tr v-for="template in templates" :key="template.id">
                                <td>{{template.name}}</td>
                                <td>
                                    <span v-for="material in template.materials.slice(0,5)" :key="material.id">{{material.name}}, </span>
                                </td>
                                <td>
                                    <button v-if="!templateAddedAlready(template)" class="btn btn-primary btn-sm"
                                            @click="addOrRemoveTemplate(template)">
                                        <i class="fas fa-plus"></i>
                                    </button>
                                    <button v-else class="btn btn-danger btn-sm" @click="addOrRemoveTemplate(template)">
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
        name: "project-template-modal",
        data() {
            return {
                templates: null,
                templateMaterials:[],
                templatesExport: [],
                tableLoaded:false
            }
        },
        updated(){
            if(!this.tableLoaded) {
                $("#project-template-modal-table").dtable({pageLimit: 5})
                this.tableLoaded = true
            }
        },
        methods: {
            getProjectTemplates() {
                axios.get('/api/project-template')
                    .then(res => {
                        this.templates = res.data.data;
                    });
            },
            addTemplate(template) {
                this.templatesExport.push(template);
            },
            removeTemplate(template) {
                this.templatesExport = this.templatesExport.filter(function (addedTemplate) {
                    return addedTemplate.id !== template.id;
                });
            },
            addOrRemoveTemplate(template) {
                this.templateAddedAlready(template) ? this.removeTemplate(template) : this.addTemplate(template);
            },
            templateAddedAlready(template) {
                return this.templatesExport.filter(_template => template.id === _template.id).length > 0;
            },
            close() {
                let result = [];
                $.each(this.templatesExport, function (tem_index, template) {
                    $.each(template.materials, function (mat_index, material) {
                        //material.quantity = 1;
                        Vue.set(material, 'quantity',1);
                        result.push(material);
                    });
                });
                Bus.$emit('projectTemplatesAdded', result);
                this.templatesExport = [];
            },
            cancel() {
                this.templatesExport = [];
            }
        }
    }
</script>
