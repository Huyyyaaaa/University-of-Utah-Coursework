<script>
import { Doughnut, mixins } from "vue-chartjs";
const { reactiveProp } = mixins;

export default {
  extends: Doughnut,
  mixins: [reactiveProp],
  mounted() {
    // this.chartData is created in the mixin.
    // If you want to pass options please create a local options object
    this.renderChart(this.chartData, this.options);
  },
  data() {
    return {
      options: {
        responsive: true,
        maintainAspectRatio: false,
        tooltips: {
          callbacks: {
            label: function(tooltipItem, data) {
              var index = tooltipItem.index;
              var datasetLabel = data.labels[index] || '';
              return datasetLabel + ': $ ' + Intl.NumberFormat().format(data.datasets[0].data[index], 2);
            }
          }
        },
        scales: {}
      }
    };
  }
};
</script>
