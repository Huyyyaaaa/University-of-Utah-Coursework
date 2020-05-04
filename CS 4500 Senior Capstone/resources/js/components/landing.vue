<template>
    <div id="main">
        <!-- navbar -->
        <nav class="navbar navbar-expand-lg fixed-top">
            <a class="navbar-brand" href="#">
                <img src="/img/logo/Regular/logo.png" alt="logo" />
            </a>
            <button
                class="navbar-toggler"
                type="button"
                data-toggle="collapse"
                data-target="#navbarSupportedContent"
                aria-controls="navbarSupportedContent"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <div style="width: 100%; float: right;">
                    <a
                        href="/register"
                        class="btn btn-outline-primary my-2 mx-2 float-right"
                    >
                        Sign Up
                    </a>
                    <a
                        href="/"
                        class="btn btn-outline-default my-2 mx-2 float-right"
                    >
                        Login
                    </a>
                </div>
            </div>
        </nav>
        <div class="container-fluid" style="margin-top: 90px;">
            <!-- app catch phrase -->
            <div class="row">
                <div class="col">
                    <div class="center">
                        <h6>BIDS MADE SIMPLE.</h6>
                        <h1>Simplify project workflow, get bids out quickly</h1>
                        <p>
                            Contrack automates the bid creation process for you
                            and your team and makes tracking projects simple and
                            intuitive
                        </p>
                        <a href="#app" class="landing-link btn btn-lg btn-primary mr-3 mb-2"
                            >Learn More</a
                        >
                        <a href="#about" class="landing-link btn btn-lg btn-default mb-2"
                            >About Us</a
                        >
                        <br />
                        <p id="track-link">
                            Track your projects?
                            <a href="/register">Get Started</a>
                        </p>
                    </div>
                </div>
                <div id="crane-col" class="col">
                    <img class="img-responsive" id="house" src="/images/crane.jpg" alt="" />
                </div>
            </div>
            <br /><br /><br>
            <hr>
            <!-- app description -->
            <div class="row" id="app">
                <div class="col-12 text-center">
                    <h1>Ditch the paper and the spreadsheets</h1>
                    <h6>Built for contractors and construction companies</h6>
                </div>
            </div>
            <br />
            <div id="img-row" class="row">
                <div class="col-12 text-center">
                    <img class="img-responsive" id="time-img" src="/images/time.jpg" alt="" />
                </div>
            </div>
            <br />
            <!-- app features -->
            <div class="row" id="app-features">
                <div class="col-md-4 col-sm-12">
                    <div class="">
                        <i class="fa fa-tasks" aria-hidden="true"></i>
                        <h5>Tasks, simplified</h5>
                        <p>
                            Task managment for projects and in general made easy
                        </p>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div>
                        <i class="fas fa-toolbox"></i>
                        <h5>Projects, organized</h5>
                        <p>
                            project information, materials, labor, and more
                            organized into a single view
                        </p>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div>
                        <i class="fas fa-file-invoice-dollar"></i>
                        <h5>Bids, automated</h5>
                        <p>
                            get an automated bid for project in seconds. Simple
                            and easy to customize
                        </p>
                    </div>
                </div>
            </div>
            <br />
            <hr />
            <br />
            <!-- about team -->
            <div class="row" id="about">
                <div class="col text-center">
                    <h1>About Us</h1>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6 col-sm-12 text-center">
                    <h6 class="text-center">the team</h6>
                    <p>We are <strong>Raise the Roof</strong></p>
                    <p>
                        We created Contrack for our families and friends who are
                        general contractors and work for construction companies.
                        We want small sized construction companies to have the
                        same tools as big construction firms without paying the
                        big prices. With Contrack, these companies will be able
                        to do more work and stay organized in less time,
                        allowing them to focus on what really matters -- their
                        clients
                    </p>
                </div>
                <div class="col-md-6 col-sm-12" id="guys">
                    <h6 class="text-center">the guys</h6>
                    <br>
                    <div class="row">
                        <div class="col">
                            <div class="member">
                                <p><strong>Aaron Templeton</strong></p>
                                <p><small>Frontend</small></p>
                                <small
                                    ><a href="mailto:aaron.templeton@live.com"
                                        >aaron.templeton@live.com</a
                                    ></small
                                >
                            </div>
                        </div>
                        <div class="col">
                            <div class="member">
                                <p><strong>Jake Pennington</strong></p>
                                <p><small>Backend</small></p>
                                <small
                                    ><a href="mailto:jakepennington7@gmail.com"
                                        >jakepennington7@gmail.com</a
                                    ></small
                                >
                            </div>
                        </div>
                    </div>
                    <br>
                    <div class="row">
                        <div class="col">
                            <div class="member">
                                <p><strong>Barrett Nield</strong></p>
                                <p><small>Frontend</small></p>
                                <small
                                    ><a href="mailto:nieldbarrett@gmail.com"
                                        >nieldbarrett@gmail.com</a
                                    ></small
                                >
                            </div>
                        </div>
                        <div class="col">
                            <div class="member">
                                <p><strong>Wyatt Davis</strong></p>
                                <p><small>Mobile</small></p>
                                <small
                                    ><a href="mailto:plax77@gmail.com"
                                        >plax77@gmail.com</a
                                    ></small
                                >
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- signup callout -->
            <br><br>
            <div class="row" id="signup">
                <div class="col text-center">
                <h1>Construction done better</h1>
                <br>
                <div>
                    <a href="/register" class="btn btn-lg btn-primary">Try Contrack</a>
                    <a href="/" class="btn btn-lg btn-outline-primary">Login</a>
                </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {};

