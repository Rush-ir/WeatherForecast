import axios from 'axios';
import localforage from 'localforage';
import memoryDriver from 'localforage-memoryStorageDriver';
import { setupCache } from 'axios-cache-adapter';
// import nprogress from 'vue-nprogress';

const WEATHER_FORECAST_ENDPOINT = 'https://localhost:5551/api/weather/forecast';

// Register the custom `memoryDriver` to `localforage`
localforage.defineDriver(memoryDriver);

// Create `localforage` instance
const forageStore = localforage.createInstance({
    // List of drivers used
    driver: [
        localforage.INDEXEDDB,
        localforage.LOCALSTORAGE,
        memoryDriver._driver
    ],
    // Prefix all storage keys to prevent conflicts
    name: 'forecast-cache'
});

// Create `axios-cache-adapter` instance
const cache = setupCache({
    maxAge: 10 * 60 * 1000, // 10 min
    store: forageStore
});

// Create `axios` instance passing the newly created `cache.adapter`
const api = axios.create({
    adapter: cache.adapter,
    cache: {
        // Attempt reading stale cache data when response status is either 4xx or 5xx
        readOnError: (error) => {
            return error.response.status >= 400 && error.response.status < 600
        },
        // Deactivate `clearOnStale` option so that we can actually read stale cache data
        clearOnStale: false
    }
});

// before a request is made start the nprogress
// api.interceptors.request.use(config => {
//     nprogress.start();
//     return config;
// }, error => {
//     return Promise.reject(error);
// });

// // // before a response is returned stop nprogress
// api.interceptors.response.use(response => {
//     nprogress.done();
//     return response;
// }, error => {
//     return Promise.reject(error);
// });


export default {

    async getByCity(city, countryCode) {
        if (countryCode.Length > 0) {
            return await api.get(`${WEATHER_FORECAST_ENDPOINT}/city/${city}?countryCode=${countryCode}`);
        }
        return await api.get(`${WEATHER_FORECAST_ENDPOINT}/city/${city}`);
    },

    async getByZipCode(zipcode, countryCode) {
        if (countryCode.Length > 0) {
            return await api.get(`${WEATHER_FORECAST_ENDPOINT}/zipcode/${zipcode}?countryCode=${countryCode}`);
        }
        return await api.get(`${WEATHER_FORECAST_ENDPOINT}/zipcode/${zipcode}`);
    }
};