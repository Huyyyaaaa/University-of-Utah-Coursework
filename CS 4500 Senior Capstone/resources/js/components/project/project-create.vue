<template>
  <div>
    <div class="row">
      <div class="col-6">
        <div class="card container py-4">
          <div class="card-header">
            <h5>Create a Project</h5>
          </div>

          <div class="form-group pt-2">
            <label for="name">Project Name</label>
            <input type="text" id="name" name="name" class="form-control" v-model="form.name" />
          </div>
          <div class="form-group">
            <label for="description">Description</label>
            <textarea
              id="description"
              name="description"
              class="form-control"
              v-model="form.description"
            ></textarea>
          </div>
          <div class="form-group">
            <label for="address">Address</label>
            <input
              type="text"
              id="address"
              name="address"
              class="form-control"
              v-model="form.address"
            />
          </div>
          <div class="form-group">
            <label for="city">City</label>
            <input type="text" id="city" name="city" class="form-control" v-model="form.city" />
          </div>
          <div class="form-group row">
            <div class="col">
              <label for="state">State</label>
              <input type="text" id="state" name="state" class="form-control" v-model="form.state" />
            </div>
            <div class="col">
              <label for="zip">Zip</label>
              <input type="text" id="zip" name="zip" class="form-control" v-model="form.zip" />
            </div>
          </div>
          <div class="form-group">
            <label for="clientname">Client Name</label>
            <input
              type="text"
              id="clientname"
              name="clientname"
              class="form-control"
              v-model="form.contact_name"
            />
          </div>
          <div class="form-group row">
            <div class="col">
              <label for="contactphone">Client Phone</label>
              <input
                type="text"
                id="contactphone"
                name="contactphone"
                class="form-control"
                v-model="form.contact_phone"
              />
            </div>
            <div class="col">
              <label for="contactemail">Client Email</label>
              <input
                type="text"
                id="contactemail"
                name="contactemail"
                class="form-control"
                v-model="form.contact_email"
              />
            </div>
          </div>
          <div class="form-group">
            <label for="sqft">Project Square Feet</label>
            <input type="text" id="sqft" name="sqft" class="form-control" v-model="form.sq_foot" />
          </div>
          <div class="form-group">
            <label for="labor_hours">Labor Hours</label>
            <input
              type="text"
              id="labor_hours"
              name="labor_hours"
              class="form-control"
              v-model="form.labor_hours"
            />
          </div>
          <div class="form-group">
            <label for="labor_rate">Labor Rate</label>
            <input
              type="text"
              id="labor_rate"
              name="labor_rate"
              class="form-control"
              v-model="form.labor_rate"
            />
          </div>
          <div class="form-group">
            <label for="percentage_on_top">Percentage on Top</label>
            <input
              type="text"
              id="percentage_on_top"
              name="percentage_on_top"
              class="form-control"
              v-model="form.percentage_on_top"
            />
          </div>
          <hr />
          <div class="form-group">
            <button class="btn btn-primary" @click="create" tour-step="1">Create</button>
            <button class="btn btn-secondary" @click="cancel">Cancel</button>
          </div>
        </div>
      </div>
      <div class="col-6">
        <div class="card container py-4">
          <div class="card-header">
            <h5>Project Materials</h5>
          </div>
          <br />
          <div class="form-group">
            <button
              @click="$refs.materialModal.getMaterials()"
              class="btn btn-primary"
              data-toggle="modal"
              data-target="#materialModal"
            >
              <i class="fas fa-plus"></i> Materials
            </button>
            <button
              @click="$refs.projectTemplateModal.getProjectTemplates()"
              class="btn btn-primary"
              data-toggle="modal"
              data-target="#projectTemplateModal"
            >
              <i class="fas fa-plus"></i> from Template
            </button>
          </div>
          <h5 v-if="form.materials <= 0" class="text-center">No materials added</h5>
          <table v-else class="table table-hover">
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
        </div>
      </div>
    </div>
    <material-search-modal ref="materialModal"></material-search-modal>
    <project-template-modal ref="projectTemplateModal"></project-template-modal>
    <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
  </div>
</template>

<script>
export default {
  name: "project-create",
  data() {
    return {
      form: {
        name: null,
        description: null,
        materials: [],
        address: null,
        city: null,
        state: null,
        zip: null,
        contact_name: null,
        contact_phone: null,
        contact_email: null,
        sq_foot: null,
        labor_hours: null,
        labor_rate: null,
        percentage_on_top: null
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
          content:
            'Fill the form above and click "Create" to save the project. <br> Don\'t forget to add Materials!'
        }
      ]
    };
  },
  mounted() {
    if(!this.checkTour()){
              this.$tours['myTour'].start()
            }
  },
  created() {
    Bus.$on("materialsAdded", data => {
      this.form.materials = this.form.materials.concat(data);
    });
    Bus.$on("projectTemplatesAdded", data => {
      this.form.materials = this.form.materials.concat(data);
    });
  },
  methods: {
    create() {
      let self = this;
      axios
        .post(`/api/project/`, this.form)
        .then(res => {
          window.location = "/project/" + res.data.project.id;
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            self.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    calculateTotal(m) {
      return m.price * m.quantity;
    },
    deleteMaterial(idx) {
      this.form.materials.splice(idx, 1);
    },
    cancel() {
      window.location = "/project";
    }
  }
};
</script>
