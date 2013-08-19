(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.TimeSelect = function (element, options) {
        function buildUrl(timeLimit) {
            return '/Status?metric=' + options.currentMetric + '&' + timeLimit;
        }

        element
            .on('change', function () {
                var timeLimit = $(this).children(':selected:first').data('timeLimit');
                window.location = buildUrl(timeLimit);
            })
            .children().each(function () {
                var currentItem = $(this);

                if (currentItem.data('timeLimit') === options.currentTimeString) {
                    currentItem.prop('selected', true);
                    return false;
                }
            });
    };
})();