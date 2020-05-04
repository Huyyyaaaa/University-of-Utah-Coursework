<template>
  <div id="taskcreateModal" class="modal" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header card-header">
          <h5>New Task</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <div class="form-group">
            <label for="task">Description</label>
            <textarea class="form-control" v-model="form.description"></textarea>
          </div>
          <div class="form-group">
            <label>Due at</label>
            <datetime
              input-id="due-date-create"
              input-class="form-control"
              v-model="form.due_at"
              type="datetime"
              use12-hour
            ></datetime>
          </div>
          <div class="form-group">
            <label>Project</label>
            <select id="project-select-create" class="form-control">
              <option value="0">-- No Project --</option>
              <option
                v-for="project in projects"
                :key="project.id"
                :value="project.id"
              >{{project.name}}</option>
            </select>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-dismiss="modal" @click="create">Create</button>
          <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "task-create-modal",
  props:['project'],
  data() {
    return {
      projects: null,
      form: {
        description: null,
        project_id: null,
        due_at: null,
        completed: 0
      }
    };
  },
  mounted() {
    this.getProjects();
  },
  updated(){
    if(this.project){
          this.form.project_id = this.project
          $("#project-select-create").val(this.project)
          }
  },
  methods: {
    getProjects() {
      axios
        .get("/api/project")
        .then(res => {
          this.projects = res.data.data;
          
        })
        .catch(err => {
          console.log(err);
        });
    },
    create() {
      let self = this;

      let pval = $("#project-select-create").val()
      if(pval != 0){
        this.projects.forEach(p => {
          if(p.id == pval){
              this.form.project_id = p.id
          }
        });
      }

      
      
      let date = moment(this.form.due_at).format('MMMM DD, YYYY, HH:mm')
      this.form.due_at = date
      
      if(this.form.due_at == "Invalid date") this.form.due_at = ""
      console.log(this.form.due_at)
      axios
        .post("/api/task", this.form)
        .then(res => {
          this.form.description = null
          this.form.due_at = null
          this.form.project_id = null
          Bus.$emit('taskCreated', res.data.task);

          this.$parent.$parent.$refs.mytoast.s(res.data.message);

        })
        .catch(err => {
          $.each(err.response.data.errors, function(index, error) {
            self.$parent.$parent.$refs.mytoast.e(error[0]);
          });
        });
    }
  }
};
</script>
