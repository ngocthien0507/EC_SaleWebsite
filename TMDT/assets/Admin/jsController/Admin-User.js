var common = {
    init: function () {
        common.registerEvent();
    },

    registerEvent: function () {
        $('#username').change(function (e) {
            $.ajax({
                url: '/Admin/Account/CheckUsername',
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

            $('#Email').change(function (e) {
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
                            $('#Email').val("");
                            $('#Email').focus();
                        }
                        else {
                            document.getElementById("EmailMessage").innerHTML = "";
                        }
                    }
                });
            }),

            $('#Phone').change(function (e) {
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
                            $('#Phone').val("");
                            $('#Phone').focus();
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