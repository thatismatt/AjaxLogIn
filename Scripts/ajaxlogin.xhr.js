ajaxlogin.Xhr = {
    Get: function (url, data, callback) {
        ajaxlogin.Xhr.Ajax(url, data, callback, 'get');
    },
    Post: function (url, data, callback) {
        ajaxlogin.Xhr.Ajax(url, data, callback, 'post');
    },
    Ajax: function (url, data, callback, type) {
        if (!callback) {
            callback = data;
            data = null;
        }
        $.ajax({
            url: url,
            cache: false,
            success: function (r) {
                if (r.Success) {
                    callback(r.Data);
                } else {
                    console.log(url, data, callback, type, r);
                    throw "ooops..";
                    //alert(r.Error || "Something went wrong...");
                }
            },
            type: type,
            data: data
        });
    }
};