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
})

function timkiemtaixe() {
    if (document.getElementById('place_from').value === document.getElementById('place_to').value) {
        alert('Vị trí điểm đi không thể trùng vị trí đến');
        return false;
    }
    if (document.getElementById('lat1').value === "" && document.getElementById('lng1').value === "") {
        alert('Vui lòng nhập địa chỉ từ');
        document.getElementById('place_from').focus();
        return false;
    }
    if (document.getElementById('lat2').value === "" && document.getElementById('lng2').value === "") {
        alert('Vui lòng nhập địa chỉ tới');
        document.getElementById('place_to').focus();
        return false;
    }

    if (document.getElementById('Loaixe_socho').value === "") {
        alert('Vui lòng chọn loại xe');
        document.getElementById('Loaixe_socho').focus();
        return false;
    }

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