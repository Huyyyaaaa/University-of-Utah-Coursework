<template>
  <div>
    <div class="row">
      <div class="col">
        <div class="card container py-4">
          <div class="card-header">
            <h5>Create Project Template</h5>
          </div>
          <br />
          <div class="container">
            <div class="form-group">
              <label for="name">Template Name</label>
              <input type="text" class="form-control" name="name" v-model="form.name" />
            </div>
            <div class="form-group">
              <label for="description">Template Description</label>
              <textarea class="form-control" name="description" v-model="form.description"></textarea>
            </div>
            <div class="form-group">
              <button
                @click="$refs.materialModal.getMaterials()"
                class="btn btn-primary"
                data-toggle="modal"
                data-target="#materialModal"
              >
                <i class="fas fa-plus"></i> Materials
              </button>
            </div>
          </div>
          <div>
            <table v-if="form.materials.length > 0" class="table table-hover">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Quantity</th>
                  <th>Price</th>
                  <th>Total</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                <tr v-bind:key="index" v-for="(m, index) in form.materials">
                  <td>{{m.name}}</td>
                  <td>
                    <input
                      type="number"
                      class="form-control"
                      v-model="form.materials[index].quantity"
                    />
                  </td>
                  <td class="price">{{m.price | formatPrice}}</td>
                  <td class="price">{{calculateTotal(m) | formatPrice}}</td>
                  <td>
                    <button class="btn btn-danger btn-sm" @click="deleteMaterial(index)">
                      <i class="fas fa-trash"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
            <h5 class="text-center" v-else>No Materials Added</h5>
          </div>
          <div class="form-group">
            <button class="btn btn-primary" @click="create" tour-step="1">Create</button>
            <button class="btn btn-secondary" @click="cancel">Cancel</button>
          </div>
        </div>
      </div>
    </div>
    <material-search-modal ref="materialModal"></material-search-modal>
    <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
  </div>
</template>

<script>
export default {
  name: "project-template-create",
  data() {
    return {
      form: {
        name: null,
        description: null,
        materials: []
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
          content: 'fill the form above and click "Create" to save the Template'
        }
      ]
    };
  },
  mounted() {
    if (!this.checkTour()) {
      this.$tours["myTour"].start();
    }
  },
  created() {
    Bus.$on("materialsAdded", data => {
      this.form.materials = this.form.materials.concat(data);
    });
  },
  methods: {
    create() {
      let self = this;
      console.log(this.form);
      axios
        .post(`/api/project-template/`, this.form)
        .then(res => {
          console.log(res);
          window.location = "/project-template/" + res.data.project_template.id;
        })
        .catch(function(err) {
          console.log(err);
          $.each(err.response.data.errors, function(index, error) {
            self.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    cancel() {
      window.location = "/project-template";
    },
    calculateTotal(m) {
      return m.price * m.quantity;
    },
    deleteMaterial(idx) {
      this.form.materials.splice(idx, 1);
    }
  }
};
</script>
