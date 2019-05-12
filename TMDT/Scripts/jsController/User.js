var common = {
    init: function () {
        common.registerEvent();
    },

    registerEvent: function () {
        $('#username').change(function (e) {
            $.ajax({
                url: '/User/CheckUsername',
                dataType: 'json',
                data: {
                    username: $(this).val()
                },
                type: 'POST',
                success: function (res) {
                    if (res.status == false) {
                        document.getElementById("UsernameMessage").innerHTML = "Tài khoản đã tồn tại";
                        $('#username').val("");
                        $('#username').focus();
                    }
                    else {
                        document.getElementById("UsernameMessage").innerHTML = "";
                    }
                }
            });
        }),

            $('#email').change(function (e) {
                $.ajax({
                    url: '/User/CheckEmail',
                    dataType: 'json',
                    data: {
                        email: $(this).val()
                    },
                    type: 'POST',
                    success: function (res) {
                        if (res.status == false) {
                            document.getElementById("EmailMessage").innerHTML = "Email đã được sử dụng";
                            $('#email').val("");
                            $('#email').focus();
                        }
                        else {
                            document.getElementById("EmailMessage").innerHTML = "";
                        }
                    }
                });
            }),
            
            $('#phone').change(function (e) {
                $.ajax({
                    url: '/User/CheckPhone',
                    dataType: 'json',
                    data: {
                        phone: $(this).val()
                    },
                    type: 'POST',
                    success: function (res) {
                        if (res.status == false) {
                            document.getElementById("PhoneMessage").innerHTML = "Số diện thoại đã được sử dụng";
                            $('#phone').val("");
                            $('#phone').focus();
                        }
                        else {
                            document.getElementById("PhoneMessage").innerHTML = "";
                        }
                    }
                });
            })
    }
}

common.init();