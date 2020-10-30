
/*
 * dc1979.onmicrosoft.com
const msalConfig = {
  auth: {
    clientId: "d82ee612-17b3-401a-81a8-769da2669bac",
    authority: "https://login.microsoftonline.com/becd6529-4f7f-4be6-85e6-ab8679470cd7",
    redirectUri: "https://azure-ad-spa-02.dc1979.com/",
  },
  cache: {
    cacheLocation: "localStorage", // This configures where your cache will be stored
    storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  }
};
*/

/* SP-Wechat-POC  kpltest.onmicrosoft.com */
const msalConfig = {
    auth: {
        clientId: "47026730-2c7a-413a-8b82-a43faecf3c37",
        authority: "https://login.microsoftonline.com/f074dbeb-1cc3-4857-a3ba-17253091f5d3",
        redirectUri: "https://weixin.dc1979.com/oauth/spa",
    },
    cache: {
        cacheLocation: "localStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    }
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
const loginRequest = {
  scopes: ["openid", "profile", "User.Read"]
};

// Add here scopes for access token to be used at MS Graph API endpoints.
const tokenRequest = {
  scopes: ["Mail.Read"]
};

const spUrl = "https://kpltest.sharepoint.com/";