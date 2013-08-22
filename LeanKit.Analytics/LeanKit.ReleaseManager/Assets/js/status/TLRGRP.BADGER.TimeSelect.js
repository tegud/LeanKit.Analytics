(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.TimeSelect = function (element) {
        var request = new TLRGRP.BADGER.MetricsRequest();
        
        function buildUrl(timeLimit) {
            var metric = request.dashboard();
            var subMetric = request.subMetric();
            
            if (metric && subMetric) {
                return '/Status?metric=' + metric + '&subMetric=' + subMetric + '&' + timeLimit;
            }
            else if (metric) {
                return '/Status?metric=' + metric + '&' + timeLimit;
            }

            return '/Status?' + timeLimit;
        }

        element
            .on('change', function () {
                var timeLimit = $(this).children(':selected:first').data('timeLimit');
                window.location = buildUrl(timeLimit);
            })
            .children().each(function () {
                var currentItem = $(this);

                if (currentItem.data('timeLimit') === 'timePeriod=' + request.timePeriod()) {
                    currentItem.prop('selected', true);
                    return false;
                }
            });
    };
})();