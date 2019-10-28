<script>
import { Line, mixins } from 'vue-chartjs';
import ChartDataLabels from 'chartjs-plugin-datalabels'; 

export default {
  name: "ForecastDetailsChart",
  mixins: [Line],
  props: [
      'chartData'
    ],
  mounted () {

    var options = {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                gridLines: {
                    display: false,
                }
            }],
            yAxes: [{
                ticks: {
                        max: Math.max(...this.chartData.datasets[0].data) + 3,
                        min: Math.min(...this.chartData.datasets[0].data) - 2,
                        display: false
                    },
                gridLines: {
                    display: false,
                    drawBorder: false,
                }   
            }]
        },
        legend: {
            display: false
        },
        plugins: {
            datalabels: {
                color: 'blue',
                align: 'top',
                font: {
                    size: 16
                }
            }
        }
    };
    
    this.renderChart(this.chartData, options);
  }
}
</script>
