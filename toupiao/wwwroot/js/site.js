// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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