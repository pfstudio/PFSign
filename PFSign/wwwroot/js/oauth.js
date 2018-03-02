"use strict";
//applicationID created in AD B2C portal
var applicationId = 'e40f6137-3433-4958-ba2b-a4a3765e3298';

//API url
var apiURL = 'https://pfstudio2018.onmicrosoft.com/pfsign/';
var scope = apiURL + 'sign';

// Do not modify
var responseType = 'token id_token';
var redirectURI = '../redirect.html';

//initiate all policies
hello.init(
    {
        adB2CSignInSignUp: applicationId
    },
    {
        redirect_uri: redirectURI,
        scope: 'openid ' + scope,
        response_type: responseType
    });

hello.on('auth.login',
    function (auth) {
        // get the raw response
        let response = hello(helloJsSignInSignUpPolicy).getAuthResponse();
        // get the id_token
        let id_token = response.id_token;
        // get the access_token
        let access_token = response.access_token;
        // check the tokens
        if (!id_token || !access_token) {
            alert('Login Failed!');
            return;
        }

        // decoded the id_token
        let decoded_id_token = jwt_decode(id_token);

        // refresh user info zone
        $('#user-info').html(`
            <h4 class="card-title">Hello, ${decoded_id_token.name}!</h4>
            <dl>
                <dt>Name</dt>
                <dd>${decoded_id_token.name}</dd>
                <dt>StudentId</dt>
                <dd>${decoded_id_token.extension_StudentId}</dd>
                <dt>Email</dt>
                <dd>${decoded_id_token.emails}</dd>
            </dl>
        `);
        $('#oauth-info').hide();

        // set token zone
        $('#id-token').attr('href', 'https://jwt.ms/#id_token=' + id_token);
        $('#access-token').attr('href', 'https://jwt.ms/#access_token=' + access_token);
        let d = new Date(0);
        d.setUTCSeconds(decoded_id_token.exp);
        $('#token-info').html(`
            <dl>
                <dt>expiration time</dt>
                <dd>${d}</dd>
            </dl>
        `);
    });
hello.on('auth.logout',
    function (auth) {
        // reset user info zone
        $('#oauth-info').show();
        $('#user-info').html('');

        // reset token zone
        $('#id-token').attr('href', '#');
        $('#access-token').attr('href', '#');
        $('#token-info').html('');

        // refresh the page
        location.reload();
    });

function login() {
    hello.login(helloJsSignInSignUpPolicy);
}

function logout() {
    hello.logout(helloJsSignInSignUpPolicy);
}

function refresh() {
    hello.login(helloJsSignInSignUpPolicy, { display: 'none' });
}