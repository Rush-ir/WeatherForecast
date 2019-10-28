<template>
    <div id="historyContainer">
        <h2 class="border border-blue rounded gradientBackground">HISTORY</h2>
        <div class="scrollable">
            <div v-for="item in historyItems" :key="item.id">
                <div class="border border-blue rounded mt-3 gradientBackground">
                    <h4>{{ item.city }}</h4>
                    <h5>{{ item.date }}</h5>
                    <main-weather-conditions
                        :temperature="item.weather.temperature"
                        :tempFormat="item.weather.temperatureFormat"
                        :weatherDescription="item.weather.weatherDescription"
                        :weatherIconUrl="item.weather.weatherIconUrl">
                    </main-weather-conditions>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import MainWeatherConditions from './MainWeatherConditions.vue';

export default {
    name: "forecast-history",    
    components: {
        MainWeatherConditions,
    },
    props: [
        'historyItems'
    ]
}
</script>

<style scoped>
.gradientBackground {
    background-image: linear-gradient(rgba(176, 224, 230, 0.5), rgba(102, 205, 170, 0.7));
}
.scrollable {
  max-height: 700px;
  overflow-y: auto;
}
</style>