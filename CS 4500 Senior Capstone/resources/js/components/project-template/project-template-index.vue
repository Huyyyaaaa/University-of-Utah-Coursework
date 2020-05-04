<template>
  <div>
    <div class="card container">
      <div class="card-header">
        <h5>Project Templates</h5>
        <a class="btn btn-primary" href="/project-template/create" tour-step="1">Create</a>
      </div>
      <br />
      <div class="card-body">
        <h4 v-if="templates.length <= 0" class="text-center">No project templates added yet</h4>
        <table v-else id="template-table" class="table table-hover py-4">
          <thead>
            <tr>
              <th>Template</th>
              <th>Description</th>
              <th>Materials</th>
              <th>Creator</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="t in templates" v-bind:key="t.id" v-bind:tempid="t">
              <td>{{t.name}}</td>
              <td>{{t.description | strLimit(20)}}</td>
              <td>
                <span v-for="m in t.materials.slice(0,5)" v-bind:key="m.id">{{m.name}},</span>
              </td>
              <td>{{t.created_by.name}}</td>
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
    <v-tour name="myTour" :steps="tourSteps" :options="tourOptions"></v-tour>
  </div>
</template>

<script>
export default {
  name: "project-template-index",
  data() {
    return {
      templates: null,
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
          content: 'click "Create" to create a Template'
        }
      ]
    };
  },
  mounted() {
    this.getTemplates();
  },
  updated() {
    if (!this.checkTour()) {
      this.$tours["myTour"].start();
    }

    $("#template-table").dtable({
      pageLimit: 25
    });
  },
  methods: {
    getTemplates() {
      axios.get("/api/project-template").then(response => {
        this.templates = response.data.data;
      });
    }
  }
};
</script>
