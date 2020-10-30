const myMSALObj = new Msal.UserAgentApplication(msalConfig);
const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;
let accessToken;


/* 

[Choosing between a pop-up or redirect experience](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-spa-sign-in?tabs=javascript#choosing-between-a-pop-up-or-redirect-experience)

You can sign in users to your application in MSAL.js in two ways:
1. Pop-up window, by using the loginPopup method
  If you don't want users to move away from your main application page 
  during authentication, we recommend the pop-up method. 
  Because the authentication redirect happens in a pop-up window, 
  the state of the main application is preserved.

2. Redirect, by using the loginRedirect method
  If users have browser constraints or policies where pop-up windows are disabled, 
  you can use the redirect method. Use the redirect method with the Internet Explorer browser, 
  because there are known issues with pop-up windows on Internet Explorer.
  see: https://github.com/AzureAD/microsoft-authentication-library-for-js/wiki/Known-issues-on-IE-and-Edge-Browser

*/
//const useRedirect = isIE;
const useRedirect = true;

if (useRedirect) {
  // Register Callbacks for Redirect flow
  myMSALObj.handleRedirectCallback(authRedirectCallBack);
}

function authRedirectCallBack(error, response) {
  if (error) {
    console.log(error);
  } else {
    if (response.tokenType === "id_token") {
      console.log('id_token acquired at: ' + new Date().toString());
    } else if (response.tokenType === "access_token") {
      console.log('access_token acquired at: ' + new Date().toString());
      accessToken = response.accessToken;

      callMSGraph(graphConfig.graphMailEndpoint, accessToken, updateUI);
      profileButton.style.display = 'none';
      mailButton.style.display = 'initial';
    } else {
      console.log("token type is:" + response.tokenType);
    }
  }
}

if (useRedirect) {
  // Redirect: once login is successful and redirects with tokens, call Graph API
  if (myMSALObj.getAccount()) {
    showWelcomeMessage(myMSALObj.getAccount());
  }
}

function signIn() {
  if (useRedirect) {
    myMSALObj.loginRedirect(loginRequest);
  }
  else {
    myMSALObj.loginPopup(loginRequest).then(function (loginResponse) {
      console.log('id_token acquired at: ' + new Date().toString());
      console.log(loginResponse);

      if (myMSALObj.getAccount()) {
        showWelcomeMessage(myMSALObj.getAccount());
      }
    }).catch(function(error) {
      console.log(error);
    });
  }
}

function signOut() {
  myMSALObj.logout();
}

// This function can be removed if you do not need to support IE
function getTokenRedirect(request, endpoint) {
  return myMSALObj.acquireTokenSilent(request, endpoint).then(function(response) {
      console.log(response);
      if (response.accessToken) {
        console.log('access_token acquired at: ' + new Date().toString());
        accessToken = response.accessToken;

        callMSGraph(endpoint, response.accessToken, updateUI);
        profileButton.style.display = 'none';
        mailButton.style.display = 'initial';
      }
  }).catch(function (error) {
      console.log("silent token acquisition fails. acquiring token using redirect");
      // fallback to interaction when silent call fails
      return myMSALObj.acquireTokenRedirect(request)
    });
}

function callMSGraph(theUrl, accessToken, callback) {
  var xmlHttp = new XMLHttpRequest();
  xmlHttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
      callback(JSON.parse(this.responseText));
    }
  }
  xmlHttp.open("GET", theUrl, true); // true for asynchronous
  xmlHttp.setRequestHeader('Authorization', 'Bearer ' + accessToken);
  xmlHttp.send();
}

function getTokenPopup(request) {
  return myMSALObj.acquireTokenSilent(request).catch(function (error) {
    console.log(error);
    console.log("silent token acquisition fails. acquiring token using popup");

    // fallback to interaction when silent call fails
    return myMSALObj.acquireTokenPopup(request).then(function (tokenResponse) {
      return tokenResponse;
    }).catch(function (error) {
      console.log(error);
    });
  });
}

function seeProfile() {
  if (useRedirect) {
    getTokenRedirect(loginRequest, graphConfig.graphMeEndpoint);
  } else {
    if (myMSALObj.getAccount()) {
      getTokenPopup(loginRequest).then(function (response) {
          callMSGraph(graphConfig.graphMeEndpoint, response.accessToken, updateUI);
          profileButton.classList.add('d-none');
          mailButton.classList.remove('d-none');
        }).catch(function (error) {
          console.log(error);
        });
    }
  }
}

function readMail() {
  if (useRedirect) {
    getTokenRedirect(tokenRequest, graphConfig.graphMailEndpoint);
  } else {
    if (myMSALObj.getAccount()) {
      getTokenPopup(tokenRequest).then(function (response) {
          callMSGraph(graphConfig.graphMailEndpoint, response.accessToken, updateUI);
        }).catch(function (error) {
          console.log(error);
        });
    }
  }
}

setTimeout(function () {
    if (myMSALObj.getAccount()) {
        showWelcomeMessage(myMSALObj.getAccount());
        $('#info').text("Already signed in O365. Redirect to Sharepoint in 5 seconds...").css({ 'background-color': '#44b549' });
        setTimeout(function () {
            window.location.href = spUrl;
        }, 1000 * 5);
    } else {
        $('#info').text("Not signed in O365. Rediret to O365 login page in 5 seconds...");
        setTimeout(function () {
            signIn();
        }, 1000 * 5);
    }
}, 1000 * 0.5);