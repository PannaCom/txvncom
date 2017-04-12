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
        bounds: true,
        country: null,
        map: "#map-canvas",
        details: false,
        detailsAttribute: "name",
        detailsScope: null,
        autoselect: true,
        location: false,

        mapOptions: {
            zoom: 14,
            scrollwheel: false,
            mapTypeId: "roadmap"
        },

        markerOptions: {
            draggable: false
        },

        maxZoom: 16,
        types: [],
        blur: false,
        geocodeAfterResult: false,
        restoreValueAfterBlur: false
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

    /**
     * Fix error between range datetime picker https://github.com/xdan/datetimepicker/issues/82
     *
     */
    
    if ($('#date_go').length && $('#date_to').length) {
        jQuery(function () {
        //var d = new Date();
        //var s = d.toLocaleString();
        $('#date_go').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'vi',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            format: 'd/m/Y H:i',            
            mask: false,
            step: 5,
            minDate: 0,         
            onShow: function (ct) {
                this.setOptions({
                    maxDate: $('#date_to').val() ? getDate($('#date_to').val()) : false,
                    formatDate: 'd/m/Y',
                })
            },
            timepicker: true
        });

        $('#date_to').datetimepicker({
            dayOfWeekStart: 1,
            lang: 'vi',
            disabledDates: ['1986/01/08', '1986/01/09', '1986/01/10'],
            format: 'd/m/Y H:i',
            mask: false,
            step: 5,           
            onShow: function (ct) {
                this.setOptions({
                    minDate: $('#date_go').val() ? getDate($('#date_go').val()) : false,
                    formatDate: 'd/m/Y',
                })
            },
            timepicker: true
        });

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

function getDate(datum) { fn = datum.split(" "); fn0 = fn[0].split("/"); fn1 = fn[1].split(":"); return fn0[0] + '/' + fn0[1] + '/' + fn0[2]; }

/**
 * and add the following function:
 * function getDate(datum) { fn = datum.split("-"); return fn[0] + '/' + fn[1] + '/' + fn[2]; }
 * for german time (d.m.Y) use this function
 * function getDate(datum) { fn = datum.split("."); return fn[2] + '/' + fn[1] + '/' + fn[0]; }
 * 
 */


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
    //console.log(parseDate(getDate($('#date_to').val())));
    //console.log(parseDate(getDate($('#date_go').val())));
    //console.log(daydiff(parseDate(getDate($('#date_to').val())), parseDate(getDate($('#date_go').val()))));
    if (document.getElementById('place_from').value === "") {
        var msb = 'Vui lòng nhập điểm đi';
        //var msb = '<div class="alert alert-warning alert-dismissible"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> Vui lòng nhập điểm đi</div>';
        //$('#place_from').parent('.field_row').append(msb).fadeIn('300');
        $('#place_from').popover({ placement: 'bottom', content: msb });
        $('#place_from').popover('show');
        document.getElementById('place_from').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_from').siblings('.alert').alert('close');
            $('#place_from').popover('hide')
        }, 600);
    }

    if (document.getElementById('place_to').value === "") {
        var msb = 'Vui lòng nhập điểm tới';
        //var msb = '<div class="alert alert-warning alert-dismissible"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> Vui lòng nhập điểm đến</div>';
        //$('#place_to').parent('.field_row').append(msb).fadeIn('300');
        $('#place_to').popover({ placement: 'bottom', content: msb });
        $('#place_to').popover('show');
        document.getElementById('place_to').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_to').popover('hide')
        }, 600);
    }


    if (document.getElementById('lat1').value === "" && document.getElementById('lng1').value === "") {
        var msb = 'Vui lòng nhập điểm tới';
        $('#place_from').popover({ placement: 'bottom', content: msb });
        $('#place_from').popover('show');
        document.getElementById('place_from').focus();
        return false;
    }
    else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_from').popover('hide')
        }, 600);
    }

    if (document.getElementById('lat2').value === "" && document.getElementById('lng2').value === "") {
        var msb = 'Vui lòng nhập điểm tới';
        $('#place_to').popover({ placement: 'bottom', content: msb });
        $('#place_to').popover('show');
        document.getElementById('place_to').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_to').popover('hide')
        }, 600);
    }

    if (document.getElementById('place_from').value === document.getElementById('place_to').value && document.getElementById('place_from').value !== "" && document.getElementById('place_to').value !== "") {
        notifywarn('Vị trí điểm đi không thể trùng vị trí đến');
       
        return false;
    }

    if ($('#date_go').val() === "") {
        var msb = 'Vui lòng nhập ngày đi';

        $('#date_go').popover({ placement: 'bottom', content: msb });
        $('#date_go').popover('show');
        //document.getElementById('date_go').focus();
        return false;
    } else {
        setTimeout(function () {
            $('#date_go').popover('hide')
        }, 600);
    }

    if (document.getElementById('type_go').value === '2') {
        if ($('#date_to').val() === "") {
            var msb = 'Vui lòng nhập ngày đến';

            $('#date_to').popover({ placement: 'bottom', content: msb });
            $('#date_to').popover('show');
            //document.getElementById('date_to').focus();
            return false;
        }
    }    

    //if (daydiff(parseDate(getDate($('#date_to').val())), parseDate(getDate($('#date_go').val()))) === 0) {
    //    console.log(daydiff(parseDate(getDate($('#date_to').val())), parseDate(getDate($('#date_go').val()))));
    //    var msb = 'Ngày đến phải lớn ngày đi ít nhất 1 ngày trở lên';

    //    $('#date_to').popover({ placement: 'bottom', content: msb });
    //    $('#date_to').popover('show');
    //    //document.getElementById('date_to').focus();
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
            var khoangcach = response.routes[0].legs[0].distance.text.replace(/km/g, "").replace(/m/g, "").replace(",",".");
            //console.log(khoangcach);

            var url = "/Home/TimTaiXe";
            url += "?lat1=" + document.getElementById('lat1').value + "&lng1=" + document.getElementById('lng1').value + "&lat2=" + document.getElementById('lat2').value + "&lng2=" + document.getElementById('lng2').value;
            url += "&from=" + document.getElementById('place_from').value + "&to=" + document.getElementById('place_to').value;
            url += "&loaixe=" + document.getElementById('Loaixe_socho').value + "&kc=" + khoangcach;
            url += "&nhaxe=" + document.getElementById('nhaxe').value;
            url += "&date_go=" + document.getElementById('date_go').value;
            url += "&date_to=" + document.getElementById('date_to').value;
            url += "&type_go=" + document.getElementById('type_go').value;
            window.location.href = url;
            //alert(khoangcach);


        } else {
            window.alert('Vui lòng nhập lại địa chỉ ' + status);
        }
    });
}

