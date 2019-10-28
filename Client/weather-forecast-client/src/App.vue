<template>
  <div id="app">
    <div id="forecastSearchInputContainer" class="container-fluid pb-5">
      <div class="row">
        <b-alert v-model="showErrorAlert" variant="danger" class="col-sm-12 col-md-12 col-lg-12" dismissible>
          Error : {{errorMessage}}
        </b-alert>
      </div>
      <div class="row pt-5">
        <div class="col-sm-4 col-md-4 col-lg-3"></div>
        <div class="col-sm-4 col-md-4 col-lg-6">
          <b-input-group >
            <!-- <vue-country-code
                  @onSelect="onCountryCodeSelect"
                  :preferredCountries="['de']">
            </vue-country-code> -->
            <b-form-input v-model="searchValue" placeholder="Enter city or zip code">
            </b-form-input>
            <b-input-group-append>
              <b-button @click="getForecast" variant="info">
                Get forecast
              </b-button>
            </b-input-group-append>
          </b-input-group>
        </div>
        <div class="col-sm-4 col-md-4 col-lg-3"></div>
      </div>
    </div>

    <div id="forecastResultsContainer" class="container-fluid pb-5">
      <div class="row">
        <div class="col-sm-3 col-md-3 col-lg-3">
          <forecast-history :historyItems="forecastsHistory"/>
        </div>
        <div id="forecastDataContainer" class="container-fluid col-sm-8 col-md-8 col-lg-8">
          <div class="row">
            <div class="col-sm-3 col-md-3 col-lg-2"></div>
            <div class="col-sm-6 col-md-6 col-lg-8 pb-20 m-3">
              <current-weather v-if="currentWeather.length != 0" :currentWeather="currentWeather"/>
            </div>
            <div class="col-sm-3 col-md-3 col-lg-2"></div>
          </div>
          <div class="row">
            <div v-if="forecast.length != 0" class="col-sm-12 col-md-12 col-lg-12">
              <forecast-table v-bind:forecast="forecast"/>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ForecastTable from './components/ForecastTable.vue';
import ForecastHistory from './components/ForecastHistory.vue';
import CurrentWeather from './components/CurrentWeather.vue';
import ForecastService from './api-services/forecast.service.js';

const LOCAL_STORAGE_FORECAST_HISTORY_KEY = 'forecastsHistory';

export default {
  name: 'app',
  components: {
    ForecastTable,
    ForecastHistory,
    CurrentWeather
  },
  data() {
    return {
      forecast: [],
      currentWeather: [],
      forecastsHistory: [],
      searchValue: "",
      countryCode: "",
      showErrorAlert: false,
      errorMessage: ""
    }
  },
  mounted() {
    if (localStorage.forecastsHistory) {
      try {
          this.forecastsHistory = JSON.parse(
            localStorage.getItem(LOCAL_STORAGE_FORECAST_HISTORY_KEY));
          if (this.forecastsHistory === null){
            this.forecastsHistory = [];
          }
        } catch (e) {
          localStorage.setItem(LOCAL_STORAGE_FORECAST_HISTORY_KEY, []);
        }
    }
  },
  methods: {
    onCountryCodeSelect({iso2}) {
       this.countryCode = iso2;
     },
    async getForecast() {
      this.forecast = [];
      this.currentWeather = [];

      //check if input parameter is zip code (contains only digits)
      if (this.searchValue.match(/^[0-9]+$/) != null) {
        await ForecastService.getByZipCode(this.searchValue, this.countryCode)
        .then(async (response) => {
          this.forecast = response.data.forecast;
          this.currentWeather = response.data.currentWeather;

          this.updateForecastHistory();
        })
        .catch((error) => {
          this.forecast = [];
          this.currentWeather = [];
          this.showErrorAlert = true;
          if (error.response == undefined) {
            this.errorMessage = "Some problems occured on server. Please try later."
          }
          else {            
            this.errorMessage = error.response.data;
          }
        });
      }
      //if contains only letters than it is a city name
      else if (this.searchValue.match(/^[a-zA-Z]+$/) != null) {
        await ForecastService.getByCity(this.searchValue, this.countryCode)
        .then(async (response) => {
          this.forecast = response.data.forecast;
          this.currentWeather = response.data.currentWeather;

          this.updateForecastHistory();
          
        })
        .catch((error) => {          
          this.forecast = [];
          this.currentWeather = [];        
          this.showErrorAlert = true;
          if (error.response == undefined) {
            this.errorMessage = "Some problems occured on server. Please try later."
          }
          else {            
            this.errorMessage = error.response.data;
          }
        });
      }
      else {
        this.showErrorAlert = true;
        this.errorMessage = "Invalid input value. Only letters (for city name) or numbers (for zip code) are allowed.";
      }
    },
    updateForecastHistory(){
      //save search in the history entity
      var key = this.forecast.city + this.currentWeather.date;
      if (!this.forecastsHistory.some(i => i.key === key)) {
        var historyItem = {
          key: key,
          city: this.forecast.city,
          date: this.currentWeather.date,
          weather: this.currentWeather.conditions
        };
        this.forecastsHistory.unshift(historyItem);
      }
      
      localStorage.setItem(LOCAL_STORAGE_FORECAST_HISTORY_KEY, 
        JSON.stringify(this.forecastsHistory));
    }
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-image: linear-gradient(rgba(203, 238, 243, 0.5),rgba(92, 224, 180, 0.5));
  min-height: 100vh;
}
</style>
