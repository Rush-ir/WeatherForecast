<template>
    <div id="mainWeatherConditions" class="container-fluid">
        <div id="weatherDetails" class="row">
            <div class="col-sm-8 col-md-8 col-lg-7">
                <div class="row"> 
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <img alt="weather logo" :src="weatherIconUrl">
                    </div>                    
                </div>
                <div  class="row"> 
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <p>{{ weatherDescription }}</p>
                    </div>                    
                </div>
            </div>
            <div class="col-sm-4 col-md-4 col-lg-5 align-self-center"> 
                <h4>{{ temperature }}&deg;{{ tempFormat }}</h4>
            </div>
        </div>        
    </div>
</template>

<script>
export default {
    name: "main-weather-conditions",
    props: [
        'weatherIconUrl',
        'weatherDescription', 
        'temperature',
        'tempFormat'
    ]
}
</script>