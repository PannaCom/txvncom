$(document).ready(function () {
    // tim kiem tai xe
    $('#getmaplocation').tooltip({
        placement: 'left',
        title: 'Lấy vị trí hiện tại'
    })
    $('#movemaplocation').tooltip({
        placement: 'left',
        title: 'Hoán vị địa điểm'
    });

    var options = {
        map: "#map-canvas"
    };
    $("#place_from").geocomplete(options)
  .bind("geocode:result", function (event, result) {
      $("#lat1").val(result.geometry.location.lat());
      $("#lng1").val(result.geometry.location.lng());
      //alert("long" + result.geometry.location.lng() + ", lat=" + result.geometry.location.lat());
  })
  .bind("geocode:error", function (event, status) {
      $.log("ERROR: " + status);
  })
  .bind("geocode:multiple", function (event, results) {
      $.log("Multiple: " + results.length + " results found");
  });

    $("#place_to").geocomplete(options)
      .bind("geocode:result", function (event, result) {
          $("#lat2").val(result.geometry.location.lat());
          $("#lng2").val(result.geometry.location.lng());
          //alert("long" + result.geometry.location.lng() + ", lat=" + result.geometry.location.lat());
      })
      .bind("geocode:error", function (event, status) {
          $.log("ERROR: " + status);
      })
      .bind("geocode:multiple", function (event, results) {
          $.log("Multiple: " + results.length + " results found");
      });
    
    
    var map_x1 = new google.maps.Map(document.getElementById('map-canvas'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 6
    });
    var infoWindow = new google.maps.InfoWindow({ map: map_x1 });

    $('#getmaplocation').on('click', function () {
        // Try HTML5 geolocation.        

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                //console.log(pos.lat + ',' + pos.lng);
                $('#lat1').val(pos.lat);
                $('#lng1').val(pos.lng);
                $.ajax({
                    url: 'http://maps.googleapis.com/maps/api/geocode/json?latlng=' + pos.lat + ',' + pos.lng + '&sensor=false',
                    cache: false
                }).done(function (html) {
                    $('#place_from').val(html.results[0].formatted_address);
                });

            }, function () {
                console.log("Browser doesn't support Geolocation");
            }, function (failure) {
                if (failure.message.indexOf("Only secure origins are allowed") == 0) {
                    // Secure Origin issue.

                }
            });
        } else {
            // Browser doesn't support Geolocation
            console.log("Browser doesn't support Geolocation");
        }
    })
    // end 

    /*===========HOME===========*/

    if ($('#datetimepicker').length) {
        $('#datetimepicker').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: '@DateTime.Now.Year/@DateTime.Now.Month/@DateTime.Now.Date'
        });
        var d = new Date();
        var s = d.toLocaleString();
        $('#datetimepicker').datetimepicker({ value: s, step: 10 });
    }

    $.ajax({
        url: "/Api/getListCarType",
        cache: false
    }).done(function (html) {
        var news = '{"news":' + html + '}';
        var json_parsed = $.parseJSON(news);
        for (var i = 0; i < json_parsed.news.length; i++) {
            if (json_parsed.news[i]) {
                var name = json_parsed.news[i].name;
                $("#car_type").append("<option value='" + name + "'>" + name + "</option>");
            }
        }
    });
    $.ajax({
        url: "/Api/getCarSize",
        cache: false
    }).done(function (html) {
        var news = '{"news":' + html + '}';
        var json_parsed = $.parseJSON(news);
        for (var i = 0; i < json_parsed.news.length; i++) {
            if (json_parsed.news[i]) {
                var name = json_parsed.news[i].name;
                $("#car_size").append("<option value='" + name + "'>" + name + "</option>");
            }
        }
    });

    //counter
    if ($('.counter').length) {
        $('.counter').counterUp({
            delay: 10,
            time: 1000
        });
    }

    if ($('#booking').length) {
        $('#booking').dataTable({
            "bFilter": false,
            scrollY: '50vh',
            scrollCollapse: true,
            paging: false,
            "bLengthChange": false,
            "bInfo": false,
            "bAutoWidth": true,
            "bSort": true,
        });
        $("#booking").on("click", ".bdx-x", function () {
            $(this).remove();
        })

    }

    if ($('.show_pn').length) {
        $('.show_pn').on('click', function (e) {
            window.location.href = "/Home/Taixe";
            e.preventDefault();
        })
    }

    $.ajax({
        url: "/Home/getCarSize",
        cache: false
    }).done(function (html) {
        $('#Loaixe_socho').append(html);
    });
    if ($('.carousel').length) {
        $('.carousel').carousel({
            interval: 1000 * 2
        });
    }

    if ($('#from_datetime').length) {
        $('#from_datetime').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: '@DateTime.Now.Year/@DateTime.Now.Month/@DateTime.Now.Date'
        });        
        var d = new Date();
        var s = d.toLocaleString();
        $('#from_datetime').datetimepicker({ value: s, step: 10 });
    }

    if ($('#to_datetime').length) {
        $('#to_datetime').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'en',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            startDate: '@DateTime.Now.Year/@DateTime.Now.Month/@DateTime.Now.Date'
        });
    }

    if ($("#owl-demo").length) {
        var owl = $("#owl-demo");

        owl.owlCarousel({
            autoPlay: 3000,
            pagination: true,
            items: 1, //10 items above 1000px browser width
            itemsDesktop: [1000, 1], //5 items between 1000px and 901px
            itemsDesktopSmall: [900, 1], // 3 items betweem 900px and 601px
            itemsTablet: [600, 1], //2 items between 600 and 0;
            itemsMobile: [480, 1] // itemsMobile disabled - inherit from itemsTablet option

        });

        // Custom Navigation Events
        $(".next").click(function () {
            owl.trigger('owl.next');
        })
        $(".prev").click(function () {
            owl.trigger('owl.prev');
        })
        $(".play").click(function () {
            owl.trigger('owl.play', 1000);
        })
        $(".stop").click(function () {
            owl.trigger('owl.stop');
        })
    }
    
    $.ajax({
        url: "/Api/getListCarType",
        cache: false
    }).done(function (html) {

        var news = '{"news":' + html + '}';
        var json_parsed = $.parseJSON(news);
        $("#car_type2").html("");
        for (var i = 0; i < json_parsed.news.length; i++) {
            if (json_parsed.news[i]) {
                var name = json_parsed.news[i].name;
                $("#car_type2").append("<option value='" + name + "'>" + name + "</option>");
            }
        }
    });

    $.ajax({
        url: "/Api/getCarHireType",
        cache: false
    }).done(function (html) {
        var news = '{"news":' + html + '}';
        var json_parsed = $.parseJSON(news);
        $("#car_hire_type").html("");
        for (var i = 0; i < json_parsed.news.length; i++) {
            if (json_parsed.news[i]) {
                var name = json_parsed.news[i].name;
                $("#car_hire_type").append("<option value='" + name + "'>" + name + "</option>");
            }
        }
    });

    $.ajax({
        url: "/Api/getCarSize",
        cache: false
    }).done(function (html) {
        var news = '{"news":' + html + '}';
        var json_parsed = $.parseJSON(news);
        $("#car_size2").html("");
        for (var i = 0; i < json_parsed.news.length; i++) {
            if (json_parsed.news[i]) {
                var name = json_parsed.news[i].name;
                $("#car_size2").append("<option value='" + name + "'>" + name + "</option>");
            }
        }
    });

    var options = {
        map: ".map_canvas"
    };
    if ($("#car_from").length) {
        
        $("#car_from").geocomplete(options)
          .bind("geocode:result", function (event, result) {
              $("#lon1").val(result.geometry.location.lng());
              $("#lat1").val(result.geometry.location.lat());
              //alert("long" + result.geometry.location.lng() + ", lat=" + result.geometry.location.lat());
          })
          .bind("geocode:error", function (event, status) {
              $.log("ERROR: " + status);
          })
          .bind("geocode:multiple", function (event, results) {
              $.log("Multiple: " + results.length + " results found");
          });        
    }

    if ($("#car_to").length) {
        $("#car_to").geocomplete(options)
          .bind("geocode:result", function (event, result) {
              $("#lon2").val(result.geometry.location.lng());
              $("#lat2").val(result.geometry.location.lat());
              //alert("long" + result.geometry.location.lng() + ", lat=" + result.geometry.location.lat());
          })
          .bind("geocode:error", function (event, status) {
              $.log("ERROR: " + status);
          })
          .bind("geocode:multiple", function (event, results) {
              $.log("Multiple: " + results.length + " results found");
          });
    }

    /*===========HOME===========*/


})

