import Vue from 'vue';
import BootstrapVue from 'bootstrap-vue';
import VueCountryCode from 'vue-country-code';
import App from './App.vue';


//imported styles
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import 'vue-country-code/dist/vue-country-code.css';

Vue.use(VueCountryCode);
Vue.use(BootstrapVue);

Vue.config.productionTip = false

new Vue({
    render: h => h(App),
}).$mount('#app');