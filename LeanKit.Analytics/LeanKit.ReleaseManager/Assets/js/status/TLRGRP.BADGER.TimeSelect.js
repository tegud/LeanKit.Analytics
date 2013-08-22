(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.TimeSelect = function (element, currentTimePeriod) {
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

        function selectTimePeriod() {
            element
                .children().each(function() {
                    var currentItem = $(this);

                    if (currentItem.data('timeLimit') === 'timePeriod=' + currentTimePeriod) {
                        currentItem.prop('selected', true);
                        return false;
                    }

                    return true;
                });
        }

        element
            .on('change', function () {
                var timeLimit = $(this).children(':selected:first').data('timeLimit');
                window.location = buildUrl(timeLimit);
            });

        selectTimePeriod();
    };
})();