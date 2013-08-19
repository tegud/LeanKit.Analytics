(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.StopStart = function (element) {
        element.on('click', function () {
            var button = $(this),
                stop;

            if (button.hasClass('stopped')) {
                button.removeClass('stopped');
            } else {
                button.addClass('stopped');
                stop = true;
            }

            for (var x = 0; x < window.frames.length; x++) {
                window.frames[x].postMessage(stop ? 'stop' : 'start', '*');
            }
        });
    };
})();