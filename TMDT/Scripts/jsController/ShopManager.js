var shop = {
    init: function () {
        shop.regEvents();
    },

    regEvents: function () {
        $('.btn-hidden').off('click').on('click', function (e) {
            
            if (confirm('Xác nhận ẩn sản phẩm này?')) {
                $.ajax({
                    data: { idsp: $(this).data('id') },
                    url: '/ShopManager/HidingProduct',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/quan-ly-shop";
                        }
                    }
                });
            } e.preventDefault();
        });

        $('#confirmOrder').off('click').on('click', function (e) {
            
            if (confirm('Xác nhận đồng ý đơn hàng?')) {
                $.ajax({
                    data: { idShopOrder: $(this).data('id') },
                    url: '/ShopManager/ConfirmOrder',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/quan-ly-shop/quan-ly-don-hang";
                        }
                    }
                });
            } e.preventDefault();
        });

        $('#refuseOrder').off('click').on('click', function (e) {

            if (confirm('Đồng ý hủy đơn hàng?')) {
                $.ajax({
                    data: { idShopOrder: $(this).data('id') },
                    url: '/ShopManager/RefuseOrder',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/quan-ly-shop/quan-ly-don-hang";
                        }
                    }
                });
            } e.preventDefault();
        });

        $('#ordercomplete').off('click').on('click', function (e) {

            if (confirm('Xác nhận đã giao hàng thành công?')) {
                $.ajax({
                    data: { idShopOrder: $(this).data('id') },
                    url: '/ShopManager/CompletedOrder',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/quan-ly-shop/quan-ly-don-hang";
                        }
                    }
                });
            } e.preventDefault();
        });
    }
}

shop.init();