<template>
    <div id="forecastDetailsCell">        
            <b-tabs content-class="mt-3">
                <b-tab title="Temperature" @click="activateTab('Temperature')" active>
                    <h6>Average temperature: 
                        <p class="thick">{{ details.avgWeatherConditions.temperature }}&deg;{{ details.avgWeatherConditions.temperatureFormat }}</p>
                    </h6>
                    <div v-if="isVisibleTab == 'Temperature'" >
                        <forecast-details-chart 
                            :chartData="createDetailedChart('Temperature')"/>
                    </div>
                </b-tab>
                <b-tab title="Pressure" @click="activateTab('Pressure')">
                    <h6>Average pressure: 
                        <p class="thick">{{details.avgWeatherConditions.pressure}}hPa</p>
                    </h6>
                    <div v-if="isVisibleTab == 'Pressure'" >
                        <forecast-details-chart 
                            :chartData="createDetailedChart('Pressure')"/>
                    </div>
                </b-tab>
                <b-tab title="Humidity" @click="activateTab('Humidity')">
                    <h6>Average humidity: 
                        <p class="thick">{{details.avgWeatherConditions.humidity}}%</p>
                    </h6>
                    <div v-if="isVisibleTab == 'Humidity'" >
                        <forecast-details-chart 
                            :chartData="createDetailedChart('Humidity')"/>
                    </div>
                </b-tab>
                <b-tab title="Wind speed" @click="activateTab('WindSpeed')">
                    <h6>Average wind speed: 
                        <p class="thick">{{details.avgWeatherConditions.windSpeed}}m/s</p>
                    </h6>
                    <div v-if="isVisibleTab == 'WindSpeed'" >
                        <forecast-details-chart 
                            :chartData="createDetailedChart('WindSpeed')"/>
                    </div>
                </b-tab>
            </b-tabs>   
    </div>
</template>

<script>
import ForecastDetailsChart from './ForecastDetailsChart.vue';

export default {
    name: "ForecastDetailsCell",
    props: [
        "details"
    ],
    components: {
        ForecastDetailsChart
    },
    data() {
        return {
            isVisibleTab: 'Temperature'
        }
    },
    methods: {
        activateTab(tabName) {
            this.isVisibleTab = tabName;
        },
        createDetailedChart(chartType) {
            var labels = this.details.detailedDailyForecasts.map(i => i.time);

            var data;
            switch (chartType) {            
                case 'Humidity' : 
                    data = this.details.detailedDailyForecasts.map(i => i.conditions.humidity);
                    break;

                case 'WindSpeed' : 
                    data = this.details.detailedDailyForecasts.map(i => i.conditions.windSpeed);
                    break;

                case 'Pressure' : 
                    data = this.details.detailedDailyForecasts.map(i => i.conditions.pressure);
                    break;

                case 'Temperature' : 
                default :
                    data = this.details.detailedDailyForecasts.map(i => i.conditions.temperature);
                    break;
            }

            return {
                labels: labels,
                datasets: [
                    {
                        backgroundColor: 'rgba(176, 224, 230, 0.5)',
                        borderColor: 'rgba(102, 205, 170, 0.7)',
                        data: data
                    }
                ]
            };
        },
    }
}
</script>

<style scoped>
.thick {
  font-weight: bold;
}
</style>