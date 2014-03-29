(function(window) {
    var i, $sound, $buttonGroup;
 
    var $sounds = $(".sound");
    var clientId = require("config").get("client_id");
    var oauthToken = require("lib/connect").getAuthToken();
    var conversionHelper = require("lib/helpers/conversion-helper");
    var $downloadButton, size;
    var params, downloadUrl, onSuccess;
 
    for (i = $sounds.length - 1; i >= 0; i--) {
        $sound = $($sounds[i]);
 
        var soundcloudUrl = "https://soundcloud.com" + ($sound.find(".soundTitle__title").attr("href") || window.location.pathname);
 
        params = {
            url: soundcloudUrl,
            client_id: clientId
        };
 
        onSuccess = (function($sound) {
            return function(data) {
                var params = {
                  client_id: clientId
                };
                downloadUrl = require("lib/url").stringify({ query: params }, data.stream_url + ".mp3");
				
            };
        })($sound);
 
        $.getJSON("http://api.soundcloud.com/resolve.json", params).success(onSuccess);
    }
})(window);