/**if ($('#booking').length) {
    var length_booking = $('#booking > tbody > tr').length - 1;
    var row_hidden = [];
    var cycleTimer;
    function startCycle() {
        cycleTimer = setInterval(function () {
            var row_random = getRandomInt(0, length_booking);
            $('#booking > tbody > tr:eq(' + row_random + ')').fadeOut();
            row_hidden.push(row_random);
            //console.log(row_hidden.length);
            if (row_hidden.length > 3) {
                clearInterval(cycleTimer);
                row_hidden.forEach(function (entry) {
                    $('#booking > tbody > tr:eq(' + entry + ')').fadeIn();
                });
                row_hidden = [];
                setTimeout(startCycle, 3000); // restart after 5 seconds
            }
        }, 5000);
    }
    //// start to automatically cycle slides
    startCycle();

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
}*/



function timkiemtaixe(e) {
    e.preventDefault();
    if (document.getElementById('lat1').value === "" && document.getElementById('lng1').value === "") {
        //notifywarn('Vui lòng nhập địa chỉ đi');
        var msb = '<div class="alert alert-warning alert-dismissible"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> Vui lòng nhập điểm đi</div>';
        $('#place_from').parent('.field_row').append(msb).fadeIn('300');
        setTimeout(function () {
            $('#place_from').siblings('.alert').alert('close');
        }, 1500);
        document.getElementById('place_from').focus();
        return false;
    }
    if (document.getElementById('lat2').value === "" && document.getElementById('lng2').value === "") {
        //notifywarn('Vui lòng nhập địa chỉ đến');
        var msb = '<div class="alert alert-warning alert-dismissible"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> Vui lòng nhập điểm đến</div>';
        $('#place_to').parent('.field_row').append(msb).fadeIn('300');
        setTimeout(function () {
            $('#place_to').siblings('.alert').alert('close');
        }, 1500);
        document.getElementById('place_to').focus();
        return false;
    }

    if (document.getElementById('place_from').value === document.getElementById('place_to').value && document.getElementById('place_from').value !== "" && document.getElementById('place_to').value !== "") {
        notifywarn('Vị trí điểm đi không thể trùng vị trí đến');
       
        return false;
    }

    //if (document.getElementById('Loaixe_socho').value === "") {
    //    alert('Vui lòng chọn loại xe');
    //    document.getElementById('Loaixe_socho').focus();
    //    return false;
    //}
       

    var directionsDisplay = new google.maps.DirectionsRenderer;
    var directionsService = new google.maps.DirectionsService;

    var selectedMode = "DRIVING";
    //var khoangcach = "";
    var latlng1 = new google.maps.LatLng(document.getElementById('lat1').value, document.getElementById('lng1').value);
    var latlng2 = new google.maps.LatLng(document.getElementById('lat2').value, document.getElementById('lng2').value);
    directionsService.route({
        origin: latlng1,  // Diem di.
        destination: latlng2,  // Diem den
        // Note that Javascript allows us to access the constant
        // using square brackets and a string value as its
        // "property."
        travelMode: google.maps.TravelMode[selectedMode]
    }, function (response, status) {
        if (status == 'OK') {
            directionsDisplay.setDirections(response);
            //console.log(response);
            var khoangcach = response.routes[0].legs[0].distance.text.replace(/km/g, "").replace(/m/g, "");
            //console.log(khoangcach);

            var url = "/Home/TimTaiXe";
            url += "?lat1=" + document.getElementById('lat1').value + "&lng1=" + document.getElementById('lng1').value + "&lat2=" + document.getElementById('lat2').value + "&lng2=" + document.getElementById('lng2').value;
            url += "&from=" + document.getElementById('place_from').value + "&to=" + document.getElementById('place_to').value;
            url += "&loaixe=" + document.getElementById('Loaixe_socho').value + "&kc=" + khoangcach;
            url += "&nhaxe=" + document.getElementById('nhaxe').value;
            window.location.href = url;
            //alert(khoangcach);


        } else {
            window.alert('Vui lòng nhập lại địa chỉ ' + status);
        }
    });
}

