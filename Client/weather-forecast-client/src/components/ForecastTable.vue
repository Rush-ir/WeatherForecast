<template>
  <div class="mainTheme">
    <b-table-lite bordered striped responsive class="text-nowrap scrollable"
      :fields="fields" 
      :items="forecast.dailyForecasts">
      
        <template v-slot:cell(ShortForecast)="data">
            <h5> {{ data.item.date }} </h5>
            <main-weather-conditions
                :temperature="data.item.avgWeatherConditions.temperature"
                :tempFormat="data.item.avgWeatherConditions.temperatureFormat"
                :weatherDescription="data.item.avgWeatherConditions.weatherDescription"
                :weatherIconUrl="data.item.avgWeatherConditions.weatherIconUrl"/>
        </template>

        <template v-slot:cell(DetailedForecast)="data">
            <forecast-details-cell v-bind:details="data.item"/>
        </template>
    </b-table-lite>
        
  </div>
</template>

<script>

import MainWeatherConditions from './MainWeatherConditions.vue';
import ForecastDetailsCell from './ForecastDetailsCell.vue';

  export default {      
    name: "forecast-table",
    components: {
        MainWeatherConditions,
        ForecastDetailsCell
    },
    props: [
      "forecast"
    ],
    data() {
      return {
        fields : [
          { key : 'ShortForecast', label : 'Short forecast' },
          { key : 'DetailedForecast', label : 'Detailed forecast' }
        ]
      }
    }
  }
</script>

<style scoped>
.mainTheme {
  background-color: rgba(229, 215, 235, 0.5);
}
.scrollable {
  max-height: 93vh;
  overflow-y: auto;
}
</style>