function timkiemtaixe2(e) {
    e.preventDefault();

    if (document.getElementById('place_from').value === "") {
        var msb = 'Vui lòng nhập điểm đi';
        $('#place_from').popover({ placement: 'bottom', content: msb });
        $('#place_from').popover('show');
        document.getElementById('place_from').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_from').popover('hide')
        }, 600);
    }

    if (document.getElementById('place_to').value === "") {
        //notifywarn('Vui lòng nhập địa chỉ đi');
        var msb = 'Vui lòng nhập điểm đến';
        $('#place_to').popover({ placement: 'bottom', content: msb });
        $('#place_to').popover('show');
        document.getElementById('place_to').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_to').popover('hide')
        }, 600);
    }


    if (document.getElementById('lat1').value === "" && document.getElementById('lng1').value === "") {
        var msb = 'Vui lòng nhập điểm đi';
        $('#place_from').popover({ placement: 'bottom', content: msb });
        $('#place_from').popover('show');
        document.getElementById('place_from').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_from').popover('hide')
        }, 600);
    }

    if (document.getElementById('lat2').value === "" && document.getElementById('lng2').value === "") {
        var msb = 'Vui lòng nhập điểm đến';
        $('#place_to').popover({ placement: 'bottom', content: msb });
        $('#place_to').popover('show');
        document.getElementById('place_to').focus();
        return false;
    } else {
        setTimeout(function () {
            //$('#place_to').siblings('.alert').alert('close');
            $('#place_to').popover('hide')
        }, 600);
    }

    if (document.getElementById('place_from').value === document.getElementById('place_to').value && document.getElementById('place_from').value !== "" && document.getElementById('place_to').value !== "") {
        notifywarn('Vị trí điểm đi không thể trùng vị trí đến');

        return false;
    }

    if ($('#date_go').val() === "") {
        var msb = 'Vui lòng nhập ngày đi';

        $('#date_go').popover({ placement: 'bottom', content: msb });
        $('#date_go').popover('show');
        //document.getElementById('date_go').focus();
        return false;
    } else {
        setTimeout(function () {
            $('#date_go').popover('hide')
        }, 600);
    }

    if (document.getElementById('type_go').value === '2') {
        if ($('#date_to').val() === "") {
            var msb = 'Vui lòng nhập ngày đến';

            $('#date_to').popover({ placement: 'bottom', content: msb });
            $('#date_to').popover('show');
            //document.getElementById('date_to').focus();
            return false;
        }
    }

    //if (daydiff(parseDate(getDate($('#date_to').val())), parseDate(getDate($('#date_go').val()))) === 0) {
    //    console.log(daydiff(parseDate(getDate($('#date_to').val())), parseDate(getDate($('#date_go').val()))));
    //    var msb = 'Ngày đến phải lớn ngày đi ít nhất 1 ngày trở lên';

    //    $('#date_to').popover({ placement: 'bottom', content: msb });
    //    $('#date_to').popover('show');
    //    //document.getElementById('date_to').focus();
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
            var khoangcach = response.routes[0].legs[0].distance.text.replace(/km/g, "").replace(/m/g, "").replace(",",".");
            //console.log(khoangcach);

            var url = "/Home/TimTaiXe";
            url += "?lat1=" + document.getElementById('lat1').value + "&lng1=" + document.getElementById('lng1').value + "&lat2=" + document.getElementById('lat2').value + "&lng2=" + document.getElementById('lng2').value;
            url += "&from=" + document.getElementById('place_from').value + "&to=" + document.getElementById('place_to').value;
            url += "&loaixe=" + document.getElementById('Loaixe_socho').value + "&kc=" + khoangcach;
            url += "&date_go=" + document.getElementById('date_go').value;
            url += "&date_to=" + document.getElementById('date_to').value;
            url += "&type_go=" + document.getElementById('type_go').value;
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