//function handleLocationError(browserHasGeolocation, infoWindow) {
//    infoWindow.setContent(browserHasGeolocation ?
//                          'Error: The Geolocation service failed.' :
//                          'Error: Your browser doesn\'t support geolocation.');
//}

function swapValues() {
    
    var from = document.getElementById("place_from").value;
    var to = document.getElementById("place_to").value;
    var lat1 = document.getElementById("lat1").value;
    var lng1 = document.getElementById("lng1").value;
    var lat2 = document.getElementById("lat2").value;
    var lng2 = document.getElementById("lng2").value;
    if (from != "" && to != "" && lat1 != "" && lng1 != "" && lat2 != "" && lng2 != "") {
        document.getElementById("place_to").value = from;
        document.getElementById("place_from").value = to;
        document.getElementById("lat2").value = lat1;
        document.getElementById("lat1").value = lat2;
        document.getElementById("lng2").value = lng1;
        document.getElementById("lng1").value = lng2;
    } 

}

function booking() {
    var formdata = new FormData(); //FormData object
    var from_datetime = document.getElementById("from_datetime").value;

    if (document.getElementById("car_from").value == "" || document.getElementById("lon1").value == "") {
        alert("Nhập điểm đi!");
        document.getElementById("car_from").focus();
        return;
    }

    if (document.getElementById("car_to").value == "" || document.getElementById("lon2").value == "") {
        alert("Nhập điểm đến!");
        document.getElementById("car_to").focus();
        return;
    }
    if (document.getElementById("car_type2").value == "") {
        alert("Nhập loại xe!");
        document.getElementById("car_type2").focus();
        return;
    }
    if (document.getElementById("car_hire_type").value == "") {
        alert("Nhập hình thức đi!");
        document.getElementById("car_hire_type").focus();
        return;
    }
    if (document.getElementById("car_size2").value == "") {
        alert("Nhập số chỗ!");
        document.getElementById("car_size2").focus();
        return;
    }
    if (document.getElementById("from_datetime").value == "") {
        alert("Ngày giờ đi!");
        document.getElementById("from_datetime").focus();
        return;
    }
    if (document.getElementById("to_datetime").value == "") {
        alert("Ngày giờ về!");
        document.getElementById("to_datetime").focus();
        return;
    }
    if (document.getElementById("name").value == "") {
        alert("Nhập họ tên!");
        document.getElementById("name").focus();
        return;
    }
    if (document.getElementById("phone").value == "") {
        alert("Nhập số điện thoại!");
        document.getElementById("phone").focus();
        return;
    }

    formdata.append("car_from", document.getElementById("car_from").value);
    formdata.append("car_to", document.getElementById("car_to").value);
    formdata.append("car_type", document.getElementById("car_type2").value);
    formdata.append("car_hire_type", document.getElementById("car_hire_type").value);
    formdata.append("car_size", document.getElementById("car_size2").value);
    formdata.append("from_datetime", document.getElementById("from_datetime").value);
    formdata.append("to_datetime", document.getElementById("to_datetime").value);
    formdata.append("lat1", document.getElementById("lat1").value);
    formdata.append("lon1", document.getElementById("lon1").value);
    formdata.append("lat2", document.getElementById("lat2").value);
    formdata.append("lon2", document.getElementById("lon2").value);
    formdata.append("name", document.getElementById("name").value);
    formdata.append("phone", document.getElementById("phone").value);
    document.getElementById("btnregister111").value = "Đang đặt xe xin chờ....";
    document.getElementById("btnregister111").disabled = true;

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/bookingdatxe');
    xhr.send(formdata);
    var content = "";
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (xhr.responseText == "1") {
                alert("Bạn đã đặt xe thành công! Tài xế sẽ liên hệ lại sớm nhất với bạn!");
            } else {
                alert("Chương trình đang cập nhật, xin quay lại sau!");
            }
            document.getElementById("btnregister111").value = "XÁC NHẬN";
            document.getElementById("btnregister111").disabled = false;
        }
    }
}