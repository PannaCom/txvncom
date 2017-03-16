
$(document).ready(function () {
    $('select').material_select();
    $("#hinh_anh").change(function () {
        //console.log($(this)[0].files[0]);
        //if (!$(this)[0].files[0].name.match(/.(jpg|jpeg|png|gif)$/i)) {
        //    alert('Tệp bạn chọn không phải là ảnh.');
        //    return false;
        //}
        //if (!$(this)[0].files[0].type.match('image.*')) {
        //    alert('Tệp bạn chọn không phải là ảnh.');
        //    return false;
        //}
        readURL(this);
    });

    //$('#website').change(function () {
    //    if ($(this).val() != "") {
    //        if ($(this).val().includes("http://") || $(this).val().includes("https://")) {
    //            $(this).val("http://" + $(this).val());
    //        }           
    //    }        
    //})

    $('#loaixe').on('change', function () {        
        var _lx = $(this).val();
        var banggia = "";
        //$('#banggiarieng').html("");
        if (_lx != null) {
            for (var i = 0; i < _lx.length; i++) {
                banggia += '<div class="input-field col s12 m3">'
                          + '<input id="banggia_' + _lx[i] + '" name="banggia_' + _lx[i] + '" type="number" class="validate" />'
                          + '<label for="banggia_' + _lx[i] + '">Bảng giá xe ' + _lx[i] + ' chỗ (đồng/1km): </label>'
                          + '</div>';
            }
        }        
        $('#banggiarieng').empty().append(banggia);
        
    })

});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        if (!input.files[0].name.match(/.(jpg|jpeg|png|gif)$/i)) {
            alert('Tệp bạn chọn không đúng định dạng ảnh.');
            return false;
        }
        if (!input.files[0].type.match('image.*')) {
            alert('Tệp bạn chọn không phải là ảnh.');
            return false;
        }
        reader.onload = function (e) {
            $('#blah').attr('src', e.target.result).show();
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function contactus(e) {
    e.preventDefault();
    //var formdata = new FormData(); //FormData object
    var $file = document.getElementById('hinh_anh'),
    $formData = new FormData();
    if (document.getElementById("first_name").value === "") {
        var _mes = 'Vui lòng nhập tên nhà xe.';
        Materialize.toast(_mes, 1000)
        document.getElementById("first_name").focus();
        return false;
    }

    if (document.getElementById("phone_number").value === "") {
        var _mes = 'Vui lòng nhập số điện thoại.';
        Materialize.toast(_mes, 1000)
        document.getElementById("phone_number").focus();
        return false;
    }

    if (document.getElementById("add_ress").value === "") {
        var _mes = 'Vui lòng nhập địa chỉ.';
        Materialize.toast(_mes, 1000)
        document.getElementById("add_ress").focus();
        return false;
    }

    if (document.getElementById("email").value === "") {
        var _mes = 'Vui lòng nhập email nhà xe.';
        Materialize.toast(_mes, 1000)
        document.getElementById("email").focus();
        return false;
    }

    //if (document.getElementById("tour").value === "") {
    //    alert('Vui lòng nhập các tour hay đi.');
    //    document.getElementById("tour").focus();
    //    return false;
    //}

    if (document.getElementById("loaixe").value === "") {
        var _mes = 'Vui lòng chọn loại xe.';
        Materialize.toast(_mes, 1000)
        document.getElementById("loaixe").focus();
        return false;
    }
    
    var loaixe = [];   
    $('#loaixe :selected').each(function (i, selected) {
        loaixe[i] = $(selected).val();
    });
    $formData.append("loaixe", loaixe);
    if (loaixe != []) {
        for (var i = 0; i < loaixe.length; i++) {
            $formData.append("banggia_" + loaixe[i], document.getElementById("banggia_" + loaixe[i]).value);
            if (document.getElementById("banggia_" + loaixe[i]).value === "") {
                var _mes = 'Vui lòng nhập bảng giá xe ' + loaixe[i] + ' chỗ.';
                Materialize.toast(_mes, 1000)
                document.getElementById("banggia_" + loaixe[i]).focus();
                return false;
            }
        }
    }

    if (document.getElementById("banggia").value === "") {
        alert('Vui lòng nhập ghi chú bảng giá');
        document.getElementById("banggia").focus();
        return false;
    }

    if ($file.files.length <= 0) {
        alert('Vui lòng gửi hình ảnh nhà xe');
        return false;
    }
    if (!$file.files[0].name.match(/.(jpg|jpeg|png|gif)$/i)) {
        alert('Tệp bạn chọn không đúng định dạng ảnh.');
        return false;
    }
    if (!$file.files[0].type.match('image.*')) {
        alert('Tệp bạn chọn không phải là ảnh.');
        return false;
    }

    if ($file.files.length > 0) {
        for (var i = 0; i < $file.files.length; i++) {
            $formData.append('file-' + i, $file.files[i]);
        }
    }

    //$formData.append("banggia_4", document.getElementById("banggia_4").value);
    //$formData.append("banggia_5", document.getElementById("banggia_5").value);
    //$formData.append("banggia_7", document.getElementById("banggia_7").value);
    //$formData.append("banggia_16", document.getElementById("banggia_16").value);
    //$formData.append("banggia_29", document.getElementById("banggia_29").value);
    //$formData.append("banggia_35", document.getElementById("banggia_35").value);
    //$formData.append("banggia_45", document.getElementById("banggia_45").value);
    $formData.append("first_name", document.getElementById("first_name").value);
    $formData.append("add_ress", document.getElementById("add_ress").value);
    $formData.append("email", document.getElementById("email").value);
    $formData.append("phone_number", document.getElementById("phone_number").value);
    $formData.append("website", document.getElementById("website").value);
    $formData.append("banggia", document.getElementById("banggia").value);
    $formData.append("tour", document.getElementById("tour").value);

    document.getElementById('btn_contact').innerHTML = 'Đang gửi thông tin đăng ký...';
    document.getElementById('btn_contact').disabled = true;
    $.ajax({
        url: '/home/contactus',
        type: 'POST',
        data: $formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            var mes = "Thông tin đăng ký của bạn đã được gửi thành công, chúng tôi sẽ liên lạc sớm lại cho bạn.";
            Materialize.toast(mes, 10000)
            $('#contact_us')[0].reset();
            document.getElementById('btn_contact').innerHTML = 'Gửi thông tin đăng ký';
            document.getElementById('btn_contact').disabled = false;

        },
        error: function (error) {
            Materialize.toast('Vui lòng kiểm tra lại kết nối internet, error: ' + error, 5000);
        }
    });


}