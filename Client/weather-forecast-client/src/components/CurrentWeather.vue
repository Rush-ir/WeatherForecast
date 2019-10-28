<template>
    <div id="currentWeather" class="gradientBackground border border-black rounded">
        <h3> Current weather </h3>
        <h5> {{currentWeather.date}} </h5>
        
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <main-weather-conditions
                        :weatherIconUrl="currentWeather.conditions.weatherIconUrl"
                        :weatherDescription="currentWeather.conditions.weatherDescription"
                        :temperature="currentWeather.conditions.temperature"
                        :tempFormat="currentWeather.conditions.temperatureFormat"/>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6 align-self-center">                
                    <h6>Humidity: 
                        <span class="thick">{{currentWeather.conditions.humidity}}%</span>
                    </h6>
                    <h6>Wind speed:
                         <span class="thick">{{currentWeather.conditions.windSpeed}}m/s</span>
                    </h6>
                    <h6>Pressure:
                         <span class="thick">{{currentWeather.conditions.pressure}}hPa</span> 
                    </h6>               
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import MainWeatherConditions from './MainWeatherConditions.vue';

export default {
    name: "current-weather",
    props: ['currentWeather'],
    components: {
        MainWeatherConditions
    },
}
</script>

<style scoped>
.gradientBackground {
    background-image: linear-gradient(rgba(176, 224, 230, 0.5), rgba(102, 205, 170, 0.7));
}
.thick {
  font-weight: bold;
}
</style>