$(document).ready(function () {
    updatecartproduct();

    $("#search3").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/getoutputs",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))

                },
                error: function (response) {
                    alert(response.responseText)
                },
                failure: function (response) {
                    alert(response.responseText)
                }

            });
        },
        select: function (e, i) {
            $("#hfcustom1").val(i.item.val);
            /*document.getElementById("hfcustom").style.marginTop = 10px;*/
        },
        minLength: 1
    });
});
    $(function () {
    $("#search2").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/getoutput",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))

                },
                error: function (response) {
                    alert(response.responseText)
                },
                failure: function (response) {
                    alert(response.responseText)
                }

            });
        },
        select: function (e, i) {
            $("#hfcustom").val(i.item.val);
            /*document.getElementById("hfcustom").style.marginTop = 10px;*/
        },
        minLength: 1
    });
    });
       /* $('#next').click(function (c) {
            console.log("click");
            location.href = '/Account/Login/';
        });*/
       /* $("#createReport").submit(function () {
            $("img.loaderimg").show();
            *//*event.preventDefault();*//*
        });*/

   

 /*   });*/

/*sign_up_btn.addEventListener("click", () => {
  container.classList.add("sign-up-mode");
});
*/
/*sign_in_btn.addEventListener("click", () => {
  container.classList.remove("sign-up-mode");
})*/;
function updatecartproduct() {
   /* debugger;*/
    console.log();
    var cartproducts;
    var exsitingcookie = $.cookie('ProductId');
    if (exsitingcookie != undefined && exsitingcookie != "" && exsitingcookie != null) {
        cartproducts = exsitingcookie.split(',');
    }
    else {
        cartproducts = []
    }
    $(".item_count").html(cartproducts.length);
};

/*window.onload = function () {
    document.getElementById("loader").style.display = "none";
    document.getElementById("view").style.display = "block";
};*/
if (window.history.replaceState) {
    window.history.replaceState(null, null, window.location.href);
}