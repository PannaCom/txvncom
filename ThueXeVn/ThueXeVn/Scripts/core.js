function unicodeToNoMark(str) {
    if (str == null) return "";
    //return str;
    //input = input.toLowerCase();
    //var noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
    //var unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
    //var a_n = noMark.split(',');
    //var a_u = unicode.split(',');
    //for (var i = 0; i < a_n.length; i++) {
    //    input = input.replace(a_u[i],a_n[i]);
    //}
    str = removeSpecialCharater(str);
    str = str.replace(/\s/g, '-');
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //str = str.replace(/!|@|\$|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|\.|\:|\'| |\"|\&|\#|\[|\]|~/g, "-");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, "");//cắt bỏ ký tự - ở đầu và cuối chuỗi

    return str;

}
function showLoadingImage() {
    $("#loadingImage").show();
}
function hideLoadingImage() {
    $("#loadingImage").hide();
}
function unicodeToNoMarkCat(str) {
    if (str == null) return "";
    //return str;
    //input = input.toLowerCase();
    //var noMark = "a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,e,u,u,u,u,u,u,u,u,u,u,u,u,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,o,i,i,i,i,i,i,y,y,y,y,y,y,d,A,A,E,U,O,O,D";
    //var unicode = "a,á,à,ả,ã,ạ,â,ấ,ầ,ẩ,ẫ,ậ,ă,ắ,ằ,ẳ,ẵ,ặ,e,é,è,ẻ,ẽ,ẹ,ê,ế,ề,ể,ễ,ệ,u,ú,ù,ủ,ũ,ụ,ư,ứ,ừ,ử,ữ,ự,o,ó,ò,ỏ,õ,ọ,ơ,ớ,ờ,ở,ỡ,ợ,ô,ố,ồ,ổ,ỗ,ộ,i,í,ì,ỉ,ĩ,ị,y,ý,ỳ,ỷ,ỹ,ỵ,đ,Â,Ă,Ê,Ư,Ơ,Ô,Đ";
    //var a_n = noMark.split(',');
    //var a_u = unicode.split(',');
    //for (var i = 0; i < a_n.length; i++) {
    //    input = input.replace(a_u[i],a_n[i]);
    //}
    str = removeSpecialCharater(str);
    str = str.replace(/\s/g, '-');
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //str = str.replace(/!|@|\$|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|\.|\:|\'| |\"|\&|\#|\[|\]|~/g, "-");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, "");//cắt bỏ ký tự - ở đầu và cuối chuỗi
    str = str.replace(/-/g, ""); //thay thế 2- thành 1-

    return str;

}
function removeSpecialCharater(input) {
    if (input == null) return "";
    //input = input.replace(/&quot;/g, '"');
    input = input.trim();
    input = input.replace(/\./g, "");
    //input = input.replace(/\,/g, "");
    input = input.replace(/\&/g, "");
    input = input.replace(/\'/g, "");
    input = input.replace(/\"/g, "");
    input = input.replace(/\;/g, "");
    input = input.replace(/\?/g, "");
    input = input.replace(/\!/g, "");
    input = input.replace(/\~/g, "");
    input = input.replace(/\*/g, "");
    input = input.replace(/\:/g, "");
    input = input.replace(/\"/g, "");
    input = input.replace("/", "");
    input = input.replace("%", "");
    input = input.replace("‘", "");
    input = input.replace("’", "");
    input = input.replace(/\"/g, "");
    input = input.replace("+", "");
    input = input.replace("/", "");
    input = input.replace("-", "_");
    input = input.replace("“", "");
    input = input.replace("”", "");
    //input = input.replace(",", "");
    input = input.replace(/\,/g, "");
    //input = input.replace(".", "");

    return input;
    //.replace(",", "").replace("_", "").replace("'", "").replace("\"", "").replace(";", "").replace("”", "").replace(".", "");
}
function getDateId(sDate) {
    if (sDate == null || sDate == "") { return null; }
    sDate = sDate.replace(/\//g, "");
    //alert(sDate);
    sDate = sDate.substring(4, 8) + sDate.substring(2, 4) + sDate.substring(0, 2);
    return sDate;
}
function convertFromDateIdToDateString(sDate) {
    //sDate = sDate.replace(/\//g, "");
    //alert(sDate);
    sDate = sDate.substring(6, 8) + "/" + sDate.substring(4, 6) + "/" + sDate.substring(0, 4);
    return sDate;
}
function getAddressList(value) {
    //var formdata = new FormData(); //FormData object
    //formdata.append("keyword", keyword);
    //formdata.append("location", location);
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/ListAddress/getAddressList');
    xhr.send();
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var news = '{"news":' + xhr.responseText + '}';
            var json_parsed = $.parseJSON(news);
            var preProvin = json_parsed.news[0].provin;
            //alert(news);
            $("#dis").html("<option selected=\"selected\" disabled=\"disabled\">Chọn</option>");
            $("#dis").append("<option value='" + json_parsed.news[0].provin + "' >" + json_parsed.news[0].provin + "</option>");
            for (var i = 0; i < json_parsed.news.length; i++) {
                if (json_parsed.news[i]) {
                    var name = json_parsed.news[i].dis;
                    //alert(name);
                    if (preProvin != json_parsed.news[i].provin) {
                        $("#dis").append("<option value='" + json_parsed.news[i].provin + "'>" + json_parsed.news[i].provin + "</option>");
                        preProvin = json_parsed.news[i].provin;
                    }
                    //$("#dis").append("<option value='" + name + "'>" + name + "</option>");
                }
            }
            $("#dis").val(value);
            //alert(news);
        }
    }
}
function searchHotel() {
    var keyword = document.getElementById('hotelname').value;
    //alert(keyword);
    $('#hotelname').autocomplete({
        source: '/Hotels/getListHotel?keyword=' + keyword,
        select: function (event, ui) {
            //alert(ui.item.id);
            $(event.target).val(ui.item.value);
            //search();
            //$('#search_form').submit();
            return false;
        },
        minLength: 2
    });
}
function searchHotelAuto() {
    var keyword = document.getElementById('hotelnameauto').value;
    //alert(keyword);
    $('#hotelnameauto').autocomplete({
        source: '/Hotels/getListHotel?keyword=' + keyword,
        select: function (event, ui) {
            //alert(ui.item.id);
            $(event.target).val(ui.item.value);
            autosearchhotel(ui.item.value);
            return false;
        },
        minLength: 2
    });
}
function autosearchhotel(val) {
    $("#labelautosearch").html("Đang tìm kiếm...<img src=\"/Images/loading.gif\" width=20 height=20>");
    var formdata = new FormData(); //FormData object
    formdata.append("keyword", val);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Hotels/getIdHotelByName');
    xhr.send(formdata);
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var id = xhr.responseText;
            if (id != "-1") {
                $("#labelautosearch").html("Gõ tên khách sạn cần tìm..");
                window.open("/Hotels/Edit/" + id, "_blank");
            }
        }
    }
}
function getProjectName(projectid, iditem) {
    //var formdata = new FormData(); //FormData object
    //formdata.append("keyword", keyword);
    //formdata.append("location", location);
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/projectitem/getProjectName?id=' + projectid);
    xhr.send();
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#td-" + projectid + "-" + iditem).html(xhr.responseText.toUpperCase());
        }
    }
}
function getProjectItemName(projectid, itemid,id) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/projectitem/getProjectItemName?id=' + itemid);
    xhr.send();
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#item-" + projectid + "-" + id).html(xhr.responseText.toUpperCase());
        }
    }
}
function optimizeImageMobile() {
    //$("img").css("height", "auto");
    if (!detectmob()) return;
    $("img").css("height", "auto");
    $("img").css("maxWidth", "95%");
    $("img").css("align", "center");
    //$("table").css("border-color","white");
    //$("table").css("border-spacing","0px");
    //$("table").css("maxWidth","95%");
    $("p").css("font-size", 16);
    //$("img").css("align","center");
    $("table").css("maxWidth", "95%");
}
function detectmob() {
   
    if (navigator.userAgent.match(/Android/i)
     || navigator.userAgent.match(/webOS/i)
     || navigator.userAgent.match(/iPhone/i)
     || navigator.userAgent.match(/iPad/i)
     || navigator.userAgent.match(/iPod/i)
     || navigator.userAgent.match(/BlackBerry/i)
     || navigator.userAgent.match(/Windows Phone/i)
     ) {
        return true;
    }
    else {
        return false;
    }   
}
function viewMenuItem(projectid, itemid) {
    //alert("ok");
    for (var i = 0; i <= 100; i++) {
        if (document.getElementById("dvmenuview_" + projectid + "_" + i)) {
            //alert("ii");
            if (document.getElementById("dvmenuview_" + projectid + "_" + i).style.display == "none") {
                document.getElementById("dvmenuview_" + projectid + "_" + i).style.display = "block";
            } else {
                document.getElementById("dvmenuview_" + projectid + "_" + i).style.display = "none";
            }
        }
    }
}