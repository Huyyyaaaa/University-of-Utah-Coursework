<template>
  <div id="task-modal" class="modal" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="card-header text-center">
          <h5 class="text-center">Tasks</h5>
        </div>
        <div class="modal-body">
           <a href="#" v-if="!showCompleted"  @click="updateCompleted">Show Completed</a>
           <a href="#" v-else @click="updateCompleted">Show Uncompleted</a>
           <br>
          <p v-if="!uncompleted" class="text-center">You're all caught up!</p>
          <div v-else>
            <div v-if="overdueTasks.length > 0">
              <h6 class="task-due">overdue</h6>
              <table>
                <colgroup>
                  <col width="20%" />
                  <col width="80%" />
                </colgroup>
                <tbody>
                  <tr v-for="(task, idx) in overdueTasks" :key="task.id">
                    <td>
                       <i v-if="task.completed" @click="markComplete(task,idx,'overdue')" class="fas fa-check-circle"></i>
                      <i v-else @click="markComplete(task, idx, 'overdue')" class="far fa-circle"></i>
                    </td>
                    <td>{{task.description | strLimit(25)}}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div v-if="todayTasks.length > 0">
              <h6 class="task-due">today</h6>
              <table>
                <colgroup>
                  <col width="20%" />
                  <col width="80%" />
                </colgroup>
                <tbody>
                  <tr v-for="(task, idx) in todayTasks" :key="task.id">
                    <td>
                      <i v-if="task.completed" @click="markComplete(task,idx,'today')" class="fas fa-check-circle"></i>
                      <i v-else @click="markComplete(task, idx, 'today')" class="far fa-circle"></i>
                    </td>
                    <td>{{task.description | strLimit(25)}}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div v-if="tomorrowTasks.length > 0">
              <h6 class="task-due">tomorrow</h6>
              <table>
                <colgroup>
                  <col width="20%" />
                  <col width="80%" />
                </colgroup>
                <tbody>
                  <tr v-for="(task,idx) in tomorrowTasks" :key="task.id">
                    <td>
                      <i v-if="task.completed" @click="markComplete(task,idx,'tomorrow')" class="fas fa-check-circle"></i>
                      <i v-else @click="markComplete(task, idx, 'tomorrow')" class="far fa-circle"></i>
                    </td>
                    <td>{{task.description | strLimit(25)}}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div v-if="laterTasks.length > 0">
              <h6 class="task-due">later</h6>
              <table>
                <colgroup>
                  <col width="20%" />
                  <col width="80%" />
                </colgroup>
                <tbody>
                  <tr v-for="(task,idx) in laterTasks" :key="task.id">
                    <td>
                      <i v-if="task.completed" @click="markComplete(task,idx,'later')" class="fas fa-check-circle"></i>
                      <i v-else @click="markComplete(task, idx, 'later')" class="far fa-circle"></i>
                    </td>
                    <td>{{task.description | strLimit(25)}}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-primary" data-target="#taskcreateModal" data-toggle="modal">Add Task</button>
          <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
    <task-create-modal ref="taskcreateModal"></task-create-modal>
  </div>
</template>

<script>
export default {
  name: "task-modal",
  data() {
    return {
      tasks: null,
      overdueTasks: [],
      todayTasks: [],
      tomorrowTasks: [],
      laterTasks: [],
      uncompleted: false,
      showCompleted: false
    };
  },
  created(){
    Bus.$on('taskCreated', data=>{
      this.tasks.push(data)
      this.overdueTasks = []
      this.todayTasks = []
      this.tomorrowTasks = []
      this.laterTasks = []
      this.orderTasks()
    })
  },
  mounted() {
    this.getUsersTasks();
    
  },
  methods: {
    getUsersTasks() {
      axios
        .get("/api/task")
        .then(res => {
          this.tasks = res.data.data;
          this.orderTasks();
        })
        .catch(err => {
          console.log(err);
        });
    },
    orderTasks() {
      const today = moment().format("MM/DD/YYYY");
      const tomorrow = moment(today)
        .add(1, "days")
        .format("MM/DD/YYYY");

      if (!this.showCompleted) {
        this.tasks.forEach(task => {
          if (!task.completed) {
            this.uncompleted = true;
            let date = moment(task.due_at).format("MM/DD/YYYY");

            if (moment(date).isBefore(today)) {
              this.overdueTasks.push(task);
            } else if (moment(date).isSame(today)) {
              this.todayTasks.push(task);
            } else if (moment(date).isSame(tomorrow)) {
              this.tomorrowTasks.push(task);
            } else {
              this.laterTasks.push(task);
            }
          }
        });
      } else {
        this.tasks.forEach(task => {
          this.uncompleted = true;
          let date = moment(task.due_at).format("MM/DD/YYYY");

          if (moment(date).isBefore(today)) {
            this.overdueTasks.push(task);
          } else if (moment(date).isSame(today)) {
            this.todayTasks.push(task);
          } else if (moment(date).isSame(tomorrow)) {
            this.tomorrowTasks.push(task);
          } else {
            this.laterTasks.push(task);
          }
        });
      }
    },
    markComplete(task, index, array) {

      if (task.completed) {
        Vue.set(task, "completed", 0);
      } else {
        Vue.set(task, "completed", 1);
      }

      //   switch (array) {
      //     case "overdue":
      //       this.overdueTasks.splice(index, 1);
      //       break;
      //     case "today":
      //       this.todayTasks.splice(index, 1);
      //       break;
      //     case "tomorrow":
      //       this.tomorrowTasks.splice(index, 1);
      //       break;
      //     case "later":
      //       this.laterTasks.splice(index, 1);
      //       break;
      //   }

      axios
        .put(`/api/task/${task.id}`, task)
        .then(res => {
          this.$parent.$refs.mytoast.s("Task successfully updated!");
            Bus.$emit('taskUpdated');
        })
        .catch(function(err) {
          $.each(err.response.data.errors, function(index, error) {
            this.$parent.$refs.mytoast.e(error[0]);
          });
        });
    },
    updateCompleted(){
        this.showCompleted = !this.showCompleted
        this.overdueTasks = []
        this.todayTasks = []
        this.tomorrowTasks = []
        this.laterTasks = []
        this.orderTasks()
    }
  }
};
</script>
<style scoped>
.modal-dialog {
  right: 0;
  top: 0;
  position: absolute;
  margin: 0;
  padding: 0;
  border-radius: 0;
  height: 100%;
  width: 350px;
}
.modal-content {
  position: relative;
  display: flex;
  flex-direction: column;
  width: 100%;
  pointer-events: auto;
  background-color: #fff;
  background-clip: padding-box;
  /* border: 1px solid rgba(0, 0, 0, 0.2); */
  border-radius: 0;
  outline: 0;
  height: 100%;
  border: none;
}
.task-due {
  font-size: 10px;
  text-transform: uppercase;
  color: #b3b3b3;
  border-bottom: 1px solid #b3b3b3;
  width: 100%;
  display: block;
}
.fa-circle {
  color: #b3b3b3;
}
.fa-check-circle{
    color: #3cd660;
}
table {
  margin: 0;
  padding: 0;
  width: 100%;
}
i:hover {
  cursor: pointer;
}
a{
    float:right;
    font-size: 10px;
    color: #b3b3b3;
}
a:hover{
    color: blue;
    text-decoration: underline;
    cursor: pointer;
}
</style>
