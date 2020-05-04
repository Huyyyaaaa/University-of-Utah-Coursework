<template>
    <div class="container-fluid">
        <div class="row">
            <div class="col-3">
                <div id="task-card" class="card count-card" @click="navigateTo('task')">
                    <i class="fas fa-tasks"></i>
                    <div class="count-card-total">
                        <h2>{{taskCount}}</h2>
                        <h3>Tasks</h3>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div id="project-card" class="card count-card" @click="navigateTo('project')">
                    <i class="fas fa-project-diagram"></i>
                    <div class="count-card-total">
                        <h2>{{projectCount}}</h2>
                        <h3>Projects</h3>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div id="template-card" class="card count-card" @click="navigateTo('project-template')">
                    <i class="fas fa-file-alt"></i>
                    <div class="count-card-total">
                        <h2>{{templateCount}}</h2>
                        <h3>Templates</h3>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div id="material-card" class="card count-card" @click="navigateTo('material')">
                    <i class="fas fa-hammer"></i>
                    <div class="count-card-total">
                        <h2>{{materialCount}}</h2>
                        <h3>Materials</h3>
                    </div>
                </div>
            </div>
        </div>

        <div class="card" tour-step="1">
            <div class="card-body">
                <div class="row">
                    <div class="col-2">
                        <h5 id="date-range">Date Range</h5>
                    </div>
                    <div class="col-10">
                        <div class="form-group form-inline">
                            <label for="start">Start date:</label>
                            <datetime v-model="from" type="date" input-class="form-control" zone='local' value-zone='local' :max-datetime="maxDate"></datetime>
                            <label for="end">End date:</label>
                            <datetime v-model="to" type="date" input-class="form-control" zone='local' value-zone='local' :max-datetime="maxDate"></datetime>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-6 justify-content-start">
                <div class="card chart">
                    <div class="card-header">
                        <h5>Percent of Income by Employee</h5>
                    </div>
                    <div class="card-body">
                        <div class="chart-container">
                            <doughnut-chart v-if="has_doughnut_data" :chart-data="datacollectionDoughnut"></doughnut-chart>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-6 justify-content-end">
                <div class="card chart">
                    <div class="card-header">
                        <h5>Income Over Time</h5>
                    </div>
                    <div class="card-body">
                        <div class="chart-container">
                            <line-chart
                                v-if="has_line_chart_data"
                                :chart-data="datacollectionLine"
                                legend="false"
                            ></line-chart>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <v-tour name="myTour" :steps="tourSteps" :options="tourOptions" :callbacks="tourCallbacks"></v-tour>
    </div>
</template>

