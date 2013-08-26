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

        function selectTimePeriod(element) {
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
        
        function setCustomPeriod() {
            var dialogElement = $('<div class="time-period-dialog">'
                    + '<label>Date: <input type="date" class="time-period-date" /></label>'
                    + '<label>Time: <input type="time" class="time-period-time" /></label>'
                    + '<label>Period: <select class="time-period-picker">' + element.html() + '</select>'
                    + '</label>' 
                + '</div>')
                .appendTo($('body'))
                .dialog({
                    title: 'Select Time Period',
                    resizable: false,
                    draggable: false,
                    modal: true,
                    close: closeDialog,
                    width: 400,
                    height: 240,
                    buttons: {
                        'Set': function () {
                            var start = $('.time-period-date', dialogElement).val() + 'T' + $('.time-period-time', dialogElement).val() + ':00Z';

                            window.location = buildUrl(timePeriodPickerElement.children(':selected:first').data('timeLimit') + '&start=' + start);
                        },
                        'Cancel': closeDialog
                    }
                });
            var timePeriodPickerElement = $('.time-period-picker', dialogElement);

            timePeriodPickerElement.children('option:last').remove();
            selectTimePeriod(timePeriodPickerElement);

            function closeDialog() {
                dialogElement.dialog('close').remove();
            }
        }

        element
            .on('change', function () {
                var timeLimit = $(this).children(':selected:first').data('timeLimit');
                
                if (timeLimit) {
                    window.location = buildUrl(timeLimit);
                    return;
                }

                setCustomPeriod();
                selectTimePeriod(element);
            });

        selectTimePeriod(element);
    };
})();