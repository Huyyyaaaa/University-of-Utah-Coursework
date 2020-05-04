<template>
  <div>
    <div class="row">
      <div class="col-10 offset-1">
        <div class="card container py-4" tour-step="1">
          <div class="card-header">
            <h5>{{template.name}}</h5>
            <a v-if="!templateEdit" class="btn btn-warning" @click="templateEdit = true">
              <i class="fas fa-pencil-alt"></i>
            </a>
          </div>
          <br />
          <div class="row">
            <div class="col">
              <div v-if="!templateEdit">
                <h5 class="sub-header">Description</h5>
                <div>{{template.description}}</div>
              </div>
              <div v-else>
                <div class="form-group">
                  <label for="name">Template Name</label>
                  <input type="text" name="name" v-model="form.name" class="form-control" />
                </div>
                <div class="form-group">
                  <label for="description">Template Description</label>
                  <textarea name="description" v-model="form.description" class="form-control"></textarea>
                </div>
              </div>
              <br />
              <button
                v-if="templateEdit"
                @click="$refs.materialModal.getMaterials"
                class="btn btn-primary"
                data-toggle="modal"
                data-target="#materialModal"
              >
                <i class="fas fa-plus"></i> Materials
              </button>
              <div>
                <h5
                  v-if="form.materials.length <= 0"
                  class="text-center"
                >No materials added to template</h5>
                <table
                  v-else
                  id="material-project-template-table"
                  class="table table-hover text-center"
                >
                  <thead>
                    <tr>
                      <th>Material</th>
                      <th>Quantity</th>
                      <th>Price</th>
                      <th>Total</th>
                      <th v-if="templateEdit"></th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-bind:key="index" v-for="(m, index) in form.materials">
                      <td>{{m.name}}</td>
                      <td v-if="!templateEdit">{{m.quantity}}</td>
                      <td v-else>
                        <input
                          type="number"
                          class="form-control"
                          v-model="form.materials[index].quantity"
                        />
                      </td>
                      <td class="price">{{m.price | formatPrice}}</td>
                      <td class="price">{{calculateTotal(m) | formatPrice}}</td>
                      <td v-if="templateEdit">
                        <button class="btn btn-danger btn-sm" @click="deleteMaterial(index)">
                          <i class="fas fa-trash"></i>
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col">
              <a class="btn btn-success" v-if="templateEdit" @click="saveTemplate()">
                <i class="fas fa-save"></i>
              </a>
              <a v-if="templateEdit" class="btn btn-danger" @click="deleteTemplate()">
                <i class="fas fa-trash-alt"></i>
              </a>
              <div>
                <small>created on {{template.created_at}}</small>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <material-search-modal ref="materialModal"></material-search-modal>
    <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
  </div>
</template>
<script>
import MaterialSearchModal from "../shared/material-search-modal";

export default {
  name: "project-template-show",
  components: { MaterialSearchModal },
  props: ["template"],
  data() {
    return {
      templateEdit: false,
      tableLoaded: false,
      form: {
        materials: [],
        name: null,
        description: null
      },
      tourOptions: {
        useKeyboardNavigation: false,
        labels: {
          buttonSkip: "Skip",
          buttonPrevious: "Previous",
          buttonNext: "Next",
          buttonStop: "Ok"
        }
      },
      tourSteps: [
        {
          target: '[tour-step="1"]',
          header: {
            title: "Hooray!"
          },
          content: "you have created a Template!"
        },
        {
          target: '[side-step="3"]',
          content: 'Click on "Projects" to make a Project',
          params: {
            placement: "right"
          }
        }
      ]
    };
  },
  created() {
    Bus.$on("materialsAdded", data => {
      this.addToMaterials(data);
    });
  },
  mounted() {
    if (!this.checkTour()) {
      this.$tours["myTour"].start();
    }
    this.form.materials = this.template.materials;
    this.form.name = this.template.name;
    this.form.description = this.template.description;
  },
  updated() {
      if(!this.tableLoaded){
      $("#material-project-template-table").dtable({ pageLimit: 5 });
      this.tableLoaded = true
      }

  },
  methods: {
    materialAddedAlready(material) {
      return (
        this.form.materials.filter(_material => material.id === _material.id)
          .length > 0
      );
    },
    addToMaterials(data) {
      let self = this;
      $.each(data, function(index, material) {
        if (!self.materialAddedAlready(material)) {
          self.form.materials.push(material);
        }
      });
    },
    calculateTotal(m) {
      return m.price * m.quantity;
    },
    deleteMaterial(idx) {
      this.tableLoaded = false
      this.form.materials.splice(idx, 1);
    },
    saveTemplate() {
      this.templateEdit = false;
      let self = this;
      axios
        .put(`/api/project-template/${this.template.id}`, this.form)
        .then(res => {
          self.$parent.$refs.mytoast.s(res.data.message);
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            self.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    deleteTemplate() {
      axios
        .delete(`/api/project-template/${this.template.id}`)
        .then(() => {
          window.location = "/project-template";
        })
        .catch(() => {
          Swal.fire({
            title: "Error",
            text: "Unable to Delete Project Template",
            type: "error",
            confirmButtonText: "OK"
          });
        });
    }
  }
};
</script>

<style scoped>
</style>
