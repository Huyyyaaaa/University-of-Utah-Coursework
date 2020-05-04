<template>
    <div style="width: 100%; height: 100%;">
        <div id="left">
            <div class="container">
                <div class="text-center img-container">
                    <img src="/img/logo/Regular/logo-lg.png" width="300px" alt="Contrack Logo">
                </div>
                <div class="row">
                <form class="col" role="form" v-on:submit.prevent="attemptLogin">
                    <input type="hidden" name="_token" :value="csrf">
                    <div class="form-group">
                        <input type="email" class="form-control" name="email"  placeholder="Email" v-model="form.email" autofocus>
                    </div>
                    <div class="form-group">
                        <input type="password" class="form-control" name="password" placeholder="Password" v-model="form.password">
                    </div>
                    <br>
                    <div class="form-group">
                        <input type="submit" class="btn btn-primary btn-lg btn-block" value="Login">
                    </div>

                </form>
                </div>
                <br>
                <div class="row">
                    <a class="col btn btn-light btn-sm m-2" href="password/reset">Forgot Your Password?</a>
                    <a class="col btn btn-light btn-sm m-2" href="/register">Need an Account?</a>
                </div>
            </div>
        </div>
        <div id="right" class="hidden-sm hidden-xs">
            <img src="images/landing2.svg" alt="construction worker">
        </div>
        <vue-toastr ref="mytoast"></vue-toastr>
    </div>

</template>

<script>
    import VueToastr from "vue-toastr";

    export default {
        name: "login-view",
        components:{VueToastr},
        data(){
             return{
            csrf: document.querySelector('meta[name="csrf-token"]').getAttribute('content'),
            form:{
                email: "",
                password: ""
            }
        }
        },
        mounted(){
            this.$refs.mytoast.defaultPosition = "toast-top-left";
        },
        methods:{
            attemptLogin(){
                axios.post('/login', this.form)
                .then(res => {
                    if(res.status == 200){
                        window.location = '/'
                    }
                }).catch(err =>{
                    this.$refs.mytoast.e("Invalid Email and/or Password")
                })
            }
        }
    }
</script>

<style scoped>
    #left{
        width: 50%;
        height: 100%;
        position: fixed;
        left: 0;
    }
    #right{
        width: 50%;
        height: 100%;
        position: fixed;
        right: 0;
    }
    #right img{
        height: 105%;
        position: absolute;
        right: 0;
        top: -5px;
    }
    .container{
        width: 75%;
        position: absolute;
        transform: translate(-50%, -50%);
        top: 60%;
        left: 50%;
    }
    .img-container{
        margin-bottom: 50px;
    }
    *{
        font-family: 'Open-Sans', sans-serif;
    }
</style>
