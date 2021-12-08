"use strict";


//notif


try {
    Notification.requestPermission()
        .then(() => console.log('granted'));
} catch (error) {
    if (error instanceof TypeError) {
        new Notification.requestPermission(() => {
            console.log('granted');
        });
    } else {
        // alert(error);
    }
}
function Notif(body) {
    new Notification('کاتینو', { body: body, icon: '/Img/CompanyLogo/logo.png' });

}

//notif





var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionId = null;
var isConnected = false;
connection.on("Bell", function (data) {
    //alert()
    Notif(data);
    
});

//Sefareshe Jadid **Connection**
connection.on("NewRequest", function () {
    GetNewRequests();
});







connection.on("Paid", function () {
    $('#paid').empty();
    $('#paid').html('<div class="row mt-5">' +
        '<div class="col-lg-12 col-md-12 col-sm-12 col-12 position-relative " >' +
        '<i class="fa fa-3x fa-check text-success bg-white rounded-circle position-fixed"></i>' +
        '<button href="#" style="color:white" class="hj-cart-submit1 w-100 btn btn-block confirmBascket text-white btn-success font-weight-bold" type="button" disabled>پرداخت شد</button>' +
        '</div >' +
        '</div >');
});

connection.start().then(function () {
    isConnected = true;
}).catch(function (err) {
    return console.error(err.toString());
});
//Get Admin Connection
function GetConnection(username) {
    connection.invoke("GetConnection", username).catch(function (err) {
        return console.error(err.toString());
    });
}

//User Click On Bell
function play() {
    if ($('.warningElem').attr("data-status") == '1') {
        $('#foodalert').modal();
        $('.warningElem').attr('data-status', '0');
        $('.fa-bell').removeClass('text-gold').addClass('text-gray');
        $.ajax({
            url: '/restaurant/Now',
            method: 'GET',
            success: function (data) {
                $.cookie('bellTime', data, { expires: 1, path: '/' });
            }
        })
        let audio = document.getElementById("bell");
        audio.play();
        //var socket = io(ip).emit("need garson", { 'user': table, 'admin': user });
        setTimeout(function () {
            $('.warningElem').attr('data-status', '1');
            $('.fa-bell').removeClass('text-gray').addClass('text-gold');
        }, 60000)
        connection.invoke("Bell", $.cookie("storeCode"), $.cookie("table")).catch(function (err) {
            return console.error(err.toString());
        });
    }
    //INJA
}

function NewRequest() {
    connection.invoke("NewRequest", $.cookie("storeCode"), $.cookie("table")).catch(function (err) {
        return console.error(err.toString());
    });
}

function RequestConnection() {
    if ($.cookie("request") != null) {
        connection.invoke("RequestConnection", $.cookie("request")).catch(function (err) {
            return console.error(err.toString());
        });
    }
}

function Paid(id) {
    connection.invoke("Paid", id).catch(function (err) {
        return console.error(err.toString());
    });
}