$(document).ready(function(){
  $(".landing-link").on('click', function(event) {

    if (this.hash !== "") {
      event.preventDefault();

      var hash = this.hash;
      $('html, body').animate({
        scrollTop: parseInt($(hash).offset().top - 100, 10)
      }, 800, function(){
        window.location.hash = hash;
      });
    }
  });
});
</script>
<style scoped>
p {
    font-family: "Nunito", sans-serif;
}
h1,
h2,
h3,
h4,
h5,
h6 {
    font-family: "Montserrat", sans-serif;
    font-weight: bold;
    letter-spacing: 1px;
}
h6 {
    text-transform: uppercase;
}

.center {
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    position: relative;
    width: 75%;
}
h6 {
    font-weight: bold;
    font-size: 15px;
    color: #b3b3b3;
}
#track-link {
    font-size: 15px;
}
#track-link a {
    text-decoration: underline;
}
p {
    font-size: 1.2em;
}
h1 {
    font-size: 2.5em;
}
.navbar {
    margin-bottom: 90px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);;
    z-index: 100;
    background-color: white;
}
i {
    font-size: 40px;
    margin-bottom: 2px;
    padding-bottom: 12px;
}
#app-features h5 {
    font-size: 20px;
    font-family: "Nunito", sans-serif;
}
#app-features p {
    font-size: 20px;
}
#app-features .col-4 div {
    width: 75%;
    margin: 0 auto;
}
.member p {
    padding: 0;
    margin: 0;

}
.member{
    text-align: center;
}
#signup{
    padding-top: 75px;
    padding-bottom: 75px;
    /* background-image: url('/images/bg2.png'); */
    background:linear-gradient(0deg, rgba(182, 182, 182, 0.5), rgba(255, 255, 48, 0.5)), url('/images/bg2.png');
}
#signup a{
    font-size: 30px;
    margin: 10px;
}
#house{
max-width: 100%;
height: auto;
right: 0;
padding:-15px;
margin-right: -15px;
}
.row{
    padding: 15px 50px 15px 50px;
    /* max-height: 1000px; */
}
#time-img{
    max-width: 100%;
    height: auto;
    max-height: 400px;
}

@media only screen and (max-width: 850px) {
    #crane-col{
        display: none;
    }
    #time-img{
        display: none;
    }
    .center{

    position: relative;
        width: 100%;
    }
    #main {
    margin-left: 0px;
}
}
</style>
