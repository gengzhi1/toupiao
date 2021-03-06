﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function zlalert(message, alert_type = 'warning',timeout=4500 ) {
    var alert_html =  `
        <div class="alert alert-${alert_type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
    `;
    $('body').append(alert_html);

    $('.alert').css({ 'top': $(document).scrollTop()});

    setTimeout(() => {
        $('.alert').remove();
    }, timeout);
}

function updateQueryStringParameter(uri, key, value) {
    uri = String(uri); key = String(key); value = String(value);
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator =String( uri).indexOf('?') !== -1 ? "&" : "?";
    if (value.length < 1) {
        return uri.replace(re, '$1', '');
    }
    else if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}


function updateQueryStringParameters(uri, keyvalues) {

    for (var key in keyvalues) {
        uri = updateQueryStringParameter(uri, key, keyvalues[key] );
    }
    return uri;

}
// 即执行函数, 这里的代码会被直接运行
(function () {
    $(document).on('click', '#index_search', (event) => {
        if ($('#index_search_input').val().length < 1) return;

        location.href = updateQueryStringParameter(
            location.href,
            'kw', $('#index_search_input').val());
    });

    $(document).on('click', '.to-page', (event) => {
        location.href = updateQueryStringParameter(
            location.href, 'pageNumber',
            $(event.currentTarget).data('page_number'));
    });
}());