(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.StopStart = function (element) {
        function toggleRefresh(stop) {
            for (var x = 0; x < window.frames.length; x++) {
                window.frames[x].postMessage(stop ? 'stop' : 'start', '*');
            }
        }

        element.on('click', function () {
            var button = $(this),
                stop;

            if (button.hasClass('stopped')) {
                button.removeClass('stopped');
            } else {
                button.addClass('stopped');
                stop = true;
            }

            toggleRefresh(stop);
        });

        $(document).on('webkitvisibilitychange msvisibilitychange mozvisibilitychange visibilitychange', function () {
            var isHidden = document.hidden || document.mozHidden || document.msHidden || document.webkitHidden;

            toggleRefresh(isHidden);
        });
    };
})();