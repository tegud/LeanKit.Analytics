(function () {
    $('#toggle-menu').on('click', function () {
        $('#top-bar').toggleClass('collapsed');
        $(this).toggleClass('collapsed');
    });
})();