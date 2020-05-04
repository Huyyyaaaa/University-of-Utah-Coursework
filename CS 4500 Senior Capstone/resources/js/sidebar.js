
var pathname = window.location.pathname;

if(pathname != '/register' && pathname != '/terms' && pathname != '/password/reset' && pathname != '/landing'){
var sidebarCookie = getCookie("ContrackSidebar");
if(sidebarCookie != ""){
    //if closed close sidebar
    if(sidebarCookie == "closed"){
        $(".sidebar").css("width", "75px");
        $("#spark-app").css("marginLeft", "75px")
        $("#sidebar-logo-link").css("transform", "translateX(-200%)")
        $(".sidebar-text").css("opacity", "0")
        $(".sidebar-text").css("display", "none")
        $(".hamburger-box").css("transform", "translateX(-93px)")
        $(".hamburger-box").toggleClass("closed")
        $(".hamburger-box").children().toggleClass("closed")
    }
    else{
      setSidebarOpen();
    }
}
else{
  setSidebarOpen();
  setCookie("ContrackSidebar", "open", 365);

}
}

function setSidebarOpen(){
  $(".sidebar").css("width", "250px");
  $("#spark-app").css("marginLeft", "250px")
  $("#sidebar-logo-link").css("transform", "translateX(0)")
  $(".sidebar-text").css("opacity", "1")
  $(".sidebar-text").css("display", "")
  $(".hamburger-box").css("transform", "translateX(0)")
}

$(document).ready(function(){


   
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
      })

    $(".hamburger-box").on('click', function(){

        if($(this).hasClass('closed')){

            gsap.to(".sidebar-text", {display: "inline-block", delay: .85})
            gsap.to(".sidebar", {width: "250px", duration: .5})
            gsap.to("#spark-app", {marginLeft: "250px", duration: .5})
            gsap.to("#sidebar-logo-link, .hamburger-box", {x: 0, duration: .5, delay: 1.5})
            gsap.to(".sidebar-text", {opacity: 1, duration: 1, delay: 1})
            setCookie("ContrackSidebar", "open", 365);

        }else{
            
        
        gsap.to(".sidebar", {width: "75px", duration: .5, delay: .5})
        gsap.to("#spark-app", {marginLeft: "75px", duration: .5, delay: .5})
        gsap.to("#sidebar-logo-link", {x: '-200%', duration: .5})
        gsap.to(".hamburger-box", {x: '-93px', duration: .5})
        gsap.to(".sidebar-text", {opacity: 0, duration: .5})
        gsap.to(".sidebar-text", {display: "none", delay: .5})
        setCookie("ContrackSidebar", "closed", 365);
        }
        $(this).toggleClass('closed')
        $(this).children().toggleClass('closed')

    })

});
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }

  function setCookie(cname, cvalue, exdays) {
    var d = new Date();
  d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
  var expires = "expires="+d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires+"; path=/;";
    
  }

