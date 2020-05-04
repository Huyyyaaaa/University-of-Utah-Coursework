<template>
  <div>
    <div class="card container">
      <div class="card-header">
        <h5>Projects</h5>
        <a class="btn btn-primary" href="/project/create" tour-step="1">Create</a>
      </div>
      <div class="card-body">
        <h4
          v-if="!projectsLoading && projects.length <= 0"
          class="text-center"
        >No projects added yet</h4>
        <table v-else id="project-index-table" class="table table-hover py-4">
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="p in projects" v-bind:key="p.id">
              <td>{{p.name}}</td>
              <td>{{p.description | strLimit(50)}}</td>
              <td>
                <a class="btn btn-primary btn-sm" :href="`/project/${p.id}`">
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
  name: "project-index",
  data() {
    return {
      projects: null,
      projectsLoading: true,
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
          content: 'click "Create" to create a Project'
        }
      ]
    };
  },
  mounted() {
    this.getProjects();
  },
  updated() {
    if (!this.checkTour()) {
      this.$tours["myTour"].start();
    }

    $("#project-index-table").dtable();
  },
  methods: {
    getProjects() {
      let self = this;
      axios.get("api/project").then(response => {
        self.projects = response.data.data;
        self.projectsLoading = false;
      });
    }
  }
};
</script>

