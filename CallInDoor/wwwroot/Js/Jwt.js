var token = localStorage.getItem('token') || sessionStorage.getItem('token');
(function () {
    if (!userInfo) location.href = '/Admin/Login';
    else token = JSON.parse(token);
})();

var setAuthHeader = function (request) {
    request.setRequestHeader('Authorization', 'Bearer ' + token);
    alert(token)
}
