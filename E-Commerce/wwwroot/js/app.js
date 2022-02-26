$(document).ready(function () {
    
        $('#singup').click(function (e) {
            console.log("click");
            location.href = '/Account/Register/';
        });
        $('#sign-in').click(function (c) {
            console.log("click");
            location.href = '/Account/Login/';
        });
       /* $('#next').click(function (c) {
            console.log("click");
            location.href = '/Account/Login/';
        });*/
       /* $("#createReport").submit(function () {
            $("img.loaderimg").show();
            *//*event.preventDefault();*//*
        });*/

       
    });

/*sign_up_btn.addEventListener("click", () => {
  container.classList.add("sign-up-mode");
});
*/
/*sign_in_btn.addEventListener("click", () => {
  container.classList.remove("sign-up-mode");
})*/;
