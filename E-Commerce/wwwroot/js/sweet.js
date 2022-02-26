
$("#a_id").click(function () {
    console.log("Click");

 

    swal({
        title: 'Logout',
    text: 'Do you want to logout this Account?',
    icon: 'warning',
    buttons: true,
    dangerMode: true
            })
                .then((willOUT) => {
                    if (willOUT) {
        window.location.href = '/Account/logout/', {
            icon: 'success',
        }
    }
                });

        });

