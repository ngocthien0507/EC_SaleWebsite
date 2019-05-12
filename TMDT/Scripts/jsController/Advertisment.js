var adver = {
    init: function () {
        adver.regEvents();
    },

    regEvents: function () {
        $('#EndDate').change(function () {
            var enddate = $(this).val();
            var startdate = $('#StartDate').val();
            if (enddate <= startdate) {
                alert("Ngày kết thúc phải lớn hơn ngày bắt đầu");
                $(this).val(null);
            }
            else {
                $.ajax({
                    url: '/Advertisment/FindTotalMoney',
                    dataType: 'json',
                    data: {
                        EndDate: $(this).val(),
                        StartDate: $('#StartDate').val(),
                        IdLocation: $('#LocationAD').val()
                    },
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            $('#TotalPrice').val(res.val + " VNĐ");
                        }
                        else {
                            alert(res.val);
                            $(this).val(null);
                        }
                    }
                });
            }
        });

        $('#StartDate').change(function () {
            var date = new moment().add(3, 'days').format('YYYY-MM-DD') ;
            var startdate = $(this).val();
            var enddate = $('#EndDate').val();
            if (startdate >= enddate && enddate!="") {
                alert("Không được chọn ngày lớn hơn ngày kết thúc");
                $(this).val(null);
            }
            else if (startdate < date) {
                alert("Phải chọn cách thời điểm hiện tại ít nhất 3 ngày");
                $(this).val(null);
            }
            else {
                $.ajax({
                    url: '/Advertisment/FindTotalMoney',
                    dataType: 'json',
                    data: {
                        EndDate: $('#EndDate').val(),
                        StartDate: $(this).val(),
                        IdLocation: $('#LocationAD').val()
                    },
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            $('#TotalPrice').val(res.val + " VNĐ");
                        }
                        else {
                            alert(res.val);
                            $(this).val(null);
                        }
                    }
                });
            }
        });
    }
}
adver.init();

function clicked(e) {
    if (!confirm('Bạn đã chắc chắn với thông tin đăng ký chưa?')) e.preventDefault();
}