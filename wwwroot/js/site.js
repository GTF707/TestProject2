var map = undefined;
var type = "";
var marker = [];
var polygon = undefined;
var layers = [];

map = L.map('map').setView([55.75411396899196, 37.62039668151654], 10);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 20,
    attribution: '©TestProject2',
}).addTo(map);

L.tileLayer.wms('http://localhost:8080/geoserver/TestProject2/wms', {
    layers: 'shape',
    format: 'image/png',
    transparent: true
}).addTo(map);



map.on('click', function (event) {
    var data = JSON.stringify({
        "point": L.marker(event.latlng)
    })
    fetch("http://localhost:5175/Shape/CheckShape", {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data
    }).then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    });
});

function showMenu() {
    const popup = document.getElementById("popup")
    popup.style.display = "flex"
}

function hideMenu() {
    const popup = document.getElementById("popup")
    popup.style.display = "none"
}


function Point() {

    if (type === "marker") {
        type = "";
        map.off('click');
        return;
    }
    map.off('click');
    type = "marker";

    map.on('click', function (event) {

        var mark = L.marker(event.latlng, { draggable: true }).addTo(map);
        layers.push(mark.toGeoJSON());
        map.off('click');
        type = "";
    });
}


function Poligon() {


    var polygonCoords = [];
    markerk = [];
    if (type === "poligon") {
        type = "";
        map.off('click');

        return;
    }
    map.off('click');
    type = "poligon";
    map.on('click', function (event) {
        polygonCoords.push(event.latlng);
        marker.push(L.circleMarker(event.latlng, { color: 'blue', radius: 2 }).addTo(map));
        if (polygonCoords.length > 2) {
            if (!polygon) {
                polygon = L.polygon(polygonCoords, { color: 'blue' }).addTo(map);
                layers.push(polygon);
            } else {
                polygon.setLatLngs(polygonCoords);
            }
        }


    });
}

function Rectangle() {
    var startLatLng;
    var endLatLng;
    var rectangle;
    if (type === "rectangle") {
        type = "";
        map.off('click');
        return;
    }

    map.off('click');
    type = "rectangle"
    map.on('click', function (event) {
        marker.push(L.circleMarker(event.latlng, { color: 'black', radius: 2 }).addTo(map));
        if (!startLatLng) {
            startLatLng = event.latlng;
        } else if (!endLatLng) {
            endLatLng = event.latlng;
            var points = L.latLngBounds(startLatLng, endLatLng);
            rectangle = L.rectangle(points, { color: 'black' }).addTo(map);

            layers.push(rectangle.toGeoJSON());


            startLatLng = null;
            endLatLng = null;
        }
    });

}
function removeMarkers(markers) {
    for (i = 0; i <= markers.length; i++) {
        map.removeLayer(markers[i])
    }
}



document.addEventListener("DOMContentLoaded", function () {
    var openModalBtn = document.getElementById("openModalBtn");
    var modal = document.getElementById("myModal");
    var closeModalSpan = document.getElementsByClassName("close")[0];


    openModalBtn.addEventListener("click", function () {
        modal.style.display = "block";
        map.off('click');
        type = "";
    });

    closeModalSpan.addEventListener("click", function () {
        modal.style.display = "none";
    });

    window.addEventListener("click", function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    });

    saveBtn.addEventListener("click", function () {


        removeMarkers(marker);
        modal.style.display = "none";
    });


});


function Save() {
    if (polygon) {
        layers.push(polygon.toGeoJSON());
    }

    const data = JSON.stringify({
        "Director": document.getElementById("directorInput").value,
        "Address": document.getElementById("addressInput").value,
        "TypeActivity": document.getElementById("activityInput").value,
        "type": "FeatureCollection",
        "features": layers
    })

    fetch("http://localhost:5175/Shape/Create", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data
    })

}





