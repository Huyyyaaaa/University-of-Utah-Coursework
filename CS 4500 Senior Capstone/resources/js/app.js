
/*
 |--------------------------------------------------------------------------
 | Laravel Spark Bootstrap
 |--------------------------------------------------------------------------
 |
 | First, we will load all of the "core" dependencies for Spark which are
 | libraries such as Vue and jQuery. This also loads the Spark helpers
 | for things such as HTTP calls, forms, and form validation errors.
 |
 | Next, we'll create the root Vue application for Spark. This will start
 | the entire application and attach it to the DOM. Of course, you may
 | customize this script as you desire and load your own components.
 |
 */

require('spark-bootstrap');

require('./components/bootstrap');

const files = require.context('./', true, /\.vue$/i);
files.keys().map(key => Vue.component(key.split('/').pop().split('.')[0], files(key).default));

require('./sidebar');
require('./table');
// import plugin
import VueToastr from "vue-toastr";
import money from 'v-money'
import moment from "moment";
import { Datetime } from 'vue-datetime'
import 'vue-datetime/dist/vue-datetime.css'
import VueTour from 'vue-tour'
import VueCookies from 'vue-cookies'
import * as VueGoogleMaps from "vue2-google-maps";
// register component
Vue.component("vue-toastr", VueToastr);
Vue.use(money, {precision: 2, prefix: '$'})
Vue.use(Datetime)
Vue.component('datetime', Datetime);

require('vue-tour/dist/vue-tour.css')

Vue.use(VueTour)

Vue.use(VueGoogleMaps, {
    load: {
        key: "AIzaSyCkrQLUu184Vye0ohxkt9hSlW0ZNDk8GQA",
    }
});

Vue.use(VueCookies)

//Global filters

/* filter for formatting a number to a price
    args: none
    usage: {{x.price | formatPrice }}
 */
Vue.filter('formatPrice', function(price){
    return new Intl.NumberFormat('en-US', {style: 'currency', currency: 'USD'}).format(price);
});
/* fliter for formatting text length
    args: length
    usage: {{text | strLimit(100)}}
*/
Vue.filter('strLimit', function(value, size){
    if(value.length <= size){
        return value;
    }

    return value.substr(0, size) + '...';
})
/* fliter for formatting date
    args: none
    usage: {{date | formatDate }}
*/
Vue.filter('formatDate', function(value){
    return moment(String(value)).format('MM/DD/YY hh:mm a')
})

Vue.mixin({
    methods:{
        checkTour: function(){
            if(this.$cookies.isKey("ContrackTour")){
                if(this.$cookies.get('ContrackTour') == 'yes'){
                    return true
                }else{
                    return false
                }
            }else{
                this.$cookies.set('ContrackTour','no', '1y')
                return false
            }
        },
        stopTour: function(){
            this.$cookies.set('ContrackTour', 'yes', '1y')
        }
    }
})


var app = new Vue({
    mixins: [require('spark')]
});
