<template>
  <div>
    <div class="card container-fluid" tour-step="3">
      <div class="card-header">
        <h5>Tasks</h5>
        <button
          class="btn btn-primary"
          data-toggle="modal"
          data-target="#taskcreateModal"
          tour-step="1"
        >Create</button>
      </div>
      <div class="card-body" tour-step="2">
        <div class="row">
          <div class="col">
            <a id="show-complete-btn" href="#" @click="toggleShowCompleted">{{showCompletedBtnText}}</a>
            <h4 v-if="filteredTasks.length <= 0" class="text-center">You're all caught up!</h4>
            <table v-else id="task-index-table" class="table py-4">
              <thead>
                <th></th>
                <th>Description</th>
                <th>Due</th>
              </thead>
              <tbody>
                <tr v-for="(task,idx) in filteredTasks" :key="task.id" @click="showDetails(task)">
                  <td>
                    <i
                      v-if="task.completed"
                      @click="markComplete(task,idx)"
                      class="fas fa-check-circle"
                    ></i>
                    <i v-else @click="markComplete(task, idx)" class="far fa-circle"></i>
                  </td>
                  <td class="desc">{{task.description | strLimit(50)}}</td>
                  <td class="date">{{formatDate(task.due_at)}}</td>
                </tr>
              </tbody>
            </table>
          </div>
          <div id="task-detail-col" class="col">
            <div id="taskSelectText" v-if="!taskSelected">
              <h5>Select a task for details</h5>
            </div>
            <div class="container" v-else>
              <div class="row">
                <div class="col">
                  <a href="#" class="btn btn-success" style="float: right;" @click="saveTask">
                    <i class="fas fa-save"></i>
                  </a>
                </div>
              </div>

              <br />
              <div class="row">
                <div class="col">
                  <label for="desc">Description</label>
                  <textarea name="desc" class="form-control" v-model="currentTask.description"></textarea>
                </div>
                <br />
              </div>
              <div class="row">
                <div class="col-6">
                  <label>Due at</label>
                  <datetime
                    input-id="due-date"
                    input-class="form-control"
                    v-model="currentTask.due_at"
                    type="datetime"
                    use12-hour
                  ></datetime>
                </div>
                <div class="col-6">
                  <label>Project</label>
                  <select id="project-select" v-if="currentTask.project != null" class="form-control" v-model="currentTask.project.id">
                    <option
                      v-for="project in projects"
                      :key="project.id"
                      :value="project.id"
                    >{{project.name}}</option>
                  </select>
                  <select v-else id="project-select" class="form-control">
                    <option value="0" selected>--No Project--</option>
                    <option
                      v-for="project in projects"
                      :key="project.id"
                      :value="project.id"
                    >{{project.name}}</option>
                  </select>
                </div>
              </div>
              <br />
              <div class="row">
                <div class="col-6">
                  <a href="#" class="btn btn-danger" @click="deleteTask">
                    <i class="fas fa-trash-alt"></i>
                  </a>
                </div>
                <div id="created" class="col-6">
                  <div>
                    <span>{{currentTask.created_at}}</span>
                    <span>{{currentTask.created_by.name}}</span>
                  </div>
                  <div class="img-container">
                    <img class="created-img" :src="currentTask.created_by.photo_url" />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <task-create-modal ref="taskcreateModal"></task-create-modal>
    <v-tour name="myTour" :steps="tourSteps" :options="tourOptions" :callbacks="tourCallbacks"></v-tour>
  </div>
</template>

