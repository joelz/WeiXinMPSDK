function callMSGraph(endpoint, token, callback) {
  console.log('request made to Graph API at: ' + new Date().toString());

  $.ajax({
    type: "GET",
    beforeSend: function (xhr) {
      xhr.setRequestHeader('Authorization', 'bearer ' + token);
    },
    url: endpoint,
    success: function (res) {
      console.log(res);
      callback(res, endpoint);
    },
    error: function (error) {
      callback(error);
    }
  });

  // fetch(endpoint, options).then(function (response) { return response.json(); }).then(function (response) { callback(response, endpoint) }).catch(function (error) { console.log(error) });
}