function getdate_go() {
    console.log(getDate($('#date_go').val()));
}

function func_datxenhanh(id, carsize, e) {
    e.preventDefault();
    if (document.getElementById('lat1').value === "" && document.getElementById('lng1').value === "") {
        alert('Vui lòng nhập điểm đi.');
        return false;
    }
    if (document.getElementById('lat2').value === "" && document.getElementById('lng2').value === "") {
        alert('Vui lòng nhập điểm đến.');
        return false;
    }
    if (id) {
        var diemdi = document.getElementById('place_from').value;
        var diemden = document.getElementById('place_to').value;
        var kcc = document.getElementById('kc_duongdi').value;
        var type_go = document.getElementById('type_go').value;
        var date_go = document.getElementById('date_go').value;
        var date_to = document.getElementById('date_to').value;       
        $.get('/Home/getModaldatxenhanh?driver_id=' + id + "&diemdi=" + diemdi + "&diemden=" + diemden + "&kcc=" + kcc + "&type_go=" + type_go + "&date_go=" + date_go + "&date_to=" + date_to + "&carsize=" + carsize, function (html) {
            $('#modal-7 .modal-body').empty().html(html);
            $('#modal-7').modal('show', { backdrop: 'static', keyboard: false });
        })
    }

}

function saveBookingToDriver(id) {
    document.getElementById('lon_from').value = document.getElementById('lng1').value;
    document.getElementById('lat_from').value = document.getElementById('lat1').value;
    document.getElementById('lon_to').value = document.getElementById('lng2').value;
    document.getElementById('lat_to').value = document.getElementById('lat2').value;
    document.getElementById('from_place').value = document.getElementById('place_from').value;
    document.getElementById('to_place').value = document.getElementById('place_to').value;
    //document.getElementById('car_type_made_model').value = $('#car_type_made_model_' + id).html();
    //document.getElementById('price_driver').value = $('#price_driver_' + id).html().replace(',', '');
    //document.getElementById('total_money').value = $('#total_money_driver_' + id).html().replace(',', '');
    document.getElementById('distance').value = document.getElementById('kc_duongdi').value;

    var url = "/Home/saveBookingToDriver"; // the script where you handle the form input.
    if (document.getElementById('customer_name').value === "") {
        alert("Vui lòng nhập họ tên khách hàng.");
        document.getElementById('customer_name').focus();
        return false;
    }
    if (document.getElementById('customer_phone').value === "") {
        alert("Vui lòng nhập số điện thoại khách hàng.");
        document.getElementById('customer_phone').focus();
        return false;
    }
    if (document.getElementById('from_date').value === "") {
        alert("Vui lòng nhập ngày đến.");
        document.getElementById('from_date').focus();
        return false;
    }
    if (document.getElementById('to_date').value === "") {
        alert("Vui lòng nhập ngày đi.");
        document.getElementById('to_date').focus();
        return false;
    }
    if (document.getElementById('email_cus').value === "") {
        alert("Vui lòng nhập email.");
        document.getElementById('email_cus').focus();
        return false;
    }
    
    if (document.getElementById('from_date').value > document.getElementById('to_date').value) {
        alert("Ngày đi phải sau ngày đến");
        document.getElementById('from_date').focus();
        return false;
    }

    document.getElementById("btn_bookingtodriver").innerHTML = "Đang đặt xe...";
    document.getElementById("btn_bookingtodriver").disabled = true;

    $.ajax({
        type: "POST",
        url: url,
        data: $("#form_dat_thue_xe").serialize(), // serializes the form's elements.
        success: function (data) {
            if (data == 1) {
                //document.getElementById("btn_bookingtodriver").innerHTML = "Đặt xe";
                //document.getElementById("btn_bookingtodriver").disabled = false;

                $('#modal-7 .modal-body').empty().html("<p>Cám ơn đã đặt xe tại hệ thống thuexevn. Vui lòng chờ trong ít phút để chúng tôi xử lý yêu cầu của bạn.</p>");

                ////reset value               
            }

        }
    });
}