<script>
    import LineChart from "./LineChart";
    import DoughnutChart from "./DoughnutChart";

    export default {
        props: ["tasks"],
        data() {
            return {
                taskCount: 0,
                materialCount: 0,
                projectCount: 0,
                templateCount: 0,
                taskSelected: false,
                datacollectionDoughnut: null,
                datacollectionLine: null,
                monthlyTeamRevenue: null,
                monthlyIndividualRevenue: null,
                from: null,
                to: null,
                maxDate: new Date().toISOString(),
                doughnutData: {
                    labels: null,
                    data: null
                },
                lineData: {
                    labels: null,
                    data: null
                },
                has_doughnut_data: false,
                has_line_chart_data: false,
                tourOptions: {
                    useKeyboardNavigation: false,
                    labels: {
                        buttonSkip: "Skip",
                        buttonPrevious: "Previous",
                        buttonNext: "Next",
                        buttonStop: "Ok"
                    }
                },
                tourCallbacks: {
                    onSkip: this.stopTour
                },
                tourSteps: [
                    {
                        target: '[tour-step="1"]',
                        header: {
                            title: "Welcome to Contrack!"
                        },
                        content:
                            "This is your Dashboard. You will see important charts here for your team as you start using Contrack"
                    },
                    {
                        target: '[side-step="1"]',
                        content:
                            'To get started, let\'s add a material. Click "Materials" on the left!',
                        params: {
                            placement: "right"
                        }
                    }
                ],
                chartOptions: {
                    responsive: true,
                    maintainAspectRatio: false
                }
            };
        },
        components: {
            LineChart,
            DoughnutChart
        },
        watch: {
            from: _.debounce(function() {

                if(this.from > this.to) {
                    this.$parent.$refs.mytoast.e( "Start date can not exceed the current date");
                }
                else{
                    this.getMonthlyTeamRevenue();
                    this.getMonthlyIndividualRevenue();
                }
            }, 2000),
            to: _.debounce(function() {

                if (this.to > new Date().toISOString()) {
                    this.$parent.$refs.mytoast.e("End date can not exceed the current date");
                }
                else{
                    this.getMonthlyTeamRevenue();
                    this.getMonthlyIndividualRevenue();
                }
            }, 2000)
        },
        mounted() {

            if (!this.checkTour()) {
                this.$tours["myTour"].start();
            }

            this.getCountTotals();

            this.to = new Date().toISOString()
            this.from = new Date(moment().subtract(1,'month')).toISOString()

            this.getMonthlyTeamRevenue();
            this.getMonthlyIndividualRevenue();
        },

        methods: {
            navigateTo(path) {
                window.location.href = "/" + path;
            },
            getCountTotals() {
                //get tasks
                axios.get("/api/task").then(res => {
                    res.data.data.forEach(task => {
                        if (task.completed == 0) {
                            this.taskCount += 1;
                        }
                    });
                });
                //get projects
                axios.get("/api/project").then(res => {
                    this.projectCount = res.data.data.length;
                });
                //get templates count
                axios.get("/api/project-template").then(res => {
                    this.templateCount = res.data.data.length;
                });
                //get matieral count
                axios.get("/api/material").then(res => {
                    this.materialCount = res.data.data.length;
                });
            },
            endTour() {
                this.stopTour;
            },
            fillDataDoughnut() {
                this.datacollectionDoughnut = {
                    labels: this.doughnutData.labels,
                    datasets: [
                        {
                            borderColor: ["rgba(255,255,255,1 )"],
                            backgroundColor: [
                                "rgba(0, 63, 92,.5)",
                                "rgba(47, 75, 124,.5)",
                                "rgba(102, 81, 145,.5)",
                                "rgba(160, 81, 149,.5)",
                                "rgba(212, 80, 135,.5)",
                                "rgba(249, 93, 106,.5)",
                                "rgba(255, 124, 67,.5)",
                                "rgba(255, 166, 0,.5)"
                            ],
                            hoverBackgroundColor: [
                                "rgba(0, 63, 92,1)",
                                "rgba(47, 75, 124,1)",
                                "rgba(102, 81, 145,1)",
                                "rgba(160, 81, 149,1)",
                                "rgba(212, 80, 135,1)",
                                "rgba(249, 93, 106,1)",
                                "rgba(255, 124, 67,1)",
                                "rgba(255, 166, 0,1)"
                            ],
                            data: this.doughnutData.data
                        }
                    ]
                };
            },
            fillDataLine() {
                this.datacollectionLine = {
                    labels: this.lineData.labels,
                    datasets: [
                        {
                            backgroundColor: ["rgba(0, 63, 92,0.5)"],
                            borderColor: ["rgba(0, 63, 92,1)"],
                            steppedLine: false,
                            pointBackgroundColor: "rgba(0, 0, 0,1)",
                            label: "Income",
                            data: this.lineData.data
                        }
                    ]
                };
            },
            getMonthlyTeamRevenue() {
                let self = this;
                axios
                    .get(
                        "api/dashboard/monthly-team-revenue?from=" +
                        this.from +
                        "&to=" +
                        this.to
                    )
                    .then(response => {
                        self.monthlyTeamRevenue = response.data;
                        var label = [];
                        var value = [];
                        for (var i = 0; i < self.monthlyTeamRevenue.length; i++) {
                            label.push(self.monthlyTeamRevenue[i].date);
                            value.push(self.monthlyTeamRevenue[i].total);
                        }
                        self.lineData.labels = label;
                        self.lineData.data = value;
                        self.fillDataLine();
                        self.has_line_chart_data = true;
                    });
            },
            getMonthlyIndividualRevenue() {
                let self = this;
                axios
                    .get(
                        "api/dashboard/monthly-individual-revenue?from=" +
                        this.from +
                        "&to=" +
                        this.to
                    )
                    .then(response => {
                        response.data
                            .sort(function(l, r) {
                                return l.total - r.total;
                            })
                            .reverse();

                        self.monthlyIndividualRevenue = response.data;
                        var label = [];
                        var value = [];

                        var upperbound =
                            self.monthlyIndividualRevenue.length > 8
                                ? 8
                                : self.monthlyIndividualRevenue.length;
                        for (var i = 0; i < upperbound; i++) {
                            //ToDo: handle when ther are more than 8 people
                            label.push(self.monthlyIndividualRevenue[i].user);
                            value.push(self.monthlyIndividualRevenue[i].total);
                        }
                        if (self.monthlyIndividualRevenue.length > 8) {
                            label[7] = "Other";

                            for (var i = 8; i < self.monthlyIndividualRevenue.length; i++) {
                                value[7] = (
                                    parseFloat(value[7]) +
                                    parseFloat(self.monthlyIndividualRevenue[i].total)
                                ).toString();
                            }
                        }

                        self.doughnutData.labels = label;
                        self.doughnutData.data = value;
                        self.fillDataDoughnut();
                        self.has_doughnut_data = true;
                    });
            }
        },
        computed: {
            chartStyles() {
                return {
                    height: "400px",
                    position: "relative"
                };
            }
        }
    };
</script>
<style>
    .chart-container {
        position: relative;
        height: 500px;
        width: 500px;
        margin: 0 auto;
    }
    .chart-container > div {
        position: relative;
        height: 100%;
    }
    .count-card {
        padding: 10px;
        overflow: hidden;
        height: 125px;
        border: none;
    }
    .count-card:hover {
        cursor: pointer;
    }
    .count-card i {
        font-size: 90px;
    }
    .count-card-total h2,
    .count-card-total h3 {
        text-align: right;
        margin: 0;
        padding: 0;
        text-transform: uppercase;
        font-weight: bold;
        color: #f3f3f3;
    }
    .count-card-total {
        top: 15px;
        position: absolute;
        right: 15px;
    }
    #task-card {
        background-color: #ffac01;
    }
    #task-card i {
        color: #e09918;
    }
    #project-card {
        background-color: #3cd660;
    }
    #project-card i {
        color: #2dad4b;
    }
    #template-card {
        background-color: #f3e850;
    }
    #template-card i {
        color: #dace2a;
    }
    #material-card {
        background-color: #fb4154;
    }
    #material-card i {
        color: #dc2f41;
    }
    .form-group label{
        margin-right: 10px;
        margin-left: 10px;
        text-transform: uppercase;
        font-weight: bold;
        color: #b3b3b3;
        font-size: 12px;
    }
    .form-group.form-inline{
        margin: 0 auto;
    }
    #date-range{
        margin-top: 10px;
        text-transform: uppercase;
        font-size: 15px;
        font-weight: bold;
        color: #b3b3b3;
    }
</style>

