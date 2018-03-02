"use strict";
$('#query-btn').click(function () {
    let begin = $('#begin').val();
    let end = $('#end').val();

    $.getJSON('/api/Record/Query', { begin: begin, end: end }, function (response) {
        $('#query-result').html(JSON.stringify(response, true, 2));
    });
});

$('#signin-btn').click(function () {
    let seat = $('#seat').val();
    let auth = hello(helloJsSignInSignUpPolicy).getAuthResponse();

    $.ajax('/api/Record/SignIn', {
        method: 'post',
        headers: { 'Authorization': auth.token_type + ' ' + auth.access_token },
        data: { seat: seat },
        dataType: 'json',
        success: function (response) {
            $('#signin-result').html(JSON.stringify(response, true, 2));
        }
    });
});

$('#signout-btn').click(function () {
    let auth = hello(helloJsSignInSignUpPolicy).getAuthResponse();

    $.ajax('/api/Record/SignOut', {
        method: 'post',
        headers: { 'Authorization': auth.token_type + ' ' + auth.access_token },
        data: { seat: 11 },
        dataType: 'json',
        success: function (response) {
            $('#signout-result').html(JSON.stringify(response, true, 2));
        }
    });
});