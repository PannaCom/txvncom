$(document).ready(function () {
    // tim kiem tai xe
    $('#getmaplocation').tooltip({
        placement: 'left',
        title: 'Lấy vị trí hiện tại'
    })
    $('#movemaplocation').tooltip({
        placement: 'left',
        title: 'Di chuyển địa điểm'
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

    // end 
})

function timkiemtaixe() {
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
    var khoangcach = "";
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
            console.log(response);
            khoangcach = response.routes[0].legs[0].distance.text.replace(" km", "");


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