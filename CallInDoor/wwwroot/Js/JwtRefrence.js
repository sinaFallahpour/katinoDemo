var token = localStorage.getItem('token') || sessionStorage.getItem('token');
if (!token) location.href = '/Refrence/Login';

var setAuthHeader = function (request) {
    request.setRequestHeader('Authorization', 'Bearer ' + token);
}