<script>
export default {
  name: "task-index",
  data() {
    return {
      tasks: null,
      projects: null,
      filteredTasks: [],
      taskSelected: false,
      showCompleted: false,
      showCompletedBtnText: "Show Completed",
      currentTask: {
        description: null,
        due_at: null,
        created_at: null,
        completed: null,
        created_by: null,
        id: null,
        project: null,
        team: null
      },
      tourOptions: {
        useKeyboardNavigation: false,
        labels: {
          buttonSkip: "Skip",
          buttonPrevious: "Previous",
          buttonNext: "Next",
          buttonStop: "Done"
        }
      },
      tourCallbacks: {
        onStop: this.stopTour,
        onSkip: this.stopTour
      },
      tourSteps: [
        {
          target: '[tour-step="1"]',
          content: 'click "Create" to create a Task'
        },
        {
          target: '[tour-step="2"]',
          content: "You will see all your Tasks here"
        },
        {
          target: '[tour-step="3"]',
          content: "You have finished the Tour!"
        }
      ]
    };
  },
  mounted() {
    this.getTasks();
    this.getProjects();
  },
  created() {
    Bus.$on("taskCreated", data => {
      this.filteredTasks.push(data)
    });
  },
  updated() {
    if (!this.checkTour()) {
      this.$tours["myTour"].start();
    }
  },
  methods: {
    getTasks() {
      axios
        .get("api/task")
        .then(res => {
          this.tasks = res.data.data;
          this.filterTasks();
        })
        .catch(err => {
          console.log(err);
        });
    },
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
    filterTasks() {
      if (!this.showCompleted) {
        this.tasks.forEach(task => {
          if (!task.completed) {
            this.filteredTasks.push(task);
          }
        });
      } else {
        this.tasks.forEach(task => {
          this.filteredTasks.push(task);
        });
      }

      this.filteredTasks.sort((a, b) =>
        moment(a.due_at).diff(moment(b.due_at))
      );
    },
    formatDate(date) {
      return moment(date).format('M/D/YY[\n]h:mm a');
    },
    toggleShowCompleted() {
      if (this.showCompleted == false) {
        this.showCompleted = true;
        this.showCompletedBtnText = "Hide Completed";
      } else {
        this.showCompleted = false;
        this.showCompletedBtnText = "Show Completed";
      }
      this.filteredTasks = [];
      this.filterTasks();
    },
    markComplete(task, index) {
      if (task.completed) {
        Vue.set(task, "completed", 0);
      } else {
        Vue.set(task, "completed", 1);
      }

      axios
        .put(`/api/task/${task.id}`, task)
        .then(res => {
          this.$parent.$refs.mytoast.s("Task successfully updated!");
          this.filteredTasks = [];
          this.taskSelected = false;
          this.filterTasks();
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            this.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    showDetails(task) {

      this.currentTask = null;
      this.taskSelected = true;
      if (task.project == null) {
        $("#project-select").val(0);
      }
      this.currentTask = task;
      this.currentTask.due_at = new Date(this.currentTask.due_at).toISOString();

    },
    saveTask() {
      let pval = $("#project-select").val();
      if (pval != 0) {
        this.projects.forEach(p => {
          if (p.id == pval) {
            this.currentTask.project_id = p.id;
            this.currentTask.project = p;
          }
        });
      }

      let date = moment(this.currentTask.due_at).format("MMMM DD, YYYY, HH:mm");
      this.currentTask.due_at = date;

      axios
        .put(`/api/task/${this.currentTask.id}`, this.currentTask)
        .then(res => {
          this.filteredTasks[this.filteredTasks.findIndex(x=> x.id == this.currentTask.id)] = res.data.task
          this.$parent.$refs.mytoast.s("Task successfully updated!");
          this.showDetails(this.currentTask);
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            this.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    deleteTask() {
      axios
        .delete(`/api/task/${this.currentTask.id}`, this.currentTask)
        .then(res => {
          this.$parent.$refs.mytoast.s("Task successfully deleted!");
          for (let i = 0; i < this.filteredTasks.length; i++) {
            if (this.currentTask.id == this.filteredTasks[i].id) {
              this.filteredTasks.splice(i);
              //filterTasks();
            }
          }
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            this.$parent.$refs.mytoast.e(error[0]);
          });
        });
    }
  }
};
</script>
<style scoped>
#taskSelectText {
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  position: absolute;
}
#task-detail-col {
  border-left: 1px solid rgba(55, 63, 67, 0.125);
}
.fa-circle {
  color: #b3b3b3;
}
.fa-check-circle {
  color: #3cd660;
}
i:hover {
  cursor: pointer;
}
tr:hover {
  cursor: pointer;
}
#show-complete-btn {
  float: right;
  font-size: 11px;
  padding: 5px;
}
#created div span {
  display: block;
  font-size: 11px;
  color: #b3b3b3;
}
#created div {
  display: inline-block;
}
.created-img{
  width: 35px;
  height: 35px;
  border-radius: 100px;
}
.img-container{
  top: 0;
  position: absolute;
  margin-left: 5px;
}
.date{
  white-space: pre;
  font-size: 12px;
}
</style>