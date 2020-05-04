<script>
import { Line, mixins } from "vue-chartjs";
const { reactiveProp } = mixins;

export default {
  extends: Line,
  mixins: [reactiveProp],
  mounted() {
    this.renderChart(this.chartData, this.options);
  },
  data() {
    return {
      options: {
        legend: false,
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true,
                callback: function(value, index, values) {
                  if (parseInt(value) >= 1000) {
                    return (
                      "$" +value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    );
                  } else {
                    return "$" + value;
                  }
                }
              }
            }
          ]
        },
        tooltips: {
          callbacks: {
            label: function(tooltipItem, data) {
              var index = tooltipItem.index;
              var datasetLabel = data.labels[index] || '';
              return datasetLabel + ': $ ' + Intl.NumberFormat().format(data.datasets[0].data[index], 2);
            }
          }
        }
      }
    };
  }
};
